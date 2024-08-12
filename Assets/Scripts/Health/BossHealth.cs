using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : BaseHealth
{
    private BattleBossFSMController fsm;

    private void Start()
    {
        InitializeSOJData();
    }

    public void InitializeSOJData()
    {
        fsm = GetComponent<BattleBossFSMController>();
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
