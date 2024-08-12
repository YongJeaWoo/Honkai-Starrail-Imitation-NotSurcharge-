using System.Collections;
using UnityEngine;

public class HpUpPotion : MonoBehaviour, IConsumable
{
    [Header("¼öÄ¡ °ª")]
    [SerializeField] private float hpUpValue;    

    private PlayerHealth playerHealth;

    public bool isUse { get; set; }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isUse = false;
    }

    public void CheckItem()
    {
        var playerSystem = FindObjectOfType<SetPlayerSystem>();
        var player = playerSystem.GetPlayer();
        playerHealth = player.GetComponent<PlayerHealth>();

        var health = playerHealth.GetCurrentHp();
        var maxHealth = playerHealth.GetMaxHp();

        if (health >= maxHealth)
        {
            isUse = false;
            StartCoroutine(SelfDestroyObject());
        }
        else
        {
            isUse = true;
            StartCoroutine(UseBehaviour());
        }
    }

    private IEnumerator UseBehaviour()
    {
        var maxHp = playerHealth.GetMaxHp();
        var currentHp = playerHealth.GetCurrentHp();

        playerHealth.SetCurrentHp(currentHp + (maxHp * hpUpValue));

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        yield break;
    }

    private IEnumerator SelfDestroyObject()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
        yield break;
    }
}
