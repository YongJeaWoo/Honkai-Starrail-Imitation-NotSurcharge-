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
        DropRandomItem(); // 랜덤으로 아이템 생성

        Drop();
        anim.SetTrigger("Open");
    }

    private void DropRandomItem()
    {
        if (ItemInfoArray.Length > 0)
        {
            // ItemInfoArray의 모든 아이템을 선택
            foreach (ItemInfo itemInfo in ItemInfoArray)
            {
                // ItemInfo의 ID와 일치하는 아이템을 InventorySystem의 ItemList에서 찾기
                Item matchingItem = inventorySystem.GetItemData(itemInfo.itemID);

                if (matchingItem != null)
                {
                    // 아이템 갯수 설정 (랜덤 한 갯수는 랜덤.래인지 쓰면됨)
                    matchingItem.ItemCount = 1;

                    // 아이템 추가
                    inventorySystem.AddItem(matchingItem);

                    // 아이템을 UI에 표시
                    infoSystem.AddItemToQueue(itemInfo);
                }
                else
                {
                    Debug.LogWarning($"아이템을 찾을 수 없음: ID {itemInfo.itemID}");
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
