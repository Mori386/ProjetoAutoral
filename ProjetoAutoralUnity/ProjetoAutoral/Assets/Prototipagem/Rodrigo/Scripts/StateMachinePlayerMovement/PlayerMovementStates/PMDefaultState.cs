using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PMDefaultState : PMBaseState
{
    public override void EnterState(PMStateManager Manager)
    {

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
        }
        if (Input.GetKeyDown(KeyCode.C) && Manager.playerInventoryManager.activeItem > 0)
        {
            Item item = Manager.playerInventoryManager.inventory.GetItemList()[Manager.playerInventoryManager.activeItem - 1];
            Manager.playerInventoryManager.inventory.RemoveItem(Manager.playerInventoryManager.activeItem - 1);
            if (Manager.playerInventoryManager.inventory.GetItemList().Count > 0)
            {
                if (Manager.playerInventoryManager.activeItem == Manager.playerInventoryManager.inventory.GetItemList().Count + 1)
                {
                    Manager.playerInventoryManager.activeItem--; Debug.Log(Manager.playerInventoryManager.inventory.GetItemList().Count);
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
    public override void FixedUpdateState(PMStateManager Manager)
    {
        Vector2 direction = new Vector2(Manager.rawInputMove.x / Mathf.Abs(Manager.rawInputMove.x), Manager.rawInputMove.y / Mathf.Abs(Manager.rawInputMove.y));
        if (direction.x > 0)
        {
            Manager.facingDirection = new Vector2Int(1, 0);
        }
        else if (direction.x < 0)
        {
            Manager.facingDirection = new Vector2Int(-1, 0);
        }
        else if (direction.y > 0)
        {
            Manager.facingDirection = new Vector2Int(0, 1);
        }
        else if (direction.y < 0)
        {
            Manager.facingDirection = new Vector2Int(0, -1);
        }
        Manager.rb.MovePosition(Manager.rb.position + Manager.regulatorDirection(Manager.rawInputMove) * Manager.moveSpeed * Time.fixedDeltaTime);
    }
}
