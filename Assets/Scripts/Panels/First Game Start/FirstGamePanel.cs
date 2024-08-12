using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirstGamePanel : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        PopupManager.Instance.GetPanelActivation().DoNotFirstPanelCreation(true);
        StartCoroutine(nameof(GetKeyDownCoroutine));
    }

    private IEnumerator GetKeyDownCoroutine()
    {
        Time.timeScale = 0;

        var playerSystem = FindObjectOfType<SetPlayerSystem>();
        var gamePanel = FindObjectOfType<GamePanel>();
        var invenController = FindObjectOfType<InventoryController>();
        var fieldPanelController = FindObjectOfType<FieldPanelController>();
        var ccController = FindObjectOfType<CharacterChooseController>();
        var panel = ccController.GetOncePartyPanel();
        var button = panel.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        var confirmButton = panel.transform.GetChild(1).GetChild(1).GetComponent<Button>();

        playerSystem.enabled = false;
        gamePanel.enabled = false;
        invenController.enabled = false;
        fieldPanelController.enabled = false;

        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.L))
            {
                button.interactable = false;
                playerSystem.enabled = true;
                gamePanel.enabled = true;
                invenController.enabled = true;
                fieldPanelController.enabled = true;
                break;
            }
        }

        confirmButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            button.interactable = true;
            PopupManager.Instance.GetPanelActivation().DoNotFirstPanelCreation(false);
        });

        yield return null;
        PopupManager.Instance.RemovePopUp(gameObject.name);
    }
}
