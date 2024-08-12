using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ConsumableType / Consumable Item")]
public class ConsumableItem : Item
{
    [Header("������ �Ҹ� Ÿ��")]
    [SerializeField] protected E_ConsumableType consumableType;

    public E_ConsumableType ConsumableType { get => consumableType; set => consumableType = value; }
}
