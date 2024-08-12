using UnityEngine;

public class GamePanel : InfoPanel
{
    private void Update()
    {
        TogglePanel();
    }

    private void TogglePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOn && panelActivation.IsAnyPanelActive()) return;

            isOn = !isOn;
            animator.SetBool(isOpen, isOn);
            panelActivation.SetPanelActive(isOn);
        }
    }
}
