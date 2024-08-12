using System;
using UnityEngine;

public abstract class BattleBehaviourComponent : MonoBehaviour
{
    public event Action turnOver;

    #region Object Options
    protected float currentHp;
    protected float maxHp;
    protected float savedHp;
    protected float damage;
    protected float actionPoint;
    protected bool isAlive;
    protected float ultimateGauge;
    protected Sprite sprite;
    private Sprite playerUltimateSprite;
    [SerializeField] private Sprite ultimateSkillIcon;

    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float SavedHp { get => savedHp; set => savedHp = value; }
    public float ActionPoint { get => actionPoint; set => actionPoint = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public float UltimateGauge { get => ultimateGauge; set => ultimateGauge = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    protected Sprite PlayerUltimateSprite { get => playerUltimateSprite; set => playerUltimateSprite = value; }
    public Sprite UltimateSkillIcon { get => ultimateSkillIcon; set => ultimateSkillIcon = value; }

    #endregion

    protected BattleSystem battleSystem;

    protected BattleBehaviourComponent attackTarget;

    [HideInInspector]
    public BaseHealth health;

    private void Awake()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
        health = GetComponent<BaseHealth>();
    }

    public abstract void StartTurn();

    public void EventActionToTurnOver()
    {
        turnOver?.Invoke();
    }

    public BattleBehaviourComponent GetAttackTarget() => attackTarget;
    public float DecreaseActionPoint() => ActionPoint = 0f;

    public Vector3 GetTargetTransform()
    {
        return new Vector3(transform.position.x, 1f, transform.position.z);
    }
}
