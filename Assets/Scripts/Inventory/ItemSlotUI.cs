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
        selectionHighlight.enabled = false; // ������ ��� ���� �� ���� ǥ�õ� ��Ȱ��ȭ
    }

    public void SelectSlot()
    {
        if (item != null)
        {
            inventorySystem.SelectItem(item, this); // ���õ� �����۰� ���� UI ����
            selectionHighlight.enabled = true; // ���� ǥ�� Ȱ��ȭ
        }
    }

    public void DeselectSlot()
    {
        selectionHighlight.enabled = false; // ���� ǥ�� ��Ȱ��ȭ
    }

    public Item GetItem()
    {
        return item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot(); // ���� Ŭ�� �� ���� ó��
    }
}
