using UnityEngine;

public class CharacterButtonInfo : MonoBehaviour
{
    protected E_ButtonType buttonType = E_ButtonType.None;

    public delegate void PlayerButtonInfo(E_ButtonType button);
    public event PlayerButtonInfo onPlayerButton;

    private GameObject attackUIArea;
    public GameObject AttackUIArea { get => attackUIArea; set => attackUIArea = value; }

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        var playerUISystem = FindObjectOfType<SetPlayerUISystem>();
        playerUISystem.SetSkillAttackUIPlayer(transform.parent.gameObject);
    }

    public E_ButtonType ButtonState
    {
        get => buttonType;
        set => UseSkill(value);
    }

    private void UseSkill(E_ButtonType type)
    {
        buttonType = type;
        onPlayerButton?.Invoke(type);

        var parent = AttackUIArea.transform;

        switch (type)
        {
            case E_ButtonType.None:
                break;
            case E_ButtonType.RushButton:
                {
                    var button = parent.GetChild(0).GetComponent<PlayerUseButton>();
                    button.UseEffectButton();
                }
                break;
            case E_ButtonType.EButton:
                {
                    var button = parent.GetChild(1).GetComponent<PlayerUseButton>();
                    button.UseEffectButton();

                    SkillBehaviour();
                }
                break;
        }
    }

    private void SkillBehaviour()
    {
        var skill = GetComponent<FieldSkill>();
        skill.UseFieldSkill();
    }
}
