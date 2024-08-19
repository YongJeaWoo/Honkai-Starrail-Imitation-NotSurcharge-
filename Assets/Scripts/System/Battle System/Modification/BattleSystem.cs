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

        // 적 및 플레이어 UI 초기화
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

        // 이전 턴의 이벤트 구독 해제
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

        // 턴이 끝날 때마다 모든 객체의 행동력을 감소시키는 로직
        foreach (var battler in turnBattlers)
        {
            float decreaseAmount = battler.ActionPoint / turnBattlers.Count; // 행동력 감소 비율

            battler.ActionPoint -= decreaseAmount;
        }

        turnBattlers.Sort((x, y) => x.ActionPoint.CompareTo(y.ActionPoint));

        for (int i = 0; i < turnBattlers.Count; i++)
        {
            BattleBehaviourComponent battler = turnBattlers[i];
        }

        // 가장 행동력이 낮은 캐릭터를 선택, 죽은 캐릭터는 건너뜀
        currentBattler = turnBattlers.FirstOrDefault(b => b.IsAlive);

        // 선택된 캐릭터의 턴 시작
        if (currentBattler != null)
        {
            targetUISystem.EnemyAllCycleUI(false);

            // 선택된 캐릭터의 턴을 시작
            currentBattler.StartTurn();

            // Player의 대기 턴이면 타겟팅 UI를 업데이트하고 표시
            if (currentBattler is BattleCharacterState characterState)
            {
                playerBattleSystem.PlayerTurnStart();
                // 궁극기 준비 상태를 확인하여 UI 변경
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

            // 다음 턴을 위해 이벤트 구독
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


