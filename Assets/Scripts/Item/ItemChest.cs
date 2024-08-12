using UnityEngine;

public class ItemChest : ItemDrop, IDetect, IBox
{
    [SerializeField] private GameObject openParticlePrefab;
    [SerializeField] private GameObject destroyParticlePrefab;
    private Animator anim;

    InventorySystem inventorySystem;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        inventorySystem = FindObjectOfType<InventorySystem>();
    }

    public void DetectiveBehaviour()
    {
        if (isGetDrop) return;

        BoxOpen();
    }

    public void BoxOpen()
    {
        DropRandomItem(); // �������� ������ ����

        Drop();
        anim.SetTrigger("Open");
    }

    private void DropRandomItem()
    {
        if (ItemInfoArray.Length > 0)
        {
            // ItemInfoArray�� ��� �������� ����
            foreach (ItemInfo itemInfo in ItemInfoArray)
            {
                // ItemInfo�� ID�� ��ġ�ϴ� �������� InventorySystem�� ItemList���� ã��
                Item matchingItem = inventorySystem.GetItemData(itemInfo.itemID);

                if (matchingItem != null)
                {
                    // ������ ���� ���� (���� �� ������ ����.������ �����)
                    matchingItem.ItemCount = 1;

                    // ������ �߰�
                    inventorySystem.AddItem(matchingItem);

                    // �������� UI�� ǥ��
                    infoSystem.AddItemToQueue(itemInfo);
                }
                else
                {
                    Debug.LogWarning($"�������� ã�� �� ����: ID {itemInfo.itemID}");
                }
            }
        }
    }

    public void OpenEffectEvent()
    {
        InteractiveParticle(openParticlePrefab);
    }

    public void BoxDestroy()
    {
        InteractiveParticle(destroyParticlePrefab);
        Destroy(gameObject);
    }

    public void InteractiveParticle(GameObject particle)
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
