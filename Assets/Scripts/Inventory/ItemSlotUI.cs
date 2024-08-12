using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectionHighlight;
    [SerializeField] private TextMeshProUGUI countText;

    private Item item;
    private InventorySystem inventorySystem;

    private void Start()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        icon.sprite = item.ItemIcon;
        countText.text = item.ItemCount.ToString();
        icon.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        countText.text = "";
        icon.gameObject.SetActive(false);
        selectionHighlight.enabled = false; // 슬롯이 비어 있을 때 선택 표시도 비활성화
    }

    public void SelectSlot()
    {
        if (item != null)
        {
            inventorySystem.SelectItem(item, this); // 선택된 아이템과 슬롯 UI 전달
            selectionHighlight.enabled = true; // 선택 표시 활성화
        }
    }

    public void DeselectSlot()
    {
        selectionHighlight.enabled = false; // 선택 표시 비활성화
    }

    public Item GetItem()
    {
        return item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot(); // 슬롯 클릭 시 선택 처리
    }
}
