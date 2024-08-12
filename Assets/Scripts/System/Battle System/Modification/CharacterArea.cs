using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArea : MonoBehaviour
{
    [Header("ü�� �� �����̴�")]
    [SerializeField] private Slider hpSlider;
    [Header("ȿ�� �����̴�")]
    [SerializeField] private Slider effectSlider;
    [Header("ü�� �۾�")]
    [SerializeField] private TextMeshProUGUI hpText;
    [Header("�� ��Ÿ�� �̹���")]
    [SerializeField] private Image coolTimeImage;

    public Slider GetEffectSlider() => effectSlider;
    public Slider GetHpSlider() => hpSlider;
    public TextMeshProUGUI GetHpText() => hpText;
    public Image GetCoolTimeImage() => coolTimeImage;
}
