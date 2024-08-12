using UnityEngine;

[CreateAssetMenu(fileName = "Character State", menuName = "CHR State / Character State")]
public class CharacterStateSOJ : ScriptableObject
{
    [SerializeField] protected float maxhp;
    [SerializeField] protected float damage;
    [SerializeField] protected float acting;
    [SerializeField] protected float ultimateGauge;
    [SerializeField] protected bool isUltimateAttack;
    [SerializeField] protected Sprite playerIcon;
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] protected Sprite playerFullBody;

    public float Maxhp { get => maxhp; set => maxhp = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Acting { get => acting; set => acting = value; }
    public float UltimateGauge { get => ultimateGauge; set => ultimateGauge = value; }
    public bool IsUltimateAttack { get => isUltimateAttack; set => isUltimateAttack = value; }
    public Sprite PlayerIcon { get => playerIcon; set => playerIcon = value; }
    public GameObject PlayerPrefab { get => playerPrefab; set => playerPrefab = value; }
    public Sprite PlayerFullBody { get => playerFullBody; set => playerFullBody = value; }
}
