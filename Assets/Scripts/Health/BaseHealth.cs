using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [Header("���� ü�� �����̴�")]
    [SerializeField] protected Slider currentHealthSlider;
    [Header("ȿ���� �����̴�")]
    [SerializeField] protected Slider effectSlider;
    [Header("ü�� �ؽ�Ʈ")]
    [SerializeField] protected TextMeshProUGUI hpText;
    [Header("lerp �ӵ�")]
    [SerializeField] protected float lerpSpeed = 0.8f;
    [Header("������ �ؽ�Ʈ")]
    [SerializeField] private GameObject alarmText;

    protected float currentHp;
    protected float maxHp;

    protected virtual void Update()
    {
        EffectSliderToCurrentHp();
    }

    private void EffectSliderToCurrentHp()
    { 
        float resultHp = currentHp / maxHp;

        if (currentHealthSlider != null)
        {
            if (currentHealthSlider.value != resultHp)
            {
                currentHealthSlider.value = resultHp;
            }
        }

        if (effectSlider != null)
        {
            if (currentHealthSlider.value != effectSlider.value)
            {
                effectSlider.value = Mathf.Lerp(effectSlider.value, resultHp, lerpSpeed * Time.deltaTime);
            }
        }
        else
        {
            return;
        }
    }

    public virtual void SetInitSliders(Slider currentSlider, Slider _effectSlider, TextMeshProUGUI text)
    {
        currentHealthSlider = currentSlider;
        effectSlider = _effectSlider;
        hpText = text;
    }

    public virtual void HpDown(float value)
    {
        if (currentHp <= 0) return;

        currentHp -= value;

        var go = Instantiate(alarmText, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = value.ToString();

        float resultHp = currentHp / maxHp;

        if (currentHealthSlider != null)
        {
            currentHealthSlider.value = resultHp;
        }

        BattleBehaviourComponent state = GetComponent<BattleBehaviourComponent>();

        if (state != null)
        {
            if (currentHp <= 0)
            {
                currentHp = 0;
            }

            hpText.text = currentHp.ToString();
        }
    }

    public virtual void HpUp(float value)
    { 
        currentHp += value;

        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        var go = Instantiate(alarmText, transform.position, Quaternion.identity);
        var textObj = go.GetComponent<TextMesh>();
        textObj.text = value.ToString();
        textObj.color = Color.green;

        float resultHp = currentHp / maxHp;

        if (currentHealthSlider != null)
        {
            currentHealthSlider.value = resultHp;
        }

        BattleBehaviourComponent state = GetComponent<BattleBehaviourComponent>();

        if (state != null)
        {
            hpText.text = currentHp.ToString();
        }
    }

    public float SetCurrentHp(float myHp) => currentHp = myHp;
    public float GetCurrentHp() => currentHp;
    public float GetMaxHp() => maxHp;
    public Slider GetCurrentHealthSlider() => currentHealthSlider;

}
