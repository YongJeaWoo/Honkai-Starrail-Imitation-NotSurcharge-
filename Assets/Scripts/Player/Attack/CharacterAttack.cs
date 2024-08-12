using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CharacterAttack : MonoBehaviour
{
    protected readonly string attackAnimationText = "Attack";

    protected PanelActivate panelActivation;
    protected Animator animator;
    protected CharacterButtonInfo buttonInfo;

    protected bool isAttack;
    public bool IsAttack { get => isAttack; set => isAttack = value; }

    [Header("���� Ÿ�� ��ġ")]
    [SerializeField] protected Transform attackTransform;

    [Header("���� ���� �ð�")]
    [SerializeField] protected float delayTime;

    [Header("���� ��ƼŬ")]
    [SerializeField] protected TrailRenderer attackTrail;

    [Header("���ռҸ�")]
    [SerializeField] protected AudioClip attackSound;

    [Header("���� SFX")]
    [SerializeField] protected AudioClip attackSFXSound;

    protected float time;

    protected bool inputAttackKey;
    protected bool skillButtonKey;

    protected void GetComponents()
    {
        panelActivation = PopupManager.Instance.GetPanelActivation();
        animator = transform.parent.GetComponent<Animator>();
        buttonInfo = GetComponent<CharacterButtonInfo>();
    }

    private void GetMouseButtonAttackKey()
    {
        inputAttackKey = Input.GetMouseButtonDown(0);
        skillButtonKey = Input.GetKeyDown(KeyCode.E);
    }

    protected void Attack()
    {
        if (panelActivation.IsAnyPanelActive())
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            return;
        }

        GetMouseButtonAttackKey();

        time += Time.deltaTime;

        if (delayTime > time) return;

        if (inputAttackKey && !IsAttack)
        {
            time = 0;
            animator.SetTrigger(attackAnimationText);
        }

        if (skillButtonKey && !IsAttack)
        {
            if (FieldState.gauge < 0) return;

            SkillButtonSet();
            time = 0;
        }
    }

    private void SkillButtonSet()
    {
        buttonInfo.ButtonState = E_ButtonType.EButton;
    }

    public Animator GetAnimator() => animator;

    public abstract void ShowAttackTrailEvent();
    public abstract void HideAttackTrailEvent();
    public abstract void AttackHitAnimationEvent();
}
