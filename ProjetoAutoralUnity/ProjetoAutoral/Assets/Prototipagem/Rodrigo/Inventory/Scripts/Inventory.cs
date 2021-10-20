using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    private PlayerInventoryManager playerInventoryManager;
    public Inventory(PlayerInventoryManager PIM)
    {
        itemList = new List<Item>();
        playerInventoryManager = PIM;
    }
    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        if (playerInventoryManager.activeItem == 0)
        {
            playerInventoryManager.activeItem = 1;
            playerInventoryManager.uiInventory.itemsUi[0].Find("Border").GetComponent<Outline>().enabled = true;
        }
    }
    public void RemoveItem(int itemNumber)
    {
        Item item = playerInventoryManager.inventory.GetItemList()[itemNumber];
        if (item.IsStackable())
        {
            item.amount--;
            if (item.amount <= 0)
            {
                itemList.Remove(item);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public void OnItemListChange()
    {
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public void UseItem(int itemNumber)
    {
        Item item = playerInventoryManager.inventory.GetItemList()[itemNumber];
        switch (item.itemType)
        {
            case Item.ItemType.Flashlight:
                if (item.isAged)
                {

                }
                else
                {
                    if (!item.active)
                    {
                        //liga
                        Debug.Log("Liga");
                        item.active = true;
                    }
                    else
                    {
                        //desliga
                        Debug.Log("Desliga");
                        item.active = false;
                    }
                }
                break;
        }
        if (item.isDestroyedOnUse())
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
