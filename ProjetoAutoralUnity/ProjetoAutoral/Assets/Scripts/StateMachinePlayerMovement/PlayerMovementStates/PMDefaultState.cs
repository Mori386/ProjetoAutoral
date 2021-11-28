using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PMDefaultState : PMBaseState
{

    public override void EnterState(PMStateManager Manager)
    {
        ManagerTTC = Manager;
        focusOnCursorOff = Manager.StartCoroutine(FocusOnCursorOff(Manager));
    }
    private int inventoryCount;
    public override void UpdateState(PMStateManager Manager)
    {
        if (Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveUp]) & Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveDown]))
        {
            Manager.rawInputMove.y = 0;
        }
        else if (Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveUp]))
        {
            Manager.rawInputMove.y = 1;
        }
        else if (Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveDown]))
        {
            Manager.rawInputMove.y = -1;
        }
        else Manager.rawInputMove.y = 0;

        if (Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveLeft]) & Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveRight]))
        {
            Manager.rawInputMove.x = 0;
        }
        else if (Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveRight]))
        {
            Manager.rawInputMove.x = 1;
        }
        else if (Input.GetKey(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.MoveLeft]))
        {
            Manager.rawInputMove.x = -1;
        }
        else Manager.rawInputMove.x = 0;
        if (interactionCheck != null) interactionCheck();
        inventoryCount = Manager.playerInventoryManager.inventory.GetItemList().Count;
        if (inventoryCount == 0)
        {
            Manager.playerInventoryManager.activeItem = 0;
            Manager.flashlightLP.transform.parent.gameObject.SetActive(false);
            Manager.animator.SetBool("FLASHLIGHT", false);
        }
        else
        {
            if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.InventorySlot1]))
            {
                if (Manager.playerInventoryManager.activeItem != 0) Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = false;
                Manager.playerInventoryManager.activeItem = 1;
                Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = true;
            }
            else if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.InventorySlot2]) && inventoryCount > 1)
            {
                if (Manager.playerInventoryManager.activeItem != 0) Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = false;
                Manager.playerInventoryManager.activeItem = 2;
                Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = true;
            }
            else if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.InventorySlot3]) && inventoryCount > 2)
            {
                if (Manager.playerInventoryManager.activeItem != 0) Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = false;
                Manager.playerInventoryManager.activeItem = 3;
                Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = true;
            }
            if (Manager.playerInventoryManager.inventory.GetItemList()[Manager.playerInventoryManager.activeItem - 1].itemType == Item.ItemType.Flashlight && Manager.playerInventoryManager.inventory.GetItemList()[Manager.playerInventoryManager.activeItem - 1].isAged == false)
            {
                Manager.animator.SetBool("FLASHLIGHT", true);
            }
            else
            {
                Manager.flashlightLP.transform.parent.gameObject.SetActive(false);
                Manager.animator.SetBool("FLASHLIGHT", false);
            }
        }
        if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.DropItem]) && Manager.playerInventoryManager.activeItem > 0)
        {
            Vector3 dropGridPosition = ItemWorld.CellPositionCenter(Manager.transform.position - new Vector3(0, Manager.GetComponent<SpriteRenderer>().bounds.size.y / 2));
            dropGridPosition -= Vector3.Scale(new Vector3(0.5f, 0.5f), new Vector2(0.64f, 0.64f));
            RaycastHit2D[] otherItemFound = Physics2D.BoxCastAll(
                dropGridPosition
                , new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, 2048);
            if (otherItemFound.Length <= 0)
            {
                Item item = Manager.playerInventoryManager.inventory.GetItemList()[Manager.playerInventoryManager.activeItem - 1];
                Manager.playerInventoryManager.inventory.RemoveItem(Manager.playerInventoryManager.activeItem - 1);
                if (Manager.playerInventoryManager.inventory.GetItemList().Count > 0)
                {
                    if (Manager.playerInventoryManager.activeItem == Manager.playerInventoryManager.inventory.GetItemList().Count + 1)
                    {
                        Manager.playerInventoryManager.activeItem--;
                    }
                    Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Manager.playerInventoryManager.activeItem--;
                }
                ItemWorld.DropItem(item, Manager);
            }
        }
        if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight]))
        {
            focusOnCursorOn = Manager.StartCoroutine(FocusOnCursorOn(Manager));
            Manager.StopCoroutine(focusOnCursorOff);
        }
        else if (Input.GetKeyUp(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight]))
        {
            Manager.StopCoroutine(focusOnCursorOn);
            focusOnCursorOff = Manager.StartCoroutine(FocusOnCursorOff(Manager));
            if (Manager.facingDirection.x != 0)
            {
                if (Manager.facingDirection.x == 1)
                {
                    Manager.flashlightLP.SetActive(false);
                    Manager.flashlightNLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(0.129f, -0.008f, 0);
                    angle = -90;
                }
                else if (Manager.facingDirection.x == -1)
                {
                    Manager.flashlightLP.SetActive(false);
                    Manager.flashlightNLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(-0.139f, -0.01f, 0);
                    angle = 90;
                }
            }
            else if (Manager.facingDirection.y != 0)
            {
                if (Manager.facingDirection.y == 1)
                {
                    Manager.flashlightLP.SetActive(false);
                    Manager.flashlightNLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(0.102f, -0.051f, 0);
                    angle = 0;
                }
                else if (Manager.facingDirection.y == -1)
                {
                    Manager.flashlightNLP.SetActive(false);
                    Manager.flashlightLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(-0.0732f, -0.0342f, 0);
                    angle = 180;
                }
            }
            Manager.flashlightLP.transform.parent.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Radio]))
        {
            if(MenuConfigs.Instance.Puzzle != 4) Manager.StartCoroutine(TextBoxQG(Manager));
        }
        if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu]))
        {
            Manager.menuUI.SetActive(true);
            Manager.SmoothSwitchState(Manager.controlOffState);
            Time.timeScale = 0;
        }
    }
    private IEnumerator TextBoxQG(PMStateManager Manager)
    {
        Manager.SmoothSwitchState(Manager.controlOffState);
        Manager.textBox.transform.parent.Find("QG").gameObject.SetActive(true);
        Manager.textBox.margin = new Vector4(129, 0, 0, 0);
        Transform transform = Manager.textBox.transform.parent.Find("QG").Find("Image");
        qgAnimation = Manager.StartCoroutine(QGAnimation(transform.GetComponent<TextBoxAnimations>(), transform.GetComponent<Image>(), Manager.timePeriod));
        Manager.textBox.pageToDisplay = 1;
        Manager.ui.SetActive(false);
        Manager.textBox.transform.parent.gameObject.SetActive(true);
        Time.timeScale = 0;
        if (Manager.timePeriod == PMStateManager.timePeriodList.Present)
        {
            switch (MenuConfigs.Instance.Puzzle)
            {
                case 1:
                    Manager.textBox.text = controleQG.puzzle1QG(MenuConfigs.Instance.PuzzleStep);
                    break;
                case 2:
                    Manager.textBox.text = controleQG.puzzle2QG(MenuConfigs.Instance.PuzzleStep);
                    break;
                case 3:
                    Manager.textBox.text = controleQG.puzzle3QG(MenuConfigs.Instance.PuzzleStep);
                    break;
            }
        }
        else
        {
            Manager.textBox.text = controleQG.FailQG();
        }
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
        if (qgAnimation != null)
        {
            Manager.StopCoroutine(qgAnimation);
            qgAnimation = null;
        }
        Manager.ui.SetActive(true);
        Manager.textBox.transform.parent.gameObject.SetActive(false);
        Manager.SmoothSwitchState(Manager.defaultState);
        Time.timeScale = 1;
        Manager.textBox.text = "";
    }
    private Coroutine qgAnimation;
    private IEnumerator QGAnimation(TextBoxAnimations textBoxAnimations, Image image, PMStateManager.timePeriodList timePeriod)
    {
        int i = 0;
        switch (timePeriod)
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
    [System.NonSerialized] public float angle;
    public Coroutine focusOnCursorOn;
    public Coroutine focusOnCursorOff;
    PMStateManager ManagerTTC;
    public delegate void InteractionCheck();
    public InteractionCheck interactionCheck;

    bool TimeTravelOnCooldown;
    [System.NonSerialized] public bool isEnemyOn;
    public void TimeTravelCheck()
    {
        if (!TimeTravelOnCooldown)
        {
            if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.TimeTravel]))
            {
                if (ManagerTTC.director != null) ManagerTTC.director.Play();
                ManagerTTC.TravelTime();
                if (isEnemyOn)
                {
                    if (AIEnemyLight.Instance.gameObject.activeInHierarchy) AIEnemyLight.Instance.gameObject.SetActive(false);
                }
                else if (isEnemyOn)
                {

                    if(!AIEnemyLight.Instance.gameObject.activeInHierarchy)  AIEnemyLight.Instance.gameObject.SetActive(true);
                }
                ManagerTTC.StartCoroutine(TimeTravelCooldown());
            }
        }
    }
    private IEnumerator TimeTravelCooldown()
    {
        TimeTravelOnCooldown = true;
        yield return new WaitForSeconds(((float)ManagerTTC.director.duration));
        TimeTravelOnCooldown = false;
    }
    public IEnumerator FocusOnCursorOn(PMStateManager Manager)
    {
        GameObject flashlight = Manager.flashlightNLP;
        Vector2 mousePos;
        while (true)
        {
            mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            mousePos = Manager.transform.InverseTransformPoint(mousePos);
            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
            if (angle > -45 && angle <= 45)
            {
                Manager.facingDirection = new Vector2Int(0, 1);
                if (flashlight != Manager.flashlightNLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightNLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(0.102f, -0.051f, 0);

            }
            else if (angle > 45 && angle <= 90 || angle < -225 && angle <= -270)
            {
                Manager.facingDirection = new Vector2Int(-1, 0);
                if (flashlight != Manager.flashlightNLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightNLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(-0.139f, -0.01f, 0);
            }
            else if (angle <= -45 && angle > -135)
            {
                Manager.facingDirection = new Vector2Int(1, 0);
                if (flashlight != Manager.flashlightNLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightNLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(0.129f, -0.008f, 0);
            }
            else if (angle <= -135 && angle >= -225)
            {
                Manager.facingDirection = new Vector2Int(0, -1);
                if (flashlight != Manager.flashlightLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(-0.0732f, -0.0342f, 0);
            }
            flashlight.transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            float FACINGDIRECTION = Manager.facingDirection.x + Manager.facingDirection.y / 10;
            Manager.animator.SetInteger("FACINGDIRECTIONX", Manager.facingDirection.x);
            Manager.animator.SetInteger("FACINGDIRECTIONY", Manager.facingDirection.y);
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator FocusOnCursorOff(PMStateManager Manager)
    {
        Vector2 direction;
        GameObject flashlight = Manager.flashlightNLP;
        while (true)
        {
            direction = new Vector2(Manager.rawInputMove.x / Mathf.Abs(Manager.rawInputMove.x), Manager.rawInputMove.y / Mathf.Abs(Manager.rawInputMove.y));
            if (direction.x > 0)
            {
                Manager.facingDirection = new Vector2Int(1, 0);
                if (flashlight != Manager.flashlightNLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightNLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(0.129f, -0.008f, 0);
                angle = -90;
            }
            else if (direction.x < 0)
            {
                Manager.facingDirection = new Vector2Int(-1, 0);
                if (flashlight != Manager.flashlightNLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightNLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(-0.139f, -0.01f, 0);
                angle = 90;
            }
            else if (direction.y > 0)
            {
                Manager.facingDirection = new Vector2Int(0, 1);
                if (flashlight != Manager.flashlightNLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightNLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(0.102f, -0.051f, 0);
                angle = 0;
            }
            else if (direction.y < 0)
            {
                Manager.facingDirection = new Vector2Int(0, -1);
                if (flashlight != Manager.flashlightLP)
                {
                    flashlight.SetActive(false);
                    flashlight = Manager.flashlightLP;
                    flashlight.SetActive(true);
                }
                flashlight.transform.parent.localPosition = new Vector3(-0.0732f, -0.0342f, 0);
                angle = 180;
            }
            flashlight.transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            Manager.animator.SetInteger("FACINGDIRECTIONX", Manager.facingDirection.x);
            Manager.animator.SetInteger("FACINGDIRECTIONY", Manager.facingDirection.y);
            yield return new WaitForFixedUpdate();
        }
    }
    public override void FixedUpdateState(PMStateManager Manager)
    {
        Vector2 regulatedDirection = Manager.regulatorDirection(Manager.rawInputMove);
        if (regulatedDirection.x != 0 || regulatedDirection.y != 0)
        {
            Manager.animator.SetBool("MOVING", true);
            if (!Manager.audioSourceWalk.isPlaying)
            {
                switch (Random.Range(1, 10))
                {
                    case 1:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep1;
                        break;
                    case 2:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep2;
                        break;
                    case 3:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep3;
                        break;
                    case 4:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep4;
                        break;
                    case 5:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep5;
                        break;
                    case 6:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep6;
                        break;
                    case 7:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep7;
                        break;
                    case 8:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep8;
                        break;
                    case 9:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep9;
                        break;
                    case 10:
                        Manager.audioSourceWalk.clip = Manager.audioClipStep10;
                        break;

                }
                Manager.audioSourceWalk.Play();
            }
        }
        else
        {
            Manager.animator.SetBool("MOVING", false);
        }
        Manager.rb.MovePosition(Manager.rb.position + regulatedDirection * Manager.moveSpeed * Time.fixedDeltaTime);
    }
}
