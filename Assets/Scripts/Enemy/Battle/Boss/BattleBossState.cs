using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleBossState : MonoBehaviour
{
    // ���� ���ѻ��±�� ��Ʈ�ѷ�
    protected BattleBossFSMController controller;

    // �ִϸ����� ������Ʈ
    protected Animator animator;

    // ���� ���� ���� �������̽�(�����ƴ�) �޼ҵ� ����

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void EnterState(E_BattleBossState state);

    // ���� ���� ������Ʈ �߻� �޼ҵ� (���� ���� ����)
    public abstract void UpdateState();

    // ���� ���� ���� (�ٸ����·� ���̵�) �޼ҵ�
    public abstract void ExitState();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<BattleBossFSMController>();
    }
}
