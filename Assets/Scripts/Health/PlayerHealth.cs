using System.Diagnostics;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    private BattleCharacterState state;
    private float saveHp;

    private void Start()
    {
        InitializeSOJData();
    }

    public void SetFieldSlider(Slider currentSlider)
    {
        currentHealthSlider = currentSlider;
    }

    public void InitializeSOJData()
    {
        state = GetComponent<BattleCharacterState>();
        var sojData = state.GetSOJData();
        maxHp = sojData.Maxhp;

        if (PlayerDataManager.Instance.GetEventSystem().GetFirstEvent())
        {
            currentHp = maxHp;
        }

        if (effectSlider != null)
        {
            currentHealthSlider.value = currentHp;
            hpText.text = currentHp.ToString();
        }
    }

    public float GetSaveHealth() => saveHp;
}
