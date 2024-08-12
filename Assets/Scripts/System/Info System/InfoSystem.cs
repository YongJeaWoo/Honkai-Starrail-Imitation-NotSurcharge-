using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoSystem : MonoBehaviour
{
    private Queue<ItemInfo> itemQueue;
    private List<GameObject> activeCells;

    private InventorySystem inventorySystem;

    [Header("셀 위치")]
    [SerializeField] private Transform cellInfoPos;
    [Header("정보 셀 프리팹")]
    [SerializeField] private GameObject infoCellPrefab;

    private int maxSize;

    private void Start()
    {
        InitValue();
        inventorySystem = FindObjectOfType<InventorySystem>();
    }

    private void InitValue()
    {
        maxSize = 8;
        itemQueue = new Queue<ItemInfo>();
        activeCells = new List<GameObject>();
    }

    public void AddItemToQueue(ItemInfo itemInfo)
    {
        itemQueue.Enqueue(itemInfo);
        DisplayNextBatch();
    }

    private void DisplayNextBatch()
    {
        int availableSlots = maxSize - activeCells.Count;
        int itemToDisplay = Mathf.Min(itemQueue.Count, availableSlots);

        for (int i = 0; i < itemToDisplay; i++)
        {
            ItemInfo itemInfo = itemQueue.Dequeue();
            GameObject cell = Instantiate(infoCellPrefab, cellInfoPos);

            var cellImage = cell.transform.Find("Item Icon").GetComponentInChildren<Image>();
            var cellText = cell.GetComponentInChildren<TextMeshProUGUI>();

            // 아이템 ID를 사용하여 InventorySystem에서 아이템 정보 가져오기
            Item item = inventorySystem.GetItemData(itemInfo.itemID);

            if (item != null)
            {
                if (cellImage != null)
                {
                    // 아이템의 이미지 설정
                    cellImage.sprite = item.ItemIcon;
                }

                if (cellText != null)
                {
                    // 아이템의 텍스트 설정
                    cellText.text = item.ItemName;
                }
            }

            activeCells.Add(cell);
            StartCoroutine(AutoDestroyCell(cell, 2f));
        }

        if (cellInfoPos.childCount == 0 && itemQueue.Count > 0)
        {
            DisplayNextBatch();
        }
    }

    private IEnumerator AutoDestroyCell(GameObject cell, float delay)
    {
        yield return new WaitForSeconds(delay);

        activeCells.Remove(cell);
        Destroy(cell);

        if (itemQueue.Count > 0)
        {
            yield return new WaitForSeconds(.5f);
            DisplayNextBatch();
        }
    }
}
