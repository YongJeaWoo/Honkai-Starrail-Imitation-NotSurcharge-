using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UiSystem : MonoBehaviour
{
    [Header("�÷��̾� UI ���")]
    [SerializeField] private GameObject playerUI;
    private CharacterArea[] areas;

    private List<BattleBehaviourComponent> players;

    [Header("�ൿ�� UI")]
    [SerializeField] private TextMeshProUGUI actingPointText;
    [SerializeField] private Image[] actionPointImages;

    private PlayerBattleSystem playerBattleSystem;

    [Header("�ñر� ���� UI")]
    [SerializeField] private Image ultimateImage; // �ñر� �ִϸ��̼��� ��� �̹���
    [SerializeField] private float animationDuration = 0.5f; // �ִϸ��̼� ���� �ð�
    private Vector3 startPosition;
    private Vector3 endPosition;

    [Header("���� UI ���")]
    [SerializeField] private GameObject bossUI;

    private void Awake()
    {
        playerBattleSystem = GetComponent<PlayerBattleSystem>();

        // �ʱ� ��ġ�� ��ǥ ��ġ ���� (��: ���ʿ��� ���������� �̵�)
        startPosition = new Vector3(-Screen.width / 4, 0, 0);
        endPosition = new Vector3(0, 0, 0);
    }

    public void SetUIPlayerData(List<BattleBehaviourComponent> _players)
    {
        players = _players;
        SetBattleUI();
    }

    public bool isSkillPosible()
    {
        var point = playerBattleSystem.GetActionPoint();
        if (point <= 0) return false;
        else return true;
    }

    public void ControllActingPoint(bool isSkill)
    {
        var point = playerBattleSystem.CheckActionPoint();
        actingPointText.text = $"{point}";

        UpdateActingPointImage(isSkill);
    }

    private void UpdateActingPointImage(bool isSkill)
    {
        var point = playerBattleSystem.CheckActionPoint();

        if (isSkill)
        {
            Animator anim = actionPointImages[point].gameObject.GetComponent<Animator>();
            anim.SetBool("isEnable", false);
        }
        else
        {
            Animator anim = actionPointImages[point - 1].gameObject.GetComponent<Animator>();
            anim.SetBool("isEnable", true);
        }
    }

    private void SetBattleUI()
    {
        areas = playerUI.GetComponentsInChildren<CharacterArea>();

        foreach (var area in areas)
        {
            area.gameObject.SetActive(false);
        }

        for (int i = 0; i < players.Count; i++)
        {
            SetUIPlayers(i);
        }
    }

    private Transform GetChilds(Transform transform, int index)
    {
        Transform trans = transform;
        for (int i = 0; i < index; i++)
            trans = trans.GetChild(0);

        return trans;
    }

    private Transform GetParents(Transform transform, int index)
    {
        Transform trans = transform;
        for (int i = 0; i < index; i++)
            trans = trans.parent;

        return trans;
    }

    private void SetUIPlayers(int number)
    {
        var imagePos = playerUI.transform.GetChild(number);
        imagePos.GetChild(1).GetComponent<Image>().sprite = players[number].Sprite;
        GetChilds(imagePos, 3).GetComponent<Image>().sprite = players[number].UltimateSkillIcon;

        // ü�� ����
        var hpSlider = areas[number].GetHpSlider();
        var effectSlider = areas[number].GetEffectSlider();
        var health = players[number].GetComponent<PlayerHealth>();
        hpSlider.value = players[number].health.GetCurrentHp();

        // ü�� �ؽ�Ʈ ����
        var hpText = areas[number].GetHpText();
        hpText.text = players[number].health.GetCurrentHp().ToString("F0");
        health.SetInitSliders(hpSlider, effectSlider, hpText);
        
        // ��Ÿ�� �̹��� ����
        var coolImage = areas[number].GetCoolTimeImage();
        coolImage.fillAmount = players[number].UltimateGauge;

        var uiSystem = players[number].GetComponent<BattleCharacterState>();
        uiSystem.SetUiSystem(number);

        // �ñر� ��ư ���� �� �̺�Ʈ ����
        var ultimateButton = GetParents(coolImage.transform, 2).GetComponent<Button>();
        // ������ �� ã������ Ȱ��ȭ
        ultimateButton.interactable = coolImage.fillAmount >= 1;
        // ��ư Ŭ�� �� �ش� ĳ������ ��ȣ�� ����
        ultimateButton.onClick.AddListener(() => ChangeUltimateButton(number));

        areas[number].gameObject.SetActive(true);
    }

    public void SetBossUI()
    {
        var bossHealth = FindObjectsOfType<BossHealth>().FirstOrDefault(bh => bh.gameObject.activeSelf);

        if (bossHealth != null)
        {
            var bossSlider = bossUI.GetComponent<Slider>();
            var effectSlider = bossSlider.transform.GetChild(1).GetComponent<Slider>();
            var hpTextTransform = bossSlider.transform.GetChild(1).GetChild(0).GetChild(2);
            var hpText = hpTextTransform.GetComponent<TextMeshProUGUI>();

            bossHealth.SetInitSliders(bossSlider, effectSlider, hpText);

            var currentHp = bossHealth.GetCurrentHp();

            bossUI.SetActive(true);

            hpText.text = currentHp.ToString("F0");
        }
    }

    public void RefreshHP(int playerIndex, float hp, float maxHp)
    {
        float resultHp = hp / maxHp;

        var hpSlider = areas[playerIndex].GetHpSlider();
        var hpText = areas[playerIndex].GetHpText();

        hpSlider.value = resultHp;
        hpText.text = hp.ToString("F0");
    }

    public void RefreshUltimateGauge(int playerIndex, float fillAmount)
    {
        var ultimateImage = areas[playerIndex].GetCoolTimeImage();
        ultimateImage.fillAmount += Mathf.Clamp(ultimateImage.fillAmount + fillAmount, 0, 1);

        var state = players[playerIndex].GetComponent<BattleCharacterState>();

        var ultimateButton = GetParents(ultimateImage.transform, 2).GetComponent<Button>();

        if (ultimateImage.fillAmount >= 1)
        {
            ultimateButton.interactable = true;
        }
        else
        {
            ultimateButton.interactable = false;
        }
    }

    // �ñر� UI�� �ٲ��� �޼ҵ�
    public void ChangeUltimateButton(int playerIndex)
    {
        var player = players[playerIndex];
        var state = player.GetComponent<BattleCharacterState>();

        state.SetUltimateReady(true);
        player.ActionPoint = 0;

        //ControllActingPoint(true); // �ൿ�� UI ������Ʈ
    }

    // ���� UI�� ǥ���ϴ� �޼ҵ�
    public void ShowAttackUI()
    {
        var buttons = GetComponent<PlayerBattleSystem>();
        buttons.GatAttackUI().SetActive(true);
        buttons.GatUltimateUI().SetActive(false);
    }

    // �ñر� UI�� ǥ���ϴ� �޼ҵ�
    public void ShowUltimateUI(int playerIndex)
    {
        var buttons = GetComponent<PlayerBattleSystem>();
        var targetUISystem = GetComponentInChildren<TargetUISystem>();

        buttons.GatAttackUI().SetActive(false);
        buttons.GatUltimateUI().SetActive(true);

        var state = players[playerIndex].GetComponent<BattleCharacterState>();

        // ĳ���� Ÿ�Կ� ���� ������ UI�� �ѱ�
        if (state.CharacterType() == E_CharacterType.Support)
        {
            var targetSelectSystem = GetComponentInChildren<TargetSelectSystem>();

            var playerList = targetSelectSystem.GetBattleSystem().GetPlayerSystem().GetPlayerList().Where(player => player.IsAlive).ToList();
            Camera.main.transform.position = targetSelectSystem.TargetCamPos.position;
            Camera.main.transform.rotation = targetSelectSystem.TargetCamPos.rotation;
            StartCoroutine(UpdateUIAfterTransition(targetUISystem, playerList, true));
        }
        else if (state.CharacterType() == E_CharacterType.Dealer)
        {
            var targetSelectSystem = GetComponentInChildren<TargetSelectSystem>();

            var enemyList = targetSelectSystem.GetBattleSystem().GetEnemySystem().GetEnemyList().Where(enemy => enemy.IsAlive).ToList();
            StartCoroutine(UpdateUIAfterTransition(targetUISystem, enemyList, false));
        }
    }

    // ȭ�� ��ȯ ��� �ڷ�ƾ
    private IEnumerator UpdateUIAfterTransition(TargetUISystem targetUISystem, List<BattleBehaviourComponent> targets, bool isPlayer)
    {
        // ȭ�� ��ȯ�� �Ϸ�Ǳ⸦ ����մϴ�.
        yield return new WaitForSeconds(0.1f); // �ʿ信 ���� ������ �ð� ����

        if (isPlayer)
        {
            targetUISystem.ShowAlivePlayerUIs(targets);
            targetUISystem.EnemyAllCycleUI(false);
        }
        else
        {
            targetUISystem.ShowAliveEnemyUIs(targets);
            targetUISystem.PlayerAllCycleUI(false);
        }
    }

    // �ñر� ��� �� ���� UI ���� �޼ҵ�
    public void ReturnUltimateButton(int playerIndex)
    {
        PlayerBattleSystem Buttons = GetComponent<PlayerBattleSystem>();

        Buttons.GatAttackUI().SetActive(true);
        Buttons.GatUltimateUI().SetActive(false);

        var ultimateImage = areas[playerIndex].GetCoolTimeImage();
        ultimateImage.fillAmount = 0;

        var ultimateButton = GetParents(ultimateImage.transform, 2).GetComponent<Button>();
        ultimateButton.interactable = false;


        var state = players[playerIndex].GetComponent<BattleCharacterState>();
        state.SetUltimateReady(false);
    }

    // �ñر� ��� �޼ҵ�
    public void UseUltimate(Sprite ultimateSprite)
    {
        // ��������Ʈ ����
        ultimateImage.sprite = ultimateSprite;

        // �̹��� ��ġ �ʱ�ȭ �� Ȱ��ȭ
        ultimateImage.transform.localPosition = startPosition;
        ultimateImage.gameObject.SetActive(true);

        // �ڷ�ƾ ����
        StartCoroutine(AnimateUltimate());
    }

    // �ñر� �ִϸ��̼� �ڷ�ƾ
    private IEnumerator AnimateUltimate()
    {
        float elapsedTime = 0;

        // ��ġ �̵� �ִϸ��̼�
        while (elapsedTime < animationDuration)
        {
            ultimateImage.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ ����
        ultimateImage.transform.localPosition = endPosition;

        // ���� �ð� ��� (�ñر� ȿ�� �ð�)
        yield return new WaitForSeconds(0.5f);

        // ������� �ִϸ��̼�
        elapsedTime = 0;

        // �ʱ� ��ġ�� ���ư��� ��Ȱ��ȭ
        ultimateImage.gameObject.SetActive(false);
    }
}

