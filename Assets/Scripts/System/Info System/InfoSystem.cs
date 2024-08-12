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

    [Header("�� ��ġ")]
    [SerializeField] private Transform cellInfoPos;
    [Header("���� �� ������")]
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

            // ������ ID�� ����Ͽ� InventorySystem���� ������ ���� ��������
            Item item = inventorySystem.GetItemData(itemInfo.itemID);

            if (item != null)
            {
                if (cellImage != null)
                {
                    // �������� �̹��� ����
                    cellImage.sprite = item.ItemIcon;
                }

                if (cellText != null)
                {
                    // �������� �ؽ�Ʈ ����
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
