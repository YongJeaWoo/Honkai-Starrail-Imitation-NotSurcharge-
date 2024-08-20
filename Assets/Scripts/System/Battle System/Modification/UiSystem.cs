using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UiSystem : MonoBehaviour
{
    [Header("플레이어 UI 목록")]
    [SerializeField] private GameObject playerUI;
    private CharacterArea[] areas;

    private List<BattleBehaviourComponent> players;

    [Header("행동력 UI")]
    [SerializeField] private TextMeshProUGUI actingPointText;
    [SerializeField] private Image[] actionPointImages;

    private PlayerBattleSystem playerBattleSystem;

    [Header("궁극기 연출 UI")]
    [SerializeField] private Image ultimateImage; // 궁극기 애니메이션을 띄울 이미지
    [SerializeField] private float animationDuration = 0.5f; // 애니메이션 지속 시간
    private Vector3 startPosition;
    private Vector3 endPosition;

    [Header("보스 UI 목록")]
    [SerializeField] private GameObject bossUI;

    private void Awake()
    {
        playerBattleSystem = GetComponent<PlayerBattleSystem>();

        // 초기 위치와 목표 위치 설정 (예: 왼쪽에서 오른쪽으로 이동)
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

        // 체력 설정
        var hpSlider = areas[number].GetHpSlider();
        var effectSlider = areas[number].GetEffectSlider();
        var health = players[number].GetComponent<PlayerHealth>();
        hpSlider.value = players[number].health.GetCurrentHp();

        // 체력 텍스트 설정
        var hpText = areas[number].GetHpText();
        hpText.text = players[number].health.GetCurrentHp().ToString("F0");
        health.SetInitSliders(hpSlider, effectSlider, hpText);
        
        // 쿨타임 이미지 설정
        var coolImage = areas[number].GetCoolTimeImage();
        coolImage.fillAmount = players[number].UltimateGauge;

        var uiSystem = players[number].GetComponent<BattleCharacterState>();
        uiSystem.SetUiSystem(number);

        // 궁극기 버튼 설정 및 이벤트 연결
        var ultimateButton = GetParents(coolImage.transform, 2).GetComponent<Button>();
        // 게이지 다 찾을때만 활성화
        ultimateButton.interactable = coolImage.fillAmount >= 1;
        // 버튼 클릭 시 해당 캐릭터의 번호를 전달
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

    // 궁극기 UI로 바꿔줄 메소드
    public void ChangeUltimateButton(int playerIndex)
    {
        var player = players[playerIndex];
        var state = player.GetComponent<BattleCharacterState>();

        state.SetUltimateReady(true);
        player.ActionPoint = 0;

        //ControllActingPoint(true); // 행동력 UI 업데이트
    }

    // 공격 UI를 표시하는 메소드
    public void ShowAttackUI()
    {
        var buttons = GetComponent<PlayerBattleSystem>();
        buttons.GatAttackUI().SetActive(true);
        buttons.GatUltimateUI().SetActive(false);
    }

    // 궁극기 UI를 표시하는 메소드
    public void ShowUltimateUI(int playerIndex)
    {
        var buttons = GetComponent<PlayerBattleSystem>();
        var targetUISystem = GetComponentInChildren<TargetUISystem>();

        buttons.GatAttackUI().SetActive(false);
        buttons.GatUltimateUI().SetActive(true);

        var state = players[playerIndex].GetComponent<BattleCharacterState>();

        // 캐릭터 타입에 따라 적절한 UI를 켜기
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

    // 화면 전환 대기 코루틴
    private IEnumerator UpdateUIAfterTransition(TargetUISystem targetUISystem, List<BattleBehaviourComponent> targets, bool isPlayer)
    {
        // 화면 전환이 완료되기를 대기합니다.
        yield return new WaitForSeconds(0.1f); // 필요에 따라 적절한 시간 조정

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

    // 궁극기 사용 후 공격 UI 변경 메소드
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

    // 궁극기 사용 메소드
    public void UseUltimate(Sprite ultimateSprite)
    {
        // 스프라이트 설정
        ultimateImage.sprite = ultimateSprite;

        // 이미지 위치 초기화 및 활성화
        ultimateImage.transform.localPosition = startPosition;
        ultimateImage.gameObject.SetActive(true);

        // 코루틴 시작
        StartCoroutine(AnimateUltimate());
    }

    // 궁극기 애니메이션 코루틴
    private IEnumerator AnimateUltimate()
    {
        float elapsedTime = 0;

        // 위치 이동 애니메이션
        while (elapsedTime < animationDuration)
        {
            ultimateImage.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 고정
        ultimateImage.transform.localPosition = endPosition;

        // 일정 시간 대기 (궁극기 효과 시간)
        yield return new WaitForSeconds(0.5f);

        // 사라지는 애니메이션
        elapsedTime = 0;

        // 초기 위치로 돌아가며 비활성화
        ultimateImage.gameObject.SetActive(false);
    }
}

