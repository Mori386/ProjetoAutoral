using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PMDefaultState : PMBaseState
{
    public override void EnterState(PMStateManager Manager)
    {
        focusOnCursorOff = Manager.StartCoroutine(FocusOnCursorOff(Manager));
    }
    private int inventoryCount;
    public override void UpdateState(PMStateManager Manager)
    {
        Manager.rawInputMove.x = Input.GetAxisRaw("Horizontal");//adiciona variaveis baseadas no input de teclas(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
        Manager.rawInputMove.y = Input.GetAxisRaw("Vertical");// mesma coisa que o de cima so que para os botoes de mover na vertical
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Manager.TravelTime();
        }
        inventoryCount = Manager.playerInventoryManager.inventory.GetItemList().Count;
        if (inventoryCount == 0)
        {
            Manager.playerInventoryManager.activeItem = 0;
            Manager.animator.SetBool("FLASHLIGHT", false);
        }
        else
        {
            for (int i = 1; i <= inventoryCount; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    if (Manager.playerInventoryManager.activeItem != 0) Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = false;
                    Manager.playerInventoryManager.activeItem = i;
                    Manager.playerInventoryManager.uiInventory.itemsUi[Manager.playerInventoryManager.activeItem - 1].Find("Border").GetComponent<Outline>().enabled = true;
                }
            }
            if (Manager.playerInventoryManager.inventory.GetItemList()[Manager.playerInventoryManager.activeItem - 1].itemType == Item.ItemType.Flashlight && Manager.playerInventoryManager.inventory.GetItemList()[Manager.playerInventoryManager.activeItem - 1].isAged == false)
            {
                Manager.animator.SetBool("FLASHLIGHT", true);
            }
            else
            {
                Manager.animator.SetBool("FLASHLIGHT", false);
            }
        }
        if (Input.GetKeyDown(KeyCode.C) && Manager.playerInventoryManager.activeItem > 0)
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            focusOnCursorOn = Manager.StartCoroutine(FocusOnCursorOn(Manager));
            Manager.StopCoroutine(focusOnCursorOff);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Manager.StopCoroutine(focusOnCursorOn);
            focusOnCursorOff = Manager.StartCoroutine(FocusOnCursorOff(Manager));
            if (Manager.facingDirection.x != 0)
            {
                if (Manager.facingDirection.x == 1) angle = -90;
                else if (Manager.facingDirection.x == -1) angle = 90;
            }
            else if (Manager.facingDirection.y != 0)
            {
                if (Manager.facingDirection.y == 1) angle = 0;
                else if (Manager.facingDirection.y == -1) angle = 180;
            }
            Manager.flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    float angle;
    private Coroutine focusOnCursorOn;
    private Coroutine focusOnCursorOff;
    private IEnumerator FocusOnCursorOn(PMStateManager Manager)
    {
        Vector2 mousePos;
        while (true)
        {
            mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            mousePos = Manager.transform.InverseTransformPoint(mousePos);
            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
            if (angle > -45 && angle <= 45)
            {
                Manager.facingDirection = new Vector2Int(0, 1);
            }
            else if (angle > 45 && angle <= 90 || angle < -225 && angle <= -270)
            {
                Manager.facingDirection = new Vector2Int(-1, 0);
            }
            else if (angle <= -45 && angle > -135)
            {
                Manager.facingDirection = new Vector2Int(1, 0);
            }
            else if (angle <= -135 && angle >= -225)
            {
                Manager.facingDirection = new Vector2Int(0, -1);
            }
            Manager.flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
            float FACINGDIRECTION = Manager.facingDirection.x + Manager.facingDirection.y / 10;
            Manager.animator.SetInteger("FACINGDIRECTIONX", Manager.facingDirection.x);
            Manager.animator.SetInteger("FACINGDIRECTIONY", Manager.facingDirection.y);
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator FocusOnCursorOff(PMStateManager Manager)
    {
        Vector2 direction;
        while (true)
        {
            direction = new Vector2(Manager.rawInputMove.x / Mathf.Abs(Manager.rawInputMove.x), Manager.rawInputMove.y / Mathf.Abs(Manager.rawInputMove.y));
            if (direction.x > 0)
            {
                Manager.facingDirection = new Vector2Int(1, 0);
                angle = -90;
            }
            else if (direction.x < 0)
            {
                Manager.facingDirection = new Vector2Int(-1, 0);
                angle = 90;
            }
            else if (direction.y > 0)
            {
                Manager.facingDirection = new Vector2Int(0, 1);
                angle = 0;
            }
            else if (direction.y < 0)
            {
                Manager.facingDirection = new Vector2Int(0, -1);
                angle = 180;
            }
            Manager.flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
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
        }
        else
        {
            Manager.animator.SetBool("MOVING", false);
            AudioSource audioData = Manager.GetComponent<AudioSource>();
            audioData.Play(0);
        }
        Manager.rb.MovePosition(Manager.rb.position + regulatedDirection * Manager.moveSpeed * Time.fixedDeltaTime);
    }
}
