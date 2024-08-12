using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{
    private readonly string UseText = $"Use Info Panel";

    [SerializeField] private GameObject setActiveUI; // 아이템 정보 패널 활성화
    [SerializeField] private GameObject buttonLayout;
    [SerializeField] private TextMeshProUGUI explainText; // 아이템 설명 텍스트
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image icon; // 아이템 아이콘 이미지
    [SerializeField] private InventoryController controller;

    [SerializeField] private InventorySystem inventorySystem;

    private TextMeshProUGUI useText;
    // 추후 변경 필요 시 사용하면 됨
    //private TextMeshProUGUI desertText;

    private Item selectedItem;

    private string desertText = $"Desert Panel";

    private Dictionary<E_ItemType, string> itemTypeToText = new Dictionary<E_ItemType, string>
    {
        { E_ItemType.Consumable, "사용하기" },
        { E_ItemType.Wearable, "장착하기" },
    };

    private void Awake()
    {
        useText = buttonLayout.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        //desertText = buttonLayout.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateItemInfo(Item item)
    {
        if (item != null)
        {
            explainText.text = item.ItemDescription;
            itemNameText.text = item.ItemName;
            icon.sprite = item.ItemIcon;
            setActiveUI.SetActive(true);
            ItemTypeToText(item);
        }
        else
        {
            explainText.text = null;
            itemNameText.text = null;
            icon.sprite = null;
            setActiveUI.SetActive(false);
        }

        selectedItem = item;
    }

    public void ItemUseButtonClick()
    {
        var go = Instantiate(selectedItem.ItemPrefab);
        var useItem = go.GetComponent<IConsumable>();
        useItem.CheckItem();
        
        if (useItem.isUse)
        {
            var popUp = PopupManager.Instance.InstantPopUp(UseText);
            var panel = popUp.GetComponentInChildren<AlramPanel>();
            panel.SetAlramText($"{selectedItem.ItemName} \n아이템을(를) 사용했습니다.");
            inventorySystem.RemoveItem(selectedItem, 1);
        }
        else
        {
            var popUp = PopupManager.Instance.InstantPopUp(UseText);
            var panel = popUp.GetComponentInChildren<AlramPanel>();
            panel.SetAlramText($"아이템을 사용할 수 있는 환경이 아닙니다.");
        }

        controller.ExitButtonClick();
    }

    public void DesertItemButtonClick()
    {
        var desertPopup = PopupManager.Instance.InstantPopUp(desertText);
        var panel = desertPopup.GetComponentInChildren<DesertPanel>();
        panel.Initialize(selectedItem, inventorySystem);
    }

    private void ItemTypeToText(Item _item)
    {
        if (itemTypeToText.TryGetValue(_item.ItemType, out string text))
        {
            useText.text = text;
        }
        else
        {
            useText.text = string.Empty;
        }
    }

    public GameObject GetActiveUI() => setActiveUI;
}
