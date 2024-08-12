using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("맵 전용 카메라")]
    [SerializeField] private Camera mapCamera;

    private GameObject mapPanel;

    private bool isOn;

    private void Start()
    {
        InitValues();
    }

    private void InitValues()
    {
        isOn = false;

        //var firstChild = UIManager.Instance.transform.GetChild(0);
        //var finalMapPanel = firstChild.GetChild(7);

        //mapPanel = finalMapPanel.gameObject;
    }

    private void Update()
    {
        MapControl();
    }

    private void MapControl()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isOn = !isOn;
            var popUp = PopupManager.Instance.GetComponentInChildren<PanelActivate>();
            popUp.SetPanelActive(isOn);
            mapPanel.SetActive(isOn);
        }
    }
}
