using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleSystem : MonoBehaviour
{
    private GameObject attackUI;
    private GameObject ultimateUI;

    private List<BattleBehaviourComponent> playerCharacter = new List<BattleBehaviourComponent>();

    public delegate void OnBasicButton();
    public event OnBasicButton onBasicAttackButton;

    public delegate void OnSkillButton();
    public event OnSkillButton onSkillAttackButton;

    public delegate void OnUltimateButton();
    public event OnUltimateButton onUltimateAttackButton;

    private Button basicAttackButton;
    private Button skillAttackButton;
    private Button ultimateAttackButton;

    private int actionPoint = 0;
    private int maxActionPoint = 5;

    [SerializeField] private Transform[] playerPos;
    [SerializeField] private Transform allTargetPos;


    public void CharacterFieldSet()
    {
        var player = playerCharacter;

        for (int i = 0; i < player.Count; i++)
        {
            var field = player[i].GetComponentInChildren<FieldComponent>();
            field.ToggleFieldObject(true);
            var playerMovement = player[i].GetComponentInChildren<CharacterMovement>();
            playerMovement.UIButtonSet();
            var health = player[i].GetComponent<PlayerHealth>();
            health.SetCurrentHp(player[i].CurrentHp);
        }

        playerCharacter.Clear();
    }

    public void SetButton()
    {
        attackUI = GameObject.Find("Attack Button Transform");

        basicAttackButton = attackUI.transform.GetChild(0).GetComponent<Button>();
        skillAttackButton = attackUI.transform.GetChild(1).GetComponent<Button>();

        ultimateUI = GameObject.Find("Ultimate Attack Button Transform");

        ultimateAttackButton = ultimateUI.transform.GetChild(0).GetComponent<Button>();

        basicAttackButton.onClick.AddListener(BasicAttackButton);
        skillAttackButton.onClick.AddListener(SkillAttackButton);
        ultimateAttackButton.onClick.AddListener(UltimateAttackButton);

        ultimateUI.SetActive(false);
    }

    #region Skill AddListener Button
    private void BasicAttackButton()
    {
        actionPoint++;
        onBasicAttackButton?.Invoke();
    }

    private void SkillAttackButton()
    {
        //actionPoint--;
        onSkillAttackButton?.Invoke();
    }

    private void UltimateAttackButton()
    {
        onUltimateAttackButton?.Invoke();
    }

    #endregion

    public void InitBattlePlayer()
    {
        CharacterStateSOJ[] playerStates = PlayerDataManager.Instance.GetPlayerSOJData();
        var battlers = PlayerDataManager.Instance.GetPlayerBattler();

        playerCharacter.Clear();

        for (int i = 0; i < battlers.Count; i++)
        {
            int battlerIndex = battlers[i];

            if (battlerIndex < playerStates.Length && playerStates[battlerIndex].PlayerPrefab != null)
            {
                GameObject player = Instantiate(playerStates[battlerIndex].PlayerPrefab, playerPos[i].position, Quaternion.identity, playerPos[i]);
                BattleCharacterState playerState = player.GetComponent<BattleCharacterState>();
                var field = player.GetComponentInChildren<FieldComponent>();
                field.ToggleFieldObject(false);

                if (playerState != null)
                {
                    playerCharacter.Add(playerState);
                    playerState.InitValues();
                    playerState.turnOver += PlayerTurnStart;
                }
            }
        }
    }

    public void PlayerTurnStart()
    {
        basicAttackButton.interactable = true;
        skillAttackButton.interactable = true;
    }

    public void PlayerTurnEnd()
    {
        basicAttackButton.interactable = false;
        skillAttackButton.interactable = false;
    }

    public int CheckActionPoint()
    {
        if (actionPoint > maxActionPoint)
        {
            actionPoint = maxActionPoint;
        }

        if (actionPoint <= 0)
        {
            actionPoint = 0;
        }

        return actionPoint;
    }

    public void SetActionPoint(bool isSkill)
    {
        if (!isSkill) actionPoint++;
        else actionPoint--;
    }

    public int GetActionPoint() => actionPoint;
    public List<BattleBehaviourComponent> GetPlayerList() => playerCharacter;
    public GameObject GatAttackUI() => attackUI;
    public GameObject GatUltimateUI() => ultimateUI;

    public Transform GetAllTargetPos() => allTargetPos;
}
