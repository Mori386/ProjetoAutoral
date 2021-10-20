using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemTimeline
    {
        Present, Future
    }
    public enum ItemType
    {
        Flashlight, Placeholder
    }
    public ItemType itemType;
    public int amount;
    public bool active;
    public bool isAged;
    [System.NonSerialized] public ItemTimeline itemTimeline;

    [System.NonSerialized] public Item itemInFuture;
    [System.NonSerialized] public ItemWorld itemWorld;
    public bool isDestroyedOnUse()
    {
        switch (itemType)
        {
            default:
            case ItemType.Flashlight:
                return false;
        }
    }
    public bool IsStackable()
    {
        switch (itemType)
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
            case ItemType.Flashlight:
                if (isAged)
                {
                    return ItemAssets.Instance.flashlightFutureSprite;
                }
                else
                {
                    return ItemAssets.Instance.flashlightPresentSprite;
                }
            case ItemType.Placeholder: return ItemAssets.Instance.placeholderSprite;
        }
    }
}