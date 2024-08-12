using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSelectSystem : MonoBehaviour
{
    [Header("Ÿ�� ī�޶� ��ġ")]
    [SerializeField] private Transform targetCamPos;
    public Transform TargetCamPos { get => targetCamPos; set => targetCamPos = value; }
    
    private int selectedIndex = 0;
    public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }

    private bool isSelectingSupportTarget;
    public bool IsSelectingSupportTarget { get => isSelectingSupportTarget; set => isSelectingSupportTarget = value; }

    private Vector3 originCameraPos;
    private Quaternion originCameraRot;
    public Vector3 OriginCameraPos { get => originCameraPos; set => originCameraPos = value; }
    public Quaternion OriginCameraRot { get => originCameraRot; set => originCameraRot = value; }

    private BattleSystem battleSystem;
    private TargetUISystem targetUISystem;

    private void Awake()
    {
        InitValues();
    }

    private void Update()
    {
        var currentBattler = battleSystem.GetCurrentBattler() as BattleCharacterState;

        if (IsSelectingSupportTarget)
        {
            if (currentBattler != null && !currentBattler.IsUltimateReady())
            {
                HandleSupportSkillSelection();

                UpdateSupportTarget();
            }
        }
        else
        {
            if (currentBattler != null && !currentBattler.IsUltimateReady())
            {
                HandleTargetSelection();

                UpdateTarget();
            }
        }
    }

    private void InitValues()
    {
        battleSystem = transform.parent.GetComponent<BattleSystem>();
        targetUISystem = GetComponent<TargetUISystem>();
    }

    private void HandleTargetSelection()
    {
        var enemySystem = battleSystem.GetEnemySystem();
        var enemyList = enemySystem.GetEnemyList().Where(enemy => enemy.IsAlive).ToList();

        // ����ִ� ���� ������ �޼��带 �����մϴ�.
        if (enemyList.Count == 0)
        {
            targetUISystem.EnemyAllCycleUI(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex = (selectedIndex + 1) % enemyList.Count;
            UpdateTarget();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex = (selectedIndex - 1 + enemyList.Count) % enemyList.Count;
            UpdateTarget();
        }
    }

    private void HandleSupportSkillSelection()
    {
        var playerSystem = battleSystem.GetPlayerSystem();
        var playerList = playerSystem.GetPlayerList().Where(player => player.IsAlive).ToList();

        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex = (selectedIndex - 1) % playerList.Count;
            UpdateSupportTarget();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex = (selectedIndex + 1 + playerList.Count) % playerList.Count;
            UpdateSupportTarget();
        }
    }

    public void UpdateTarget()
    {
        var enemySystem = battleSystem.GetEnemySystem();
        var enemyList = enemySystem.GetEnemyList().Where(enemy => enemy.IsAlive).ToList();

        if (enemyList == null || enemyList.Count == 0)
        {
            Debug.Log("��ȿ���� ���� Ÿ�� ����Ʈ�Դϴ�.");
            return;
        }

        if (SelectedIndex < 0 || SelectedIndex >= enemyList.Count)
        {
            Debug.Log("��ȿ���� ���� Ÿ�� �ε����Դϴ�.");
            return;
        }

        var currentTarget = enemyList[selectedIndex];

        if (currentTarget != null && currentTarget.IsAlive)
        {
            targetUISystem.EnemyAllCycleUI(true);
            targetUISystem.PlayerAllCycleUI(false);
        }

        targetUISystem.EnemyTargeting(currentTarget, selectedIndex);
    }

    public void UpdateSupportTarget()
    {
        var playerList = battleSystem.GetPlayerSystem().GetPlayerList().Where(player => player.IsAlive).ToList();

        if (playerList == null || playerList.Count == 0)
        {
            Debug.Log("��ȿ���� ���� �÷��̾� ����Ʈ�Դϴ�.");
            return;
        }

        if (SelectedIndex < 0 || SelectedIndex >= playerList.Count)
        {
            Debug.Log("��ȿ���� ���� �÷��̾� �ε����Դϴ�.");
            return;
        }

        var currentTarget = playerList[selectedIndex];

        if (currentTarget != null)
        {
            targetUISystem.EnemyAllCycleUI(false);
            targetUISystem.PlayerAllCycleUI(true);
        }

        targetUISystem.PlayerTargeting(currentTarget, selectedIndex);
    }

    public int GetCurrentSelectedIndex()
    {
        // ���� ���õ� �ε����� ��ȯ�մϴ�.
        return selectedIndex;
    }

    public BattleSystem GetBattleSystem() => battleSystem;
}
