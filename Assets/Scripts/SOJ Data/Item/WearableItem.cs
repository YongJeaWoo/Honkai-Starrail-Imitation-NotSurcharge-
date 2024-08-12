using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "WearableType / Wearable Item")]
public class WearableItem : Item
{
    [Header("������ �Ҹ� Ÿ��")]
    [SerializeField] protected E_WearableType wearableType;

    public E_WearableType WearableType { get => wearableType; set => wearableType = value; }
}
