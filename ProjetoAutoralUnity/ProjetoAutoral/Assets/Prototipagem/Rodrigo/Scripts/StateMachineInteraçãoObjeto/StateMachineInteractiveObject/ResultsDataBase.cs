using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsDataBase : MonoBehaviour
{
    public static void Interaction(string result, IOStateManager manager, PMStateManager player)
    {
        switch (result)
        {
            //cases do Puzzle1
            #region
            case "SofaPresente":
                Debug.Log("InteraçãoSucesso");
                break;
            case "SofaFuturo":
                player.playerInventoryManager.inventory.AddItem(new Item {itemType = Item.ItemType.KeyCabine1, amount = 1, isAged = true });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "BanheiraPresente":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyExit1, amount = 1, isAged = false });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = bathSprite.Instance.emptyBathPresent;
                break;
            case "BanheiraFuturo":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyExit1, amount = 1, isAged = true });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = bathSprite.Instance.emptyBathFuture;
                break;
            case "CabinePia":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Plunger, amount = 1});
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "PanoEstante":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Cloth, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "PorcelanaQuebrada":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyCabine2, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "ExitPuzzle1Present":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontPresentOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case "ExitPuzzle1Future":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontFutureOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            #endregion
            //cases do Puzzle2
            #region
            case "Geladeira":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyEscritCongela, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "GavetasPia":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Matches, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "Descongelar":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyEscrit, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "Armario quarto":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.ClothesHanger, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "ExitPuzzle2Present":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontPresentOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case "ExitPuzzle2Future":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontFutureOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
                #endregion
        }
    }
}
