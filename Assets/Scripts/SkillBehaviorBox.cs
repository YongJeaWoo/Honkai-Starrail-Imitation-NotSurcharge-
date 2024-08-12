using UnityEngine;

public class SkillBehaviorBox : FieldObject, IBox
{
    [Header("파티클 생성 위치")]
    [SerializeField] private Transform particleTrans;

    [Header("박스 오픈 파티클")]
    [SerializeField] private GameObject openParticlePrefab;

    [Header("박스 파괴 파티클")]
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
        // UI 처리 필요
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
