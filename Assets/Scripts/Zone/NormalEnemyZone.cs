using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class NormalEnemyZone : EnemyZone
{
    [Header("생성할 적")]
    [SerializeField] private List<GameObject> enemies;
    [Header("생성할 위치")]
    [SerializeField] private Transform[] spawnPos;
    [Header("배틀용 적")]
    [SerializeField] private List<GameObject> battleEnemies;

    private List<GameObject> fieldEnemiesList;

    private float originRespawnTime;

    private int spawnCount;

    private bool hasDroppedItem;

    private void Start()
    {
        InitValues();
    }

    private void InitValues()
    {
        hasDroppedItem = false;
        originRespawnTime = respawnTimer;

        Spawn();
    }

    protected override void Spawn()
    {
        if (AllEnemiesDead) return;

        AllEnemiesDead = false;

        fieldEnemiesList = new List<GameObject>(enemies);

        spawnCount = Random.Range(1, spawnPos.Length + 1);
        enemyCount = spawnCount;
        HashSet<int> usedSpawnIndices = new HashSet<int>();

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, fieldEnemiesList.Count);
            GameObject enemyPrefab = fieldEnemiesList[randomIndex];
            fieldEnemiesList.Remove(enemyPrefab);

            if (fieldEnemiesList.Count == 0)
            {
                fieldEnemiesList = new List<GameObject>(enemies);
            }

            int spawnPosIndex;

            do
            {
                spawnPosIndex = Random.Range(0, spawnPos.Length);
            }
            while (usedSpawnIndices.Contains(spawnPosIndex));

            usedSpawnIndices.Add(spawnPosIndex);
            Transform pos = spawnPos[spawnPosIndex];

            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            Instantiate(enemyPrefab, pos.position, randomRot, pos);
        }
    }

    public override void CallRespawn()
    {
        StartCoroutine(Respawn());
    }

    protected override IEnumerator Respawn()
    {
        AllEnemiesDead = true;

        foreach (Transform spawnPoint in spawnPos)
        {
            foreach (Transform child in spawnPoint)
            {
                Destroy(child.gameObject);
            }
        }

        if (AllEnemiesDead && !hasDroppedItem)
        {
            Drop();
            hasDroppedItem = true;
        }

        if (hasDroppedItem)
        {
            while (respawnTimer > 0)
            {
                respawnTimer -= Time.deltaTime;
                yield return null;
            }

            AllEnemiesDead = false;
            Spawn();
            respawnTimer = originRespawnTime;
            hasDroppedItem = false;
        }
    }

    public bool GetAllEnemiesDead() => AllEnemiesDead;

    public override int GetEnemiesCount()
    {
        return base.GetEnemiesCount();
    }

    public List<GameObject> GetBattleEnemies()
    {
        return new List<GameObject>(battleEnemies);
    }

    public List<GameObject> GetFieldEnemies()
    {
        return fieldEnemiesList;
    }

    public Transform[] GetSpawnPos() => spawnPos;
}
