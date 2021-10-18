using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [System.NonSerialized] public int activeItem;
    [System.NonSerialized] public Inventory inventory;
    public Ui_Inventory uiInventory;
    private void Awake()
    {
        inventory = new Inventory(this);
    }
    void Start()
    {
        uiInventory.SetInventory(inventory);
    }
    Coroutine coroutine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            coroutine = StartCoroutine(WaitForInput(itemWorld));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            StopCoroutine(coroutine);
        }
    }
    private IEnumerator WaitForInput(ItemWorld itemW)
    {
        Debug.Log("a");
        while(!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
        inventory.AddItem(itemW.GetItem());
        itemW.DestroySelf();
    }
}
