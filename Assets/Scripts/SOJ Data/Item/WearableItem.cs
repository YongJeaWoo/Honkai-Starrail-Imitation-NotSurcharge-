using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "WearableType / Wearable Item")]
public class WearableItem : Item
{
    [Header("아이템 소모성 타입")]
    [SerializeField] protected E_WearableType wearableType;

    public E_WearableType WearableType { get => wearableType; set => wearableType = value; }
}
