public class EnemyHealth : BaseHealth
{
    private BattleEnemyFSMController fsm;
    
    private void Start()
    {
        InitializeSOJData();
    }

    public void InitializeSOJData()
    {
        fsm = GetComponent<BattleEnemyFSMController>();
        var soj = fsm.GetSOJData();
        maxHp = soj.Maxhp;
        currentHp = maxHp;

        if (effectSlider != null)
        {
            currentHealthSlider.value = currentHp;
            hpText.text = currentHp.ToString();
        }
    }

    public override void HpDown(float value)
    {
        base.HpDown(value);

        if (currentHp <= 0)
        {
            currentHealthSlider.gameObject.SetActive(false);
        }
    }
}
