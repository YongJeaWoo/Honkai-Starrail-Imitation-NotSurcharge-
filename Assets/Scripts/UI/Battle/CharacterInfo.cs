using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [Header("행동력 Text")]
    [SerializeField] private TextMeshProUGUI activePointText;

    [Header("캐릭터 이미지")]
    [SerializeField] private Image characterImage;

    [Header("아군/적군 표시 Bar")]
    [SerializeField] private Image infoBar;

    public void SetInfoUI(BattleBehaviourComponent actionPoint)
    {
        activePointText.text = actionPoint.ActionPoint.ToString("F0");
    }
}
