using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private bool isOn;

    private InventorySystem inventorySystem;
    private ItemInfoPanel itemInfoPanel;
    private PanelActivate panelActivate;
    
    [SerializeField] private GameObject inventoryCanvas;

    private void Start()
    {
        InitInventory();
    }

    private void InitInventory()
    {
        inventorySystem = GetComponentInChildren<InventorySystem>();

        panelActivate = PopupManager.Instance.GetPanelActivation();

        var firstChild = transform.GetChild(0);
        var secondChild = firstChild.GetChild(0);
        var thirdChild = secondChild.GetChild(0);
        var parentInfoPanel = thirdChild.GetChild(3);
        itemInfoPanel = parentInfoPanel.GetComponent<ItemInfoPanel>();
    }

    private void Update()
    {
        ToggleInputKey();
    }

    private void ToggleInputKey()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isOn && panelActivate.IsAnyPanelActive()) return;

            isOn = !isOn;
            inventoryCanvas.SetActive(isOn);
            panelActivate.SetPanelActive(isOn);

            if (isOn)
            {
                itemInfoPanel.GetActiveUI().SetActive(false);
            }
            else
            {
                inventorySystem.DeselectItem();
            }
        }
    }

    public void ExitButtonClick()
    {
        isOn = false;
        inventoryCanvas.SetActive(false);
        panelActivate.SetPanelActive(false);
        inventorySystem.DeselectItem();
    }
}
