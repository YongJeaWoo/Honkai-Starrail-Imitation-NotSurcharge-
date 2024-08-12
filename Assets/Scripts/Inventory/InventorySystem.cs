using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("������ ��ũ���ͺ� ������")]
    [SerializeField] private ItemList[] itemList;
    public ItemList[] ItemList { get => itemList; set => itemList = value; }

    // ���� �κ��丮�� �ִ� ������ ����� �����ϴ� ����Ʈ
    private List<Item> hasItemList;
    public List<Item> HasItemList { get => hasItemList; set => hasItemList = value; }
   
    // ���� �����۵��� ��Ÿ�� ������ ���� ����Ʈ
    private List<ItemSlotUI> inventorySlots;

    [Space(5f)]
    [Header("�κ��丮 �ִ� ����")]
    [SerializeField] private int inventorySize;
    [Header("�κ��丮 ��")]
    [SerializeField] private GameObject invenUIPrefab;
    [Header("�κ��丮 �� ���� ��ġ")]
    [SerializeField] private Transform contentView;

    private Item selectedItem; // ���õ� ������
    private ItemSlotUI selectedSlotUI; // ���õ� �������� ���� UI
    [SerializeField] private InventoryUI inventoryUI; // InventoryUI ���� �߰�

    private void Awake()
    {
        InitValueInventory();
    }

    // ���� �ʱ�ȭ �ϴ� �޼���
    private void InitValueInventory()
    {
        hasItemList = new List<Item>();
        inventorySlots = new List<ItemSlotUI>();

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(invenUIPrefab, contentView.position, Quaternion.identity, contentView);
            ItemSlotUI slot = slotObject.GetComponent<ItemSlotUI>();
            inventorySlots.Add(slot);
            slot.ClearSlot();
        }
    }

    // �־��� �������� �κ��丮�� �ִ� ��ġ�� ã�� �޼���
    private int FindItemIndex(Item newItem)
    {
        int result = -1;
        for (int i = hasItemList.Count - 1; i >= 0; i--)
        {
            if (hasItemList[i].ItemID == newItem.ItemID)
            {
                result = i;
                break;
            }
        }
        return result;
    }

    // ������ ID�� ItemList���� �������� ã�� �޼���
    public Item GetItemData(int itemID)
    {
        foreach (var itemListInstance in ItemList)
        {
            foreach (var item in itemListInstance.ItemsSOJList)
            {
                if (item.ItemID == itemID)
                {
                    return item;
                }
            }
        }
        return null;
    }

    // �κ��丮�� �� �������� �߰��ϴ� �޼���
    public void AddItem(Item newItem)
    {
        int index = FindItemIndex(newItem);

        Item itemData = GetItemData(newItem.ItemID);

        if (itemData != null)
        {
            // �κ��丮�� �ִ� �������̸�
            if (index >= 0)
            {
                // �κ��丮�� �̹� �ִ� �������̸� ������ �߰�
                hasItemList[index].ItemCount += newItem.ItemCount;
                inventorySlots[index].SetItem(hasItemList[index]);
            }
            else // ���� �κ��丮�� ���� �������̸�
            {
                // �κ��丮�� ���� �������̸� ���� �߰�
                Item itemToAdd = new Item
                {
                    ItemID = itemData.ItemID,
                    ItemName = itemData.ItemName,
                    ItemIcon = itemData.ItemIcon,
                    ItemDescription = itemData.ItemDescription,
                    ItemPrefab = itemData.ItemPrefab,
                    ItemCount = newItem.ItemCount,
                    ItemType = itemData.ItemType
                };

                hasItemList.Add(itemToAdd);
                int newIndex = hasItemList.Count - 1;
                inventorySlots[newIndex].SetItem(itemToAdd);
            }
        }
    }

    public void RemoveItem(Item newItem, int count)
    {
        if (newItem == null)
        {
            Debug.Log("�����Ϸ��� �������� �����ϴ�.");
            return;
        }

        int index = FindItemIndex(newItem);

        if (index >= 0) // �κ��丮�� �������� ������ ���
        {
            // �������� ���� ����
            hasItemList[index].ItemCount -= count;

            // �������� ������ 0�� �Ǹ� �κ��丮���� ������ ����
            if (hasItemList[index].ItemCount <= 0)
            {
                hasItemList.RemoveAt(index);
                OrganizeSlots();
            }
            else
            {
                inventorySlots[index].SetItem(hasItemList[index]);
            }
        }
        else
        {
            Debug.Log("�κ��丮�� ���� �������� �����Ϸ��� �õ� �߽��ϴ�.");
        }
    }

    public void SelectItem(Item item, ItemSlotUI slotUI)
    {
        DeselectItem();

        selectedItem = item;
        selectedSlotUI = slotUI;
        inventoryUI.OnItemSlotClicked(item);

        if (selectedSlotUI != null)
        {
            selectedSlotUI.DeselectSlot(); // ������ ���õǾ��� �������� ���� ǥ�� ����
        }
    }

    public void DeselectItem()
    {
        if (selectedSlotUI != null)
        {
            selectedSlotUI.DeselectSlot();
            selectedItem = null;
            selectedSlotUI = null;
        }
    }

    public Item GetSelectedItem()
    {
        return selectedItem;
    }

    private void OrganizeSlots()
    {
        foreach (var slot in inventorySlots)
        {
            slot.ClearSlot();
        }

        for (int i = 0; i < hasItemList.Count; i++)
        {
            inventorySlots[i].SetItem(hasItemList[i]);
        }
    }
}
