using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ItemInfoPanel itemInfoPanel;
    
    public void OnItemSlotClicked(Item item)
    {
        if (item != null)
        {
            itemInfoPanel.UpdateItemInfo(item);
        }
    }
}
