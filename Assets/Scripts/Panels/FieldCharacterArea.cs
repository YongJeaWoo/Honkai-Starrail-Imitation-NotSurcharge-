using UnityEngine;
using UnityEngine.UI;

public class FieldCharacterArea : MonoBehaviour
{
    [Header("ĳ���� �̸�")]
    [SerializeField] private Text characterName;
    [Header("ĳ���� �ʻ�ȭ Image")]
    [SerializeField] private Image characterImage;
    [Header("ĳ���� ü�� Slider")]
    [SerializeField] private Slider characterHpSlider;
    [Header("ĳ���� �ñر� Image")]
    [SerializeField] private Image skillImage;

    private FieldState field;

    private CharacterStateSOJ soj;
    public CharacterStateSOJ Soj { get => soj; set => soj = value; }

    public void SetCharacterUI(GameObject player)
    {
        field = player.GetComponentInChildren<FieldState>();
        var health = player.GetComponent<PlayerHealth>();

        health.InitializeSOJData();
        health.SetFieldSlider(characterHpSlider);

        var currentHp = health.GetCurrentHp();
        var maxHp = health.GetMaxHp();
        float resultHp = currentHp / maxHp;

        characterHpSlider.value = resultHp;

        characterName.text = soj.PlayerPrefab.name;
        characterImage.sprite = field.StateSOJ.GetCharacterIcon();
        skillImage.sprite = field.StateSOJ.GetSkillIcon();
    }
}
