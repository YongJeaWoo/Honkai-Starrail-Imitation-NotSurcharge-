using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [Header("�ൿ�� Text")]
    [SerializeField] private TextMeshProUGUI activePointText;

    [Header("ĳ���� �̹���")]
    [SerializeField] private Image characterImage;

    [Header("�Ʊ�/���� ǥ�� Bar")]
    [SerializeField] private Image infoBar;

    public void SetInfoUI(BattleBehaviourComponent actionPoint)
    {
        activePointText.text = actionPoint.ActionPoint.ToString("F0");
    }
}
