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
            case "SofaFuturo":
                player.playerInventoryManager.inventory.AddItem(new Item {itemType = Item.ItemType.KeyCabine1, amount = 1, isAged = true });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "BanheiraPresente":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyExit1, amount = 1, isAged = false });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = bathSprite.Instance.emptyBathPresent;
                GameObject.Find("BanheiraF").GetComponent<SpriteRenderer>().sprite = bathSprite.Instance.emptyBathFuture;
                GameObject.Find("BanheiraF").GetComponent<IOStateManager>().enabled = false;
                break;
            case "BanheiraFuturo":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyExit1, amount = 1, isAged = true });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = bathSprite.Instance.emptyBathFuture;
                manager.gameObject.GetComponent<IOStateManager>().enabled = false;
                break;
            case "CabinePia":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Plunger, amount = 1});
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                MenuConfigs.Instance.PuzzleStep = 3;
                break;
            case "PanoEstante":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Cloth, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "PorcelanaQuebradaPres":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyCabine2, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                Object.Destroy(GameObject.Find("PorcelanaP"));
                Object.Destroy(GameObject.Find("PorcelanaF"));
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
                MenuConfigs.Instance.PuzzleStep = 4;
                break;
            case "GavetasPiaCozinha":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Matches, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                MenuConfigs.Instance.PuzzleStep = 3;
                break;
            case "Descongelar":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyEscrit, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "AbrirEscritorio":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontPresentOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case "Escrivaninha":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Stilleto, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                MenuConfigs.Instance.PuzzleStep = 6;
                break;
            case "ArmarioQuarto":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.ClothesHanger, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                MenuConfigs.Instance.PuzzleStep = 7;
                break;
            case "Ralo":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyExit2, amount = 1 });
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
            //cases do Puzzle3
            #region
            case "PalitoDente":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.ToothPick, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                MenuConfigs.Instance.PuzzleStep = 3;
                break;
            case "GrampoPrivada":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.HairClip, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                MenuConfigs.Instance.PuzzleStep = 2;
                break;
            case "Bau":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.LoginPass, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "Computador":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.SecretPass, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "GloboQuebrado":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.KeyExit3, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "Compartimento":
                player.playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Screwdriver, amount = 1 });
                player.playerInventoryManager.uiInventory.SetInventory(player.playerInventoryManager.inventory);
                break;
            case "ExitPuzzle3Present":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontPresentOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case "ExitPuzzle3Future":
                manager.gameObject.GetComponent<SpriteRenderer>().sprite = DoorSprite.Instance.doorFrontFutureOpen;
                manager.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            #endregion
            case "fogoSofa":
                AiBoss aiBoss = AiBoss.Instance;
                player.transform.Find("Flashlights").GetComponent<PolygonCollider2D>().enabled = false;
                aiBoss.target = manager.gameObject;
                if(aiBoss.followRoute!=null)aiBoss.StopCoroutine(aiBoss.followRoute);
                aiBoss.followRoute = null;
                aiBoss.enragedCharging = true;
                aiBoss.animator.SetTrigger("Preparation");
                manager.transform.Find("ParticleEmissorSmoke").GetComponent<ParticleSystem>().Play();
                manager.transform.Find("fogoMovel").GetComponent<Animator>().SetBool("onFire", true);
                manager.transform.Find("fogoMovel2").GetComponent<Animator>().SetBool("onFire", true);
                Transform FireLight2D = manager.transform.Find("FireLight2D");
                FireLight2D.gameObject.SetActive(true);
                FireLight2D.GetComponent<FireLight2D>().StartCoroutine(FireLight2D.GetComponent<FireLight2D>().LightIntensityWave());
                break;
        }
    }
}
