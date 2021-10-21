using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IODefaultState : IOBaseState
{
    public override void EnterState(IOStateManager Manager)
    {

    }
    Coroutine coroutine;
    bool coroutineIsRunning;
    public override void OnTriggerEnter2DState(IOStateManager Manager, Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!coroutineIsRunning) coroutine = Manager.StartCoroutine(WaitForPlayerInput(Manager, collision));
        }
    }
    public override void OnTriggerExit2DState(IOStateManager Manager, Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (coroutineIsRunning)
            {
                Manager.StopCoroutine(coroutine);
                coroutineIsRunning = false;
            }
        }
    }
    public IEnumerator WaitForPlayerInput(IOStateManager Manager, Collider2D collision)
    {
        coroutineIsRunning = true;
        bool successive=false;
        Manager.textBox.pageToDisplay = 0;
        while (!Input.GetKeyDown(KeyCode.E)) yield return null;
        PMStateManager player = collision.GetComponent<PMStateManager>();
        player.SmoothSwitchState(player.controlOffState);
        if (Manager.needItemToInteract)
        {
            switch (Manager.needToInteract)
            {
                case IOStateManager.NeedToInteract.needAllItemsToInteract:

                    Item[] itemsNeeded = new Item[Manager.itemsNeeded.Length];
                    int numberOfItemsRequiredInInventory = 0;
                    foreach (Item item in Manager.itemsNeeded)
                    {
                        foreach (Item itemInInventory in player.playerInventoryManager.inventory.GetItemList())
                        {
                            if (item.itemType == itemInInventory.itemType
                                && item.isAged == itemInInventory.isAged
                                && item.amount == itemInInventory.amount
                                && item.active == itemInInventory.active)
                            {
                                itemsNeeded[numberOfItemsRequiredInInventory] = itemInInventory;
                                numberOfItemsRequiredInInventory++;
                            }
                        }
                    }
                    if (numberOfItemsRequiredInInventory != Manager.itemsNeeded.Length)
                    {
                        Manager.textBox.text = Manager.textBoxOnFailedInteraction;
                    }
                    else
                    {
                        if (Manager.destroyItemOnSuccessiveInteraction)
                        {
                            foreach (Item item in itemsNeeded)
                            {
                                player.playerInventoryManager.inventory.GetItemList().Remove(item);
                            }
                            player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                        }
                        successive = true;
                        Manager.textBox.text = Manager.textBoxOnSuccessiveInteraction;
                    }
                    break;
                case IOStateManager.NeedToInteract.needOneOfTheItemsToInteract:
                    bool foundItem = false;
                    foreach (Item item in Manager.itemsNeeded)
                    {
                        foreach (Item itemInInventory in player.playerInventoryManager.inventory.GetItemList())
                        {
                            if (item.itemType == itemInInventory.itemType
                                && item.isAged == itemInInventory.isAged
                                && item.amount == itemInInventory.amount
                                && item.active == itemInInventory.active)
                            {
                                if (Manager.destroyItemOnSuccessiveInteraction)
                                {
                                    player.playerInventoryManager.inventory.GetItemList().Remove(itemInInventory);
                                    player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                                }
                                Manager.textBox.text = Manager.textBoxOnSuccessiveInteraction;
                                successive = true;
                                foundItem = true;
                                break;
                            }
                        }
                        if (foundItem) break;
                        else
                        {
                            Manager.textBox.text = Manager.textBoxOnFailedInteraction;
                        }
                    }
                    break;
            }
        }
        else
        {
            successive = true;
            Manager.textBox.text = Manager.textBoxOnSuccessiveInteraction;
        }
        Manager.ui.SetActive(false);
        TextBoxDefineEnabled(Manager, true);
        Time.timeScale = 0;
        while (Manager.textBox.textInfo.pageCount == 0)
        {
            yield return null;
        }
        while (Manager.textBox.textInfo.pageCount - Manager.textBox.pageToDisplay >= 0)
        {
            while (!Input.GetKeyDown(KeyCode.E)) yield return null;
            Manager.textBox.pageToDisplay++;
            yield return null;
        }
        if(successive) ResultsDataBase.Interaction(Manager.onSuccessiveInteractionConsequence);
        Manager.ui.SetActive(true);
        TextBoxDefineEnabled(Manager, false);
        player.SmoothSwitchState(player.defaultState);
        Time.timeScale = 1;
        coroutineIsRunning = false;
    }
    private void TextBoxDefineEnabled(IOStateManager Manager, bool enabledState)
    {
        Manager.textBox.transform.parent.gameObject.SetActive(enabledState);
    }
}
