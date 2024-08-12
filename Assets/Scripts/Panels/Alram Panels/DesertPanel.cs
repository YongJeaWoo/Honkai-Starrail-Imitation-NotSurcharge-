using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesertPanel : DataPanel
{
    [Header("갯수 선택 슬라이더")]
    [SerializeField] private Slider countSlider;
    [Header("최소 갯수 버튼")]
    [SerializeField] private Button minButton;
    [Header("최대 갯수 버튼")]
    [SerializeField] private Button maxButton;
    [Header("확인 버튼")]
    [SerializeField] private Button confirmButton;
    [Header("아이템 갯수 텍스트")]
    [SerializeField] private TextMeshProUGUI itemCountText;

    private Item selectedItem;
    private InventorySystem inventorySystem;

    private void Update()
    {
        UpdateItemCount(countSlider.value);
    }

    public void Initialize(Item item, InventorySystem _inventorySystem)
    {
        selectedItem = item;
        inventorySystem = _inventorySystem;

        countSlider.minValue = 1;
        countSlider.maxValue = selectedItem.ItemCount;
        countSlider.value = countSlider.minValue;

        countSlider.wholeNumbers = true;
        UpdateItemCount(countSlider.value);
    }

    private void UpdateItemCount(float value)
    {
        itemCountText.text = $"{value.ToString("F0")} 개";
    }

    #region Button Setting
    public void SetMinButtonClick()
    {
        countSlider.value = countSlider.minValue;
        UpdateItemCount(countSlider.value);
    }

    public void SetMaxButtonClick()
    {
        countSlider.value = countSlider.maxValue;
        UpdateItemCount(countSlider.value);
    }

    public void MinusButtonClick()
    {
        if (countSlider.value > countSlider.minValue)
        {
            countSlider.value -= 1;
        }
    }

    public void PlusButtonClick()
    {
        if (countSlider.value < countSlider.maxValue)
        {
            countSlider.value += 1;
        }
    }

    public void ConfirmButtonClick()
    {
        int count = (int)countSlider.value;
        inventorySystem.RemoveItem(selectedItem, count);
        StartCoroutine(SelfDestroyPanel(this));
    }

    public void CancelButtonClick()
    {
        StartCoroutine(SelfDestroyPanel(this));
    }
    #endregion
}
