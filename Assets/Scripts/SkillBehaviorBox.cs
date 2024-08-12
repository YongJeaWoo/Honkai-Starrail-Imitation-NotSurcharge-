using UnityEngine;

public class SkillBehaviorBox : FieldObject, IBox
{
    [Header("��ƼŬ ���� ��ġ")]
    [SerializeField] private Transform particleTrans;

    [Header("�ڽ� ���� ��ƼŬ")]
    [SerializeField] private GameObject openParticlePrefab;

    [Header("�ڽ� �ı� ��ƼŬ")]
    [SerializeField] private GameObject destroyParticlePrefab;

    private Animator anim;
    private bool isOpen;
    private string openParam = "Open";

    private void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    public override void DetectiveBehaviour()
    {
        BoxOpen();
        isOpen = true;
    }

    public void BoxOpen()
    {
        if (isOpen) return;

        anim.SetTrigger(openParam);
        ItemUse();
    }

    public void ItemUse()
    {
        UpActivePoint();
    }

    private void UpActivePoint()
    {
        if (FieldState.gauge >= 3) return;

        UIUpdate();
    }

    private void UIUpdate()
    {
        // UI ó�� �ʿ�
        SkillButton ui = GameObject.FindAnyObjectByType<SkillButton>().GetComponent<SkillButton>();
        ui.HealItemUse();
    }

    public void OpenEffectEvent()
    {
        InteractiveParticle(openParticlePrefab);
    }

    public void BoxDestroy()
    {
        InteractiveParticle(destroyParticlePrefab);
        Destroy(gameObject);
    }

    public void InteractiveParticle(GameObject particle)
    {
        Instantiate(particle, particleTrans.position, Quaternion.identity);
    }
}
