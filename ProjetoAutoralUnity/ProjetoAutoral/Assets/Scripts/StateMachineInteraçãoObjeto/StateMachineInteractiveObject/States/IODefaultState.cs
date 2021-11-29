using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            else if (coroutineRestarter != null)
            {
                Manager.StopCoroutine(coroutineRestarter);
            }
        }
    }
    public IEnumerator WaitForPlayerInput(IOStateManager Manager, Collider2D collision)
    {
        coroutineIsRunning = true;
        bool successive = false;
        while (!Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction])) yield return null;
        PMStateManager Player = collision.GetComponent<PMStateManager>();
        if (Manager.textBox.text != "")  Player.SmoothSwitchState(Player.controlOffState);
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
                                    if (Player.playerInventoryManager.inventory.GetItemList()[Player.playerInventoryManager.activeItem-1] == item)
                                    {
                                        if (PMStateManager.Instance.playerInventoryManager.activeItem != 0) PMStateManager.Instance.playerInventoryManager.uiInventory.itemsUi[PMStateManager.Instance.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = false;
                                    }
                                    Player.playerInventoryManager.inventory.GetItemList().Remove(item);
                                }
                                if (Player.playerInventoryManager.inventory.GetItemList().Count > 0)
                                {
                                    PMStateManager.Instance.playerInventoryManager.activeItem = 1;
                                    PMStateManager.Instance.playerInventoryManager.uiInventory.itemsUi[PMStateManager.Instance.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = true;
                                }
                                else
                                {
                                    PMStateManager.Instance.playerInventoryManager.activeItem = 0;
                                }
                                Player.playerInventoryManager.uiInventory.SetInventory(Player.playerInventoryManager.inventory);
                            }
                            if (Manager.playAudio)
                            {
                                Manager.audioSource.clip = Manager.audioClip;
                                Manager.audioSource.Play();
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
                                    if (Manager.playAudio)
                                    {
                                        Manager.audioSource.clip = Manager.audioClip;
                                        Manager.audioSource.Play();
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
                if (Manager.playAudio)
                {
                    Manager.audioSource.clip = Manager.audioClip;
                    Manager.audioSource.Play();
                }
                Manager.textBox.text = Manager.textBoxOnSuccessiveInteraction;
            }
        }
        else
        {
            Manager.textBox.text = Manager.textOnDisabledSuccessiveInteract;
        }
        if (Manager.textBox.text != "")
        {
            Manager.textBox.transform.parent.Find("QG").gameObject.SetActive(false);
            //Manager.textBox.transform.parent.Find("QG").gameObject.SetActive(true);
            Manager.textBox.margin = new Vector4(0, 0, 0, 0);
            //Manager.textBox.margin = new Vector4(129, 0, 0, 0);
            //Transform transform = Manager.textBox.transform.parent.Find("QG").Find("Image");
            //qgAnimation = Manager.StartCoroutine(QGAnimation(transform.GetComponent<TextBoxAnimations>(), transform.GetComponent<Image>(), Player.timePeriod));
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
                while (!Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction])) yield return null;
                Manager.textBox.pageToDisplay++;
                yield return null;
            }
        }
        if (successive)
        {
            ResultsDataBase.Interaction(Manager.onSuccessiveInteractionConsequence, Manager, Player);
        }
        if (Manager.textBox.text != "")
        {
            if (qgAnimation != null)
            {
                Manager.StopCoroutine(qgAnimation);
                qgAnimation = null;
            }
            Manager.ui.SetActive(true);
            TextBoxDefineEnabled(Manager, false);
            Player.SmoothSwitchState(Player.defaultState);
            Time.timeScale = 1;
            Manager.textBox.text = "";
        }
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
    private Coroutine qgAnimation;
    private IEnumerator QGAnimation(TextBoxAnimations textBoxAnimations,Image image,PMStateManager.timePeriodList timePeriod)
    {
        int i = 0;
        switch(timePeriod)
        {
            case PMStateManager.timePeriodList.Present:
                while (true)
                {
                    switch (i)
                    {
                        case 0:
                            image.sprite = textBoxAnimations.QGP_0;
                            break;                             
                        case 1:                                
                            image.sprite = textBoxAnimations.QGP_1;
                            break;                             
                        case 2:                                
                            image.sprite = textBoxAnimations.QGP_2;
                            break;                             
                        case 3:                                
                            image.sprite = textBoxAnimations.QGP_3;
                            break;                             
                        case 4:                                
                            image.sprite = textBoxAnimations.QGP_4;
                            break;                            
                        case 5:                                
                            image.sprite = textBoxAnimations.QGP_5;
                            break;                             
                        case 6:                                
                            image.sprite = textBoxAnimations.QGP_6;
                            break;                             
                        case 7:                                
                            image.sprite = textBoxAnimations.QGP_7;
                            break;                             
                        case 8:                                
                            image.sprite = textBoxAnimations.QGP_8;
                            break;                             
                        case 9:                                
                            image.sprite = textBoxAnimations.QGP_9;
                            break;                            
                        case 10:                               
                            image.sprite = textBoxAnimations.QGP_10;
                            break;                             
                        case 11:                               
                            image.sprite = textBoxAnimations.QGP_11;
                            break;
                    }
                    i++;
                    if (i >= 11) i = 0;
                    yield return new WaitForSecondsRealtime(0.1f);
                }
            case PMStateManager.timePeriodList.Future:
                while (true)
                {
                    switch (i)
                    {
                        case 0:
                            image.sprite = textBoxAnimations.QGF_0;
                            break;
                        case 1:
                            image.sprite = textBoxAnimations.QGF_1;
                            break;
                        case 2:
                            image.sprite = textBoxAnimations.QGF_2;
                            break;
                        case 3:
                            image.sprite = textBoxAnimations.QGF_3;
                            break;
                        case 4:
                            image.sprite = textBoxAnimations.QGF_4;
                            break;
                        case 5:
                            image.sprite = textBoxAnimations.QGF_5;
                            break;
                        case 6:
                            image.sprite = textBoxAnimations.QGF_6;
                            break;
                        case 7:
                            image.sprite = textBoxAnimations.QGF_7;
                            break;
                        case 8:
                            image.sprite = textBoxAnimations.QGF_8;
                            break;
                        case 9:
                            image.sprite = textBoxAnimations.QGF_9;
                            break;
                        case 10:
                            image.sprite = textBoxAnimations.QGF_10;
                            break;
                        case 11:
                            image.sprite = textBoxAnimations.QGF_11;
                            break;
                    }
                    i++;
                    if (i >= 11) i = 0;
                    yield return new WaitForSecondsRealtime(0.1f);
                }
        }
    }
    IEnumerator ifStayOnTrigger(IOStateManager Manager, Collider2D collision)
    {
        yield return new WaitForSecondsRealtime(1);
        if (!coroutineIsRunning) coroutine = Manager.StartCoroutine(WaitForPlayerInput(Manager, collision));
    }
    private void TextBoxDefineEnabled(IOStateManager Manager, bool enabledState)
    {
        Manager.textBox.transform.parent.gameObject.SetActive(enabledState);
    }
}
