using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "Item / ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] protected List<Item> itemsSOJList;
    public List<Item> ItemsSOJList { get => itemsSOJList; set => itemsSOJList = value; }
}
