using System.Collections;
using TMPro;
using UnityEngine;

public class AlramPanel : MonoBehaviour
{
    protected Animator animator;
    protected TextMeshProUGUI alramText;
    protected string openText = $"isOpen";

    protected virtual void Awake()
    {
        InitializeValues();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(SelfDestroyPanel(this));
    }

    private void InitializeValues()
    {
        alramText = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    protected IEnumerator SelfDestroyPanel(AlramPanel panel)
    {
        animator.SetBool(openText, true);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        yield return new WaitForSecondsRealtime(1.8f);

        animator.SetBool(openText, false);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animator.GetCurrentAnimatorStateInfo(0).IsName("Close"));

        PopupManager.Instance.RemovePopUp(panel.name);
    }

    public string SetAlramText(string text) => alramText.text = text;
}
