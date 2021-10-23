using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;
    private void Start()
    {
        ItemWorld.SpawnItemWorld(transform.position, item).GetItem().itemInFuture = ItemWorld.SpawnItemInFuture(item, transform.position).GetItem();
        Destroy(gameObject);
    }
}
