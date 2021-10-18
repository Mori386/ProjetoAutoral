using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Flashlight,Placeholder
    }
    public ItemType itemType;
    public int amount;
    public bool active;
    public bool isDestroyedOnUse()
    {
        switch(itemType)
        {
            default:
            case ItemType.Flashlight:
                return false;
        }
    }
    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.Flashlight:
                return false;
            case ItemType.Placeholder:
                return true;
        }
    }
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Flashlight:return ItemAssets.Instance.flashlightSprite;
            case ItemType.Placeholder: return ItemAssets.Instance.placeholderSprite;
        }
    }
}
