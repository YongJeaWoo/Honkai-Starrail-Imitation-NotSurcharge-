using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Field Player State", menuName = "Field State / UsePlayerName")]
public class FieldPlayerState : ScriptableObject
{
    #region UI Skill Icon
    [Header("기본 공격 아이콘")]
    [SerializeField] private Sprite basicAttackIcon;
    [Header("스킬 공격 아이콘")]
    [SerializeField] private Sprite skillIcon;
    [Header("캐릭터 초상화")]
    [SerializeField] private Sprite characterSprite;
    [Header("스킬 텍스트")]
    [SerializeField] private string skillText;
    #endregion

    public Sprite GetBasicAttackIcon() => basicAttackIcon;
    public Sprite GetSkillIcon() => skillIcon;
    public Sprite GetCharacterIcon() => characterSprite;
    public string GetSkillText() => skillText;
}
