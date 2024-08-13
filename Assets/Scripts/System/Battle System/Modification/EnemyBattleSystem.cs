using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleSystem : MonoBehaviour
{
    private List<BattleBehaviourComponent> enemyCharacters = new List<BattleBehaviourComponent>();
    private List<GameObject> enemyPrefabs;
    // 전체 적 수
    private int totalEnemies;

    // 현재 페이지
    private int currentPage;

    // 페이지당 적 수
    private int enemiesPerPage;

    // 지금까지 생성된 적 수
    private int enemiesCreated;

    private string nextStage = $"Next Battle Panel";

    [SerializeField] private Transform[] enemyPos;

    [SerializeField] private BattleSystem battleSystem;

    // 때릴 타겟 리스트
    private List<BattleBehaviourComponent> selectableTargetsList = new List<BattleBehaviourComponent>();

    // 전투 초기화
    public void InitBattleEnemy()
    {
        enemyPrefabs = BattleEntryManager.Instance.GetBattleEnemies();
        totalEnemies = BattleEntryManager.Instance.GetEnemiesCount();
        enemiesPerPage = enemyPos.Length;
        currentPage = 0;
        enemiesCreated = 0;

        // 첫 페이지 적 생성
        SpawnEnemiesForPage();
    }

    // 현재 페이지의 적 생성
    private void SpawnEnemiesForPage()
    {
        // 현재 페이지의 적들 제거
        foreach (var enemy in enemyCharacters)
        {
            Destroy(enemy.gameObject);
        }

        enemyCharacters.Clear();

        int startIdx = enemiesCreated;
        int endIdx = Mathf.Min(enemiesCreated + enemiesPerPage, totalEnemies);

        for (int i = startIdx; i < endIdx; i++)
        {
            int prefabIndex = Random.Range(0, enemyPrefabs.Count); // 랜덤한 프리팹 선택
            GameObject prefab = enemyPrefabs[prefabIndex];

            int enemyPosIndex = i - startIdx; // 위치 인덱스를 계산할 때, 현재 페이지의 시작 인덱스를 고려

            if (enemyPosIndex >= enemyPos.Length)
            {
                continue; // 인덱스가 배열을 벗어나면 생성을 건너뜁니다.
            }

            GameObject enemy = Instantiate(prefab, enemyPos[enemyPosIndex].position, prefab.transform.rotation, enemyPos[enemyPosIndex]); // 프리팹의 로테이션 사용
            BattleFSMController enemyController = enemy.GetComponent<BattleFSMController>();
            if (enemyController != null)
            {
                enemyCharacters.Add(enemyController);
                enemyController.InitValues();
            }
        }

        enemiesCreated += endIdx - startIdx;
    }

    // 현재 페이지의 적이 모두 제거되었을 때 호출
    public void OnPageCleared()
    {
        if (enemiesCreated < totalEnemies)
        {
            currentPage++;
            // 페이지 전환 시 현재 페이지의 적 생성 초기화
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
        panel.SetAlramText($"{page + 1} 스테이지");
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

    // 때릴 타겟 리스트를 가져오는 메소드
    public List<BattleBehaviourComponent> GetSelectTargetList()
    {
        // 플레이어 시스템의 플레이어 리스트를 가져와서 selectableTargetsList에 추가
        var playerList = battleSystem.GetPlayerSystem().GetPlayerList();
        selectableTargetsList.AddRange(playerList);

        return selectableTargetsList;
    }

    public Transform[] GetEnemyPos() => enemyPos;

    // 소환 가능한 자리가 있는지 확인
    public bool HasEmptySlots()
    {
        // 현재 소환된 적의 수를 확인
        int activeEnemies = enemyCharacters.Count;

        // 전체 적 소환 자리 수와 비교
        bool hasEmptySlots = activeEnemies < enemyPos.Length;

        // 빈 슬롯이 있으면 true 반환
        return hasEmptySlots;
    }
}
