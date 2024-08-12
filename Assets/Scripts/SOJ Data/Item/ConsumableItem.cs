using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ConsumableType / Consumable Item")]
public class ConsumableItem : Item
{
    [Header("아이템 소모성 타입")]
    [SerializeField] protected E_ConsumableType consumableType;

    public E_ConsumableType ConsumableType { get => consumableType; set => consumableType = value; }
}
