using UnityEngine;

public class PanelActivate : MonoBehaviour
{
    private bool isAnyPanelActive = false;

    private GameInputSystem inputSystem;

    private bool isFirst;

    private void Awake()
    {
        GameObject system = new GameObject($"Alt Input System");
        system.transform.parent = transform;
        inputSystem = system.AddComponent<GameInputSystem>();
    }

    public bool IsAnyPanelActive() => isAnyPanelActive;

    public void SetPanelActive(bool active)
    {
        isAnyPanelActive = active;

        if (isAnyPanelActive)
        {
            Time.timeScale = 0;
            PopupManager.Instance.ShowMouse();
            inputSystem.SetFreeCamera(0, 0);
        }
        else
        {
            Time.timeScale = 1f;
            PopupManager.Instance.HideMouse();
            var xValue = inputSystem.GetXValue();
            var yValue = inputSystem.GetYValue();
            inputSystem.SetFreeCamera(xValue, yValue);
        }
    }

    public void DoNotFirstPanelCreation(bool isOn)
    {
        isFirst = isOn;
    }

    public bool GetIsFirst() => isFirst;
}
