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
        Flashlight, Placeholder, 
        KeyCabine1, KeyCabine2, Cloth, Plunger, KeyExit1 //itens do puzzle1
        
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
            //puzzle1
            #region
            case ItemType.KeyCabine1:
            case ItemType.KeyCabine2:
            case ItemType.Cloth:
            case ItemType.Plunger:
            case ItemType.KeyExit1:
                #endregion
                //puzzle2
                #region
                #endregion
                //puzzle3
                #region
                #endregion
                return true;
        }
    }
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Flashlight:
            case ItemType.Cloth:
            case ItemType.KeyCabine1:
            case ItemType.KeyCabine2:
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