using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Field Player State", menuName = "Field State / UsePlayerName")]
public class FieldPlayerState : ScriptableObject
{
    #region UI Skill Icon
    [Header("�⺻ ���� ������")]
    [SerializeField] private Sprite basicAttackIcon;
    [Header("��ų ���� ������")]
    [SerializeField] private Sprite skillIcon;
    [Header("ĳ���� �ʻ�ȭ")]
    [SerializeField] private Sprite characterSprite;
    [Header("��ų �ؽ�Ʈ")]
    [SerializeField] private string skillText;
    #endregion

    public Sprite GetBasicAttackIcon() => basicAttackIcon;
    public Sprite GetSkillIcon() => skillIcon;
    public Sprite GetCharacterIcon() => characterSprite;
    public string GetSkillText() => skillText;
}
