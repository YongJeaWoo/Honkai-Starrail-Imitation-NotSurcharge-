using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArea : MonoBehaviour
{
    [Header("체력 바 슬라이더")]
    [SerializeField] private Slider hpSlider;
    [Header("효과 슬라이더")]
    [SerializeField] private Slider effectSlider;
    [Header("체력 글씨")]
    [SerializeField] private TextMeshProUGUI hpText;
    [Header("궁 쿨타임 이미지")]
    [SerializeField] private Image coolTimeImage;

    public Slider GetEffectSlider() => effectSlider;
    public Slider GetHpSlider() => hpSlider;
    public TextMeshProUGUI GetHpText() => hpText;
    public Image GetCoolTimeImage() => coolTimeImage;
}
