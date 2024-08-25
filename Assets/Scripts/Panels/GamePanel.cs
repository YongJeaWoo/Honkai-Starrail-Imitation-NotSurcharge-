using UnityEngine;
using UnityEngine.SceneManagement;

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

            PanelActive(isOn);
        }
    }

    public void OptionPanel()
    {
        PanelActive(isOn);
    }

    private void PanelActive(bool _isOn)
    {
        isOn = !_isOn;
        animator.SetBool(isOpen, isOn);
        panelActivation.SetPanelActive(isOn);
    }

    public void GoBackTitle()
    {
        Time.timeScale = 1;
        BattleEntryManager.Instance.EnemyZone(false);
        SceneManager.LoadSceneAsync("Title");
    }
}
