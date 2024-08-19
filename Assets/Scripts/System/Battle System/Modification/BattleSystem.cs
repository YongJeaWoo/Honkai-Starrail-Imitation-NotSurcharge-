using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private readonly string deadPanel = $"Player All Dead Panel";

    private Camera mainCam;
    private List<BattleBehaviourComponent> turnBattlers;
    private BattleBehaviourComponent currentBattler;

    private PlayerBattleSystem playerBattleSystem;
    private EnemyBattleSystem enemyBattleSystem;
    private UiSystem uiSystem;

    private TargetUISystem targetUISystem;

    private void Awake()
    {
        InitValues();
    }

    private void InitValues()
    {
        mainCam = Camera.main;
        playerBattleSystem = GetComponent<PlayerBattleSystem>();
        enemyBattleSystem = GetComponent<EnemyBattleSystem>();
        uiSystem = GetComponent<UiSystem>();
        targetUISystem = GetComponentInChildren<TargetUISystem>();
        turnBattlers = new List<BattleBehaviourComponent>();
        
        InitOtherSystem();
    }

    private void InitOtherSystem()
    {
        playerBattleSystem.InitBattlePlayer();
        enemyBattleSystem.InitBattleEnemy();
        playerBattleSystem.SetButton();
        uiSystem.SetUIPlayerData(playerBattleSystem.GetPlayerList());

        turnBattlers.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<BattleBehaviourComponent>());

        // �� �� �÷��̾� UI �ʱ�ȭ
        targetUISystem.InitializeEnemyUI(enemyBattleSystem.GetEnemyList().Count);
        targetUISystem.InitializePlayerUI(playerBattleSystem.GetPlayerList().Count);
    }

    public void SetAttackTurn(E_TurnBase turnType)
    {
        foreach (var battler in turnBattlers)
        {
            if (turnType == E_TurnBase.Player && battler is BattleCharacterState)
            {
                (battler as BattleCharacterState).DecreaseActionPoint();
            }
            else if (turnType == E_TurnBase.Enemy && battler is BattleFSMController)
            {
                battler.DecreaseActionPoint();
            }
        }

        AdvanceTurn();
    }

    protected virtual void AdvanceTurn()
    {
        PlayerAllDead();

        // ���� ���� �̺�Ʈ ���� ����
        if (currentBattler != null)
        {
            currentBattler.turnOver -= AdvanceTurn;
        }

        if (enemyBattleSystem.GetEnemyList().All(b => !b.IsAlive))
        {
            currentBattler.turnOver -= AdvanceTurn;

            enemyBattleSystem.OnPageCleared();

            return;
        }

        // ���� ���� ������ ��� ��ü�� �ൿ���� ���ҽ�Ű�� ����
        foreach (var battler in turnBattlers)
        {
            float decreaseAmount = battler.ActionPoint / turnBattlers.Count; // �ൿ�� ���� ����

            battler.ActionPoint -= decreaseAmount;
        }

        turnBattlers.Sort((x, y) => x.ActionPoint.CompareTo(y.ActionPoint));

        for (int i = 0; i < turnBattlers.Count; i++)
        {
            BattleBehaviourComponent battler = turnBattlers[i];
        }

        // ���� �ൿ���� ���� ĳ���͸� ����, ���� ĳ���ʹ� �ǳʶ�
        currentBattler = turnBattlers.FirstOrDefault(b => b.IsAlive);

        // ���õ� ĳ������ �� ����
        if (currentBattler != null)
        {
            targetUISystem.EnemyAllCycleUI(false);

            // ���õ� ĳ������ ���� ����
            currentBattler.StartTurn();

            // Player�� ��� ���̸� Ÿ���� UI�� ������Ʈ�ϰ� ǥ��
            if (currentBattler is BattleCharacterState characterState)
            {
                playerBattleSystem.PlayerTurnStart();
                // �ñر� �غ� ���¸� Ȯ���Ͽ� UI ����
                if (characterState.IsUltimateReady())
                {
                    uiSystem.ShowUltimateUI(characterState.GatPlayerIndex());
                }
                else
                {
                    uiSystem.ShowAttackUI();
                }
            }
            else
            {
                targetUISystem.EnemyAllCycleUI(false);
                targetUISystem.PlayerAllCycleUI(false);
                playerBattleSystem.PlayerTurnEnd();
            }

            // ���� ���� ���� �̺�Ʈ ����
            currentBattler.turnOver += AdvanceTurn;
        }
    }

    public void MoveCameraToPosition(Vector3 position, Vector3 lookAtPosition)
    {
        mainCam.transform.position = position;
        mainCam.transform.LookAt(lookAtPosition);
    }

    public void BattleEnd()
    {
        BattleEntryManager.Instance.BattleEndCoroutine();
    }

    public void PlayerAllDead()
    {
        var players = playerBattleSystem.GetPlayerList();

        bool allDead = true;

        foreach (var player in players)
        {
            var state = player.GetComponent<BattleCharacterState>();

            if (state.IsAlive)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            PopupManager.Instance.InstantPopUp(deadPanel);
        }
    }

    public PlayerBattleSystem GetPlayerSystem() => playerBattleSystem;
    public EnemyBattleSystem GetEnemySystem() => enemyBattleSystem;
    public BattleBehaviourComponent GetCurrentBattler() => currentBattler;
    public List<BattleBehaviourComponent> GetTurnList() => turnBattlers;
    public UiSystem GetUiSystem() => uiSystem;

    public void GatAdvanceTurn() => AdvanceTurn();
}


