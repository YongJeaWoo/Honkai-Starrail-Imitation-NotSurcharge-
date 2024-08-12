using TMPro;
using UnityEngine;

public class PartyButton : MonoBehaviour
{
    [SerializeField] private GameObject lineObj;

    private Color originColor;

    private TextMeshProUGUI text;
    private Animator animator;

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        animator = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        InitValues();
    }

    private void InitValues()
    {
        originColor = text.color;
    }

    public void SelectButton()
    {
        lineObj.SetActive(true);
        text.color = Color.yellow;
        animator.SetTrigger("isSelect");
    }

    public void DeselectButton()
    {
        lineObj.SetActive(false);
        text.color = originColor;
    }
}
