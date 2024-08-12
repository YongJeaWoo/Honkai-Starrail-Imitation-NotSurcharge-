using System.Collections;
using UnityEngine;

public class DataPanel : MonoBehaviour
{
    protected Animator animator;    

    protected string openText = $"isOpen";

    protected virtual void Awake()
    {
        InitializeValues();
    }

    protected virtual void OnEnable()
    {
        OpenPanel(true);
    }

    private void InitializeValues()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OpenPanel(bool isOn)
    {
        animator.SetBool(openText, isOn);
    }

    protected IEnumerator SelfDestroyPanel(DataPanel panel)
    {
        OpenPanel(false);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animator.GetCurrentAnimatorStateInfo(0).IsName("Close"));

        PopupManager.Instance.RemovePopUp(panel.name);
    }
}
