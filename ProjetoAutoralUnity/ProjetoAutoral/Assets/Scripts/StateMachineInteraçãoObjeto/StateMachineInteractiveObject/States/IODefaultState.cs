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
            if (!coroutineIsRunning)
            {
                collision.transform.Find("TeclaE").gameObject.SetActive(true);
                coroutine = Manager.StartCoroutine(WaitForPlayerInput(Manager, collision)); 
            }
        }
    }
    public override void OnTriggerExit2DState(IOStateManager Manager, Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (coroutineIsRunning)
            {
                collision.transform.Find("TeclaE").gameObject.SetActive(false);
                Manager.StopCoroutine(coroutine);
                coroutineIsRunning = false;
            }
            else if (coroutineRestarter!=null)
            {
                Manager.StopCoroutine(coroutineRestarter);
            }
        }
    }
    public IEnumerator WaitForPlayerInput(IOStateManager Manager, Collider2D collision)
    {
        coroutineIsRunning = true;
        bool successive=false;
        while (!Input.GetKeyDown(KeyCode.E)) yield return null;
        PMStateManager Player = collision.GetComponent<PMStateManager>();
        Player.SmoothSwitchState(Player.controlOffState);
        if (Manager.canSuccessiveInteract)
        {
            if (Manager.needItemToInteract)
            {
                switch (Manager.needToInteract)
                {
                    case IOStateManager.NeedToInteract.needAllItemsToInteract:

                        Item[] itemsNeeded = new Item[Manager.itemsNeeded.Length];
                        int numberOfItemsRequiredInInventory = 0;
                        foreach (Item item in Manager.itemsNeeded)
                        {
                            foreach (Item itemInInventory in Player.playerInventoryManager.inventory.GetItemList())
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
                                    Player.playerInventoryManager.inventory.GetItemList().Remove(item);
                                }
                                Player.playerInventoryManager.uiInventory.SetInventory(Player.playerInventoryManager.inventory);
                            }
                            successive = true;
                            Manager.textBox.text = Manager.textBoxOnSuccessiveInteraction;
                        }
                        break;
                    case IOStateManager.NeedToInteract.needOneOfTheItemsToInteract:
                        bool foundItem = false;
                        foreach (Item item in Manager.itemsNeeded)
                        {
                            foreach (Item itemInInventory in Player.playerInventoryManager.inventory.GetItemList())
                            {
                                if (item.itemType == itemInInventory.itemType
                                    && item.isAged == itemInInventory.isAged
                                    && item.amount == itemInInventory.amount
                                    && item.active == itemInInventory.active)
                                {
                                    if (Manager.destroyItemOnSuccessiveInteraction)
                                    {
                                        Player.playerInventoryManager.inventory.GetItemList().Remove(itemInInventory);
                                        Player.playerInventoryManager.uiInventory.SetInventory(Player.playerInventoryManager.inventory);
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
        }
        else
        {
            Manager.textBox.text = Manager.textOnDisabledSuccessiveInteract;
        }
        Manager.textBox.pageToDisplay = 1;
        Manager.ui.SetActive(false);
        TextBoxDefineEnabled(Manager, true);
        Time.timeScale = 0;
        while (Manager.textBox.textInfo.pageCount == 0)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.1f);
        while (Manager.textBox.textInfo.pageCount - Manager.textBox.pageToDisplay >= 0)
        {
            while (!Input.GetKeyDown(KeyCode.E)) yield return null;
            Manager.textBox.pageToDisplay++;
            yield return null;
        }
        if (successive)
        {
            ResultsDataBase.Interaction(Manager.onSuccessiveInteractionConsequence, Manager, Player);
        }
        Manager.ui.SetActive(true);
        TextBoxDefineEnabled(Manager, false);
        Player.SmoothSwitchState(Player.defaultState);
        Time.timeScale = 1;
        Manager.textBox.text = "";
        coroutineIsRunning = false;
        collision.transform.Find("TeclaE").gameObject.SetActive(false);
        if (successive && Manager.singleTimeUse)
        {
            foreach (BoxCollider2D boxCollider2D in Manager.GetComponents<BoxCollider2D>())
            {
                if (boxCollider2D.isTrigger)
                {
                    boxCollider2D.enabled = false;
                }
            }
        }
        else if (!successive)
        {
            coroutineRestarter = Manager.StartCoroutine(ifStayOnTrigger(Manager, collision));
        }
        
    }
    Coroutine coroutineRestarter;
    IEnumerator ifStayOnTrigger(IOStateManager Manager,Collider2D collision)
    {
        yield return new WaitForSecondsRealtime(1);
        if (!coroutineIsRunning) coroutine = Manager.StartCoroutine(WaitForPlayerInput(Manager, collision));
    }
    private void TextBoxDefineEnabled(IOStateManager Manager, bool enabledState)
    {
        Manager.textBox.transform.parent.gameObject.SetActive(enabledState);
    }
}
