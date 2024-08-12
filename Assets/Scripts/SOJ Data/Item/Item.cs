using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item / ItemObject")]
public class Item : ScriptableObject
{
    [Header("������ ������")]
    [SerializeField] protected Sprite itemIcon;
    [Header("������ �̸�")]
    [SerializeField] protected string itemName;
    [Header("������ �ΰ�����")]
    [SerializeField] protected string itemDescription;
    [Header("������ ���� ���̵�")]
    [SerializeField] protected int itemID;
    [Header("������ ���� ������")]
    [SerializeField] protected GameObject itemPrefab;
    [Header("������ ����")]
    [SerializeField] protected int itemCount;
    [Header("������ Ÿ��")]
    [SerializeField] protected E_ItemType itemType;

    public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public string ItemDescription { get => itemDescription; set => itemDescription = value; }
    public int ItemID { get => itemID; set => itemID = value; }
    public GameObject ItemPrefab { get => itemPrefab; set => itemPrefab = value; }
    public int ItemCount { get => itemCount; set => itemCount = value; }
    public E_ItemType ItemType { get => itemType; set => itemType = value; }
}
