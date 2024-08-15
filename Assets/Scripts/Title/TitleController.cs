using System.Collections;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] private BridgeController bridgeController;

    private bool isCoroutineRunning = false;

    private void OnEnable()
    {
        isCoroutineRunning = false;
    }

    private void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        if (IsAnyKey() && !isCoroutineRunning)
        {
            StartCoroutine(nameof(TimerCoroutine));
        }
    }

    private IEnumerator TimerCoroutine()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1.5f);
        bridgeController.NextPhase();
        yield return new WaitForSeconds(1.5f);
        isCoroutineRunning = false;
    }

    private bool IsAnyKey()
    {
        return Input.anyKeyDown;
    }
}
