using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("아이템 스크립터블 데이터")]
    [SerializeField] private ItemList[] itemList;
    public ItemList[] ItemList { get => itemList; set => itemList = value; }

    // 현재 인벤토리에 있는 아이템 목록을 저장하는 리스트
    private List<Item> hasItemList;
    public List<Item> HasItemList { get => hasItemList; set => hasItemList = value; }
   
    // 실제 아이템들을 나타낼 프리펩 슬롯 리스트
    private List<ItemSlotUI> inventorySlots;

    [Space(5f)]
    [Header("인벤토리 최대 갯수")]
    [SerializeField] private int inventorySize;
    [Header("인벤토리 셀")]
    [SerializeField] private GameObject invenUIPrefab;
    [Header("인벤토리 셀 생성 위치")]
    [SerializeField] private Transform contentView;

    private Item selectedItem; // 선택된 아이템
    private ItemSlotUI selectedSlotUI; // 선택된 아이템의 슬롯 UI
    [SerializeField] private InventoryUI inventoryUI; // InventoryUI 참조 추가

    private void Awake()
    {
        InitValueInventory();
    }

    // 슬롯 초기화 하는 메서드
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

    // 주어진 아이템이 인벤토리에 있는 위치를 찾는 메서드
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

    // 아이템 ID로 ItemList에서 아이템을 찾는 메서드
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

    // 인벤토리에 새 아이템을 추가하는 메서드
    public void AddItem(Item newItem)
    {
        int index = FindItemIndex(newItem);

        Item itemData = GetItemData(newItem.ItemID);

        if (itemData != null)
        {
            // 인벤토리에 있던 아이템이면
            if (index >= 0)
            {
                // 인벤토리에 이미 있는 아이템이면 갯수를 추가
                hasItemList[index].ItemCount += newItem.ItemCount;
                inventorySlots[index].SetItem(hasItemList[index]);
            }
            else // 기존 인벤토리에 없던 아이템이면
            {
                // 인벤토리에 없는 아이템이면 새로 추가
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
            Debug.Log("제거하려는 아이템이 없습니다.");
            return;
        }

        int index = FindItemIndex(newItem);

        if (index >= 0) // 인벤토리에 아이템이 존재할 경우
        {
            // 아이템의 개수 감소
            hasItemList[index].ItemCount -= count;

            // 아이템의 개수가 0이 되면 인벤토리에서 아이템 제거
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
            Debug.Log("인벤토리에 없는 아이템을 제거하려고 시도 했습니다.");
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
            selectedSlotUI.DeselectSlot(); // 이전에 선택되었던 아이템의 선택 표시 해제
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
