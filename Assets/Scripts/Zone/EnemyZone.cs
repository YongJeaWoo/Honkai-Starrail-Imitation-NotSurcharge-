using System.Collections;
using UnityEngine;

// ������ ���� ��ũ��Ʈ ����ϱ�
public abstract class EnemyZone : ItemDrop
{
    [Header("����� �ð�")]
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
