using UnityEngine;

public class CharacterMovement : MonoBehaviour, IHit
{
    private string moveText = "Move";
    private string isWalkingText = "Walk";
    private string isHitText = "Hit";

    private string customHorizontal = "Custom Horizontal";
    private string customVertical = "Custom Vertical";

    private Animator animator;
    private CharacterController controller;
    private Transform camTransform;
    private CharacterButtonInfo buttonInfo;
    private FastRunEffect fastRunEffect;
    private DashButton dashButton;
    private PanelActivate panelActivation;

    private Vector3 movement;
    private Vector3 direction;

    private float h, v;

    private bool isPanelActive = false;
    private bool isWalking = false;
    private bool walkButtonClick;
    private bool[] runButtonClick;

    private bool isFastRun;
    private bool isHit = false;
    public bool IsHit { get => isHit; set => isHit = value; }

    [Header("걷는 속도")]
    [SerializeField] private float walkSpeed;
    [Header("뛰는 속도")]
    [SerializeField] private float runSpeed;
    [Header("최대로 뛰는 속도")]
    [SerializeField] private float fastRunSpeed;
    private float currentSpeed;
    private float savedSpeed;

    private string infoText = "Player Info Panel";
    private string runText = $"달리기 모드로 변환되었습니다.";
    private string walkText = $"걷기 모드로 변환되었습니다.";

    [Header("회전 속도")]
    [SerializeField] private float rotSpeed;

    private void Start()
    {
        GetComponents();
        InitValue();
    }

    private void Update()
    {
        PanelActivation();
        GetAxisMethod();
        ToggleRunClick();
        ToggleWalkClick();
        Movement();
        UpdateAnimator();
    }

    // TODO : 배틀 종료 후 게임 씬 돌아갈 때 UIButtonSet의 문제
    #region Init Values
    private void GetComponents()
    {
        camTransform = Camera.main.transform;

        buttonInfo = GetComponent<CharacterButtonInfo>();
        animator = transform.parent.GetComponent<Animator>();
        controller = transform.parent.GetComponent<CharacterController>();
        fastRunEffect = GetComponent<FastRunEffect>();
        panelActivation = PopupManager.Instance.GetPanelActivation();
    }

    public void UIButtonSet()
    {
        var button = buttonInfo.AttackUIArea.transform.GetChild(0);
        dashButton = button.GetComponent<DashButton>();
    }

    private void InitValue()
    {
        currentSpeed = runSpeed;
        UIButtonSet();
    }
    #endregion

    #region CharacterMove

    private void PanelActivation()
    {
        bool isAnyPanelActive = panelActivation.IsAnyPanelActive();

        if (isAnyPanelActive && !isPanelActive)
        {
            savedSpeed = currentSpeed;
            currentSpeed = 0;
            isPanelActive = true;
        }
        else if (!isAnyPanelActive && isPanelActive)
        {
            currentSpeed = savedSpeed;
            isPanelActive = false;
        }
    }

    private void GetAxisMethod()
    {
        runButtonClick = new bool[2];

        h = Input.GetAxis(customHorizontal);
        v = Input.GetAxis(customVertical);

        runButtonClick[0] = Input.GetKeyDown(KeyCode.LeftShift);
        runButtonClick[1] = Input.GetMouseButtonDown(1);

        walkButtonClick = Input.GetKeyDown(KeyCode.LeftControl);
    }

    private void ToggleRunClick()
    {
        if (runButtonClick[0] || runButtonClick[1])
        {
            if (currentSpeed != fastRunSpeed)
            {
                if (movement.magnitude == 0) return;

                ToggleUISet(true);
                fastRunEffect.PlayFastRunEffect();
            }

            if (isWalking)
            {
                currentSpeed = fastRunSpeed;
                isWalking = false;
                animator.SetBool(isWalkingText, false);
            }
            else
            {
                currentSpeed = currentSpeed == runSpeed ? fastRunSpeed : runSpeed;

                if (currentSpeed != fastRunSpeed)
                {
                    ToggleUISet(false);
                }
            }
        }
    }

    private void ToggleUISet(bool isOn)
    {
        isFastRun = isOn;
        dashButton.IsRunCheck(isFastRun);
        buttonInfo.ButtonState = E_ButtonType.RushButton;
    }

    private void ToggleWalkClick()
    { 
        if (walkButtonClick)
        {
            if (panelActivation.IsAnyPanelActive()) return;

            var info = PopupManager.Instance.InstantPopUp(infoText);
            if (info == null) return;
            var infoPanel = info.GetComponentInChildren<AlramPanel>();

            if (isWalking)
            {
                infoPanel.SetAlramText(runText);
                currentSpeed = runSpeed;
                isWalking = false;
                animator.SetBool(isWalkingText, false);
            }
            else
            {
                infoPanel.SetAlramText(walkText);
                currentSpeed = walkSpeed;
                isWalking = true;
                animator.SetBool(isWalkingText, true);
            }
        }
    }

    private void Movement()
    {
        direction = new Vector3(h, 0, v).normalized;
        direction = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * direction;
        direction.Normalize();

        movement = currentSpeed * direction;
        controller.SimpleMove(movement);

        if (movement != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, targetRot, rotSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimator()
    {
        float moveMagnitude = movement.magnitude;

        if (moveMagnitude == 0f)
        {
            animator.SetBool(isWalkingText, false);
        }
        else if (isWalking)
        {
            animator.SetBool(isWalkingText, true);
        }

        float animatorValue = moveMagnitude > 0 ? (currentSpeed == runSpeed ? 0.5f : (currentSpeed == fastRunSpeed ? 1f : 0.5f)) : 0f;
        animator.SetFloat(moveText, animatorValue);
    }

    #endregion

    public void Hit()
    {
        if (IsHit) return;

        currentSpeed = 0;
        IsHit = true;
        animator.SetTrigger(isHitText);
        BattleEntryManager.Instance.FadeoutEntryBattle(E_TurnBase.Enemy);
    }

    public float SetCurrentSpeed(float value)
    {
        float previousSpeed = currentSpeed;
        currentSpeed = value;
        return previousSpeed;
    }
}
