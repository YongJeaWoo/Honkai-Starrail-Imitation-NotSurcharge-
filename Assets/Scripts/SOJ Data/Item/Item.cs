using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item / ItemObject")]
public class Item : ScriptableObject
{
    [Header("아이템 아이콘")]
    [SerializeField] protected Sprite itemIcon;
    [Header("아이템 이름")]
    [SerializeField] protected string itemName;
    [Header("아이템 부가설명")]
    [SerializeField] protected string itemDescription;
    [Header("아이템 선별 아이디")]
    [SerializeField] protected int itemID;
    [Header("아이템 생성 프리팹")]
    [SerializeField] protected GameObject itemPrefab;
    [Header("아이템 갯수")]
    [SerializeField] protected int itemCount;
    [Header("아이템 타입")]
    [SerializeField] protected E_ItemType itemType;

    public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public string ItemDescription { get => itemDescription; set => itemDescription = value; }
    public int ItemID { get => itemID; set => itemID = value; }
    public GameObject ItemPrefab { get => itemPrefab; set => itemPrefab = value; }
    public int ItemCount { get => itemCount; set => itemCount = value; }
    public E_ItemType ItemType { get => itemType; set => itemType = value; }
}
