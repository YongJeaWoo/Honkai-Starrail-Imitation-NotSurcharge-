using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleSystem : MonoBehaviour
{
    private List<BattleBehaviourComponent> enemyCharacters = new List<BattleBehaviourComponent>();
    private List<GameObject> enemyPrefabs;
    // ��ü �� ��
    private int totalEnemies;

    // ���� ������
    private int currentPage;

    // �������� �� ��
    private int enemiesPerPage;

    // ���ݱ��� ������ �� ��
    private int enemiesCreated;

    private string nextStage = $"Next Battle Panel";

    [SerializeField] private Transform[] enemyPos;

    [SerializeField] private BattleSystem battleSystem;

    // ���� Ÿ�� ����Ʈ
    private List<BattleBehaviourComponent> selectableTargetsList = new List<BattleBehaviourComponent>();

    // ���� �ʱ�ȭ
    public void InitBattleEnemy()
    {
        enemyPrefabs = BattleEntryManager.Instance.GetBattleEnemies();
        totalEnemies = BattleEntryManager.Instance.GetEnemiesCount();
        enemiesPerPage = enemyPos.Length;
        currentPage = 0;
        enemiesCreated = 0;

        // ù ������ �� ����
        SpawnEnemiesForPage();
    }

    // ���� �������� �� ����
    private void SpawnEnemiesForPage()
    {
        // ���� �������� ���� ����
        foreach (var enemy in enemyCharacters)
        {
            Destroy(enemy.gameObject);
        }

        enemyCharacters.Clear();

        int startIdx = enemiesCreated;
        int endIdx = Mathf.Min(enemiesCreated + enemiesPerPage, totalEnemies);

        for (int i = startIdx; i < endIdx; i++)
        {
            int prefabIndex = Random.Range(0, enemyPrefabs.Count); // ������ ������ ����
            GameObject prefab = enemyPrefabs[prefabIndex];

            int enemyPosIndex = i - startIdx; // ��ġ �ε����� ����� ��, ���� �������� ���� �ε����� ���

            if (enemyPosIndex >= enemyPos.Length)
            {
                continue; // �ε����� �迭�� ����� ������ �ǳʶݴϴ�.
            }

            GameObject enemy = Instantiate(prefab, enemyPos[enemyPosIndex].position, prefab.transform.rotation, enemyPos[enemyPosIndex]); // �������� �����̼� ���
            BattleFSMController enemyController = enemy.GetComponent<BattleFSMController>();
            if (enemyController != null)
            {
                enemyCharacters.Add(enemyController);
                enemyController.InitValues();
            }
        }

        enemiesCreated += endIdx - startIdx;
    }

    // ���� �������� ���� ��� ���ŵǾ��� �� ȣ��
    public void OnPageCleared()
    {
        if (enemiesCreated < totalEnemies)
        {
            currentPage++;
            // ������ ��ȯ �� ���� �������� �� ���� �ʱ�ȭ
            StartCoroutine(NextCoroutine(currentPage));
            SpawnEnemiesForPage();
            battleSystem.GatAdvanceTurn();
            return;
        }

        battleSystem.BattleEnd();
    }

    private IEnumerator NextCoroutine(int page)
    {
        var next = PopupManager.Instance.InstantPopUp(nextStage);
        var panel = next.GetComponentInChildren<AlramPanel>();
        panel.SetAlramText($"{page + 1} ��������");
        yield return null;
    }

    public Vector3 GetEnemiesMidPoint()
    {
        Vector3 sumPositions = Vector3.zero;

        int count = 0;

        foreach (var enemy in enemyCharacters)
        {
            if (enemy != null && enemy.IsAlive)
            {
                sumPositions += enemy.transform.position;
                count++;
            }
        }

        if (count == 0)
        {
            return Vector3.zero;
        }

        return sumPositions / count;
    }

    public List<BattleBehaviourComponent> GetEnemyList() => enemyCharacters;

    // ���� Ÿ�� ����Ʈ�� �������� �޼ҵ�
    public List<BattleBehaviourComponent> GetSelectTargetList()
    {
        // �÷��̾� �ý����� �÷��̾� ����Ʈ�� �����ͼ� selectableTargetsList�� �߰�
        var playerList = battleSystem.GetPlayerSystem().GetPlayerList();
        selectableTargetsList.AddRange(playerList);

        return selectableTargetsList;
    }

    public Transform[] GetEnemyPos() => enemyPos;

    // ��ȯ ������ �ڸ��� �ִ��� Ȯ��
    public bool HasEmptySlots()
    {
        // ���� ��ȯ�� ���� ���� Ȯ��
        int activeEnemies = enemyCharacters.Count;

        // ��ü �� ��ȯ �ڸ� ���� ��
        bool hasEmptySlots = activeEnemies < enemyPos.Length;

        // �� ������ ������ true ��ȯ
        return hasEmptySlots;
    }
}
