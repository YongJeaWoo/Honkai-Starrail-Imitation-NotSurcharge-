using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject gameCanvas;

    private void Start()
    {
        var system = PlayerDataManager.Instance.GetEventSystem();

        if (!system.GetFirstEvent())
        {
            PopupManager.Instance.InstantPopUp("First Game Panel", gameCanvas.transform);
            system.SetFirstEvent(true);
        }
    }
}
