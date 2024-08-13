using System.Collections;
using UnityEngine;

// 아이템 관련 스크립트 상속하기
public abstract class EnemyZone : ItemDrop
{
    [Header("재생성 시간")]
    [SerializeField] protected float respawnTimer;

    protected int enemyCount;

    protected bool allEnemiesDead;
    public bool AllEnemiesDead { get => allEnemiesDead; set => allEnemiesDead = value; }

    protected abstract void Spawn();
    public abstract void CallRespawn();
    protected abstract IEnumerator Respawn();

    public virtual int GetEnemiesCount()
    {
        return enemyCount;
    }
}
