using UnityEngine;

public abstract class BattleEnemyState : MonoBehaviour
{
    // ���� ���ѻ��±�� ��Ʈ�ѷ�
    protected BattleEnemyFSMController controller;

    // �ִϸ����� ������Ʈ
    protected Animator animator;

    // ���� ���� ���� �������̽�(�����ƴ�) �޼ҵ� ����

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void EnterState(E_BattleEnemyState state);

    // ���� ���� ������Ʈ �߻� �޼ҵ� (���� ���� ����)
    public abstract void UpdateState();

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void ExitState();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<BattleEnemyFSMController>();
    }
}
