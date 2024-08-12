using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{
    private readonly string UseText = $"Use Info Panel";

    [SerializeField] private GameObject setActiveUI; // ������ ���� �г� Ȱ��ȭ
    [SerializeField] private GameObject buttonLayout;
    [SerializeField] private TextMeshProUGUI explainText; // ������ ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image icon; // ������ ������ �̹���
    [SerializeField] private InventoryController controller;

    [SerializeField] private InventorySystem inventorySystem;

    private TextMeshProUGUI useText;
    // ���� ���� �ʿ� �� ����ϸ� ��
    //private TextMeshProUGUI desertText;

    private Item selectedItem;

    private string desertText = $"Desert Panel";

    private Dictionary<E_ItemType, string> itemTypeToText = new Dictionary<E_ItemType, string>
    {
        { E_ItemType.Consumable, "����ϱ�" },
        { E_ItemType.Wearable, "�����ϱ�" },
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
            panel.SetAlramText($"{selectedItem.ItemName} \n��������(��) ����߽��ϴ�.");
            inventorySystem.RemoveItem(selectedItem, 1);
        }
        else
        {
            var popUp = PopupManager.Instance.InstantPopUp(UseText);
            var panel = popUp.GetComponentInChildren<AlramPanel>();
            panel.SetAlramText($"�������� ����� �� �ִ� ȯ���� �ƴմϴ�.");
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
