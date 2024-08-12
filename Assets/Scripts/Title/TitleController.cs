using System.Collections;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] private BridgeController bridgeController;

    private void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        if (IsAnyKey())
        {
            StartCoroutine(nameof(TimerCoroutine));
        }
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        bridgeController.NextPhase();
    }

    private bool IsAnyKey()
    {
        return Input.anyKeyDown;
    }
}
