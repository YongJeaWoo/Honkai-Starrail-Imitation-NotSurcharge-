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

            isOn = !isOn;
            animator.SetBool(isOpen, isOn);
            panelActivation.SetPanelActive(isOn);
        }
    }

    public void GoBackTitle()
    {
        Time.timeScale = 1;
        BattleEntryManager.Instance.EnemyZone(false);
        SceneManager.LoadSceneAsync("Title");
    }
}
