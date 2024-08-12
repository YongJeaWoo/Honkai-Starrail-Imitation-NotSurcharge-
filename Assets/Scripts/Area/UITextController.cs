using TMPro;
using UnityEngine;

public class UITextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI areaText;

    public string SetAreaText(string text) => areaText.text = text;
}
