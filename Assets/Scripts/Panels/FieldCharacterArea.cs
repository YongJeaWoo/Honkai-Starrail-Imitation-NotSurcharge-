using UnityEngine;
using UnityEngine.UI;

public class FieldCharacterArea : MonoBehaviour
{
    [Header("캐릭터 이름")]
    [SerializeField] private Text characterName;
    [Header("캐릭터 초상화 Image")]
    [SerializeField] private Image characterImage;
    [Header("캐릭터 체력 Slider")]
    [SerializeField] private Slider characterHpSlider;
    [Header("캐릭터 궁극기 Image")]
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
