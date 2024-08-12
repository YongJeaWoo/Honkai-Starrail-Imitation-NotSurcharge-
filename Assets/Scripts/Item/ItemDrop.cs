using System;
using UnityEngine;

[Serializable]
public struct ItemInfo
{
    public int itemID;
}

public abstract class ItemDrop : MonoBehaviour
{
    [SerializeField] protected ItemInfo[] itemInfoArray;
    public ItemInfo[] ItemInfoArray { get => itemInfoArray; set => itemInfoArray = value; }

    protected int itemCount;
    protected bool isGetDrop;
    protected InfoSystem infoSystem;

    protected virtual void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        infoSystem = FindObjectOfType<InfoSystem>();
        isGetDrop = false;
        itemCount = ItemInfoArray.Length;
    }

    public virtual void Drop()
    {
        isGetDrop = true;
    }

    public int GetItemCount() => itemCount;
}
