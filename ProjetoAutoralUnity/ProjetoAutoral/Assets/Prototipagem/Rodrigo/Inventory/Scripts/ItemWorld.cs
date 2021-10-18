using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        return itemWorld;
    }
    public static ItemWorld DropItem(Item item,PMStateManager player)
    {
        Tilemap tilemap;
        if (player.timePeriod == PMStateManager.timePeriodList.Present)
        {
            tilemap = player.tilemapPresente.GetComponent<Tilemap>();
        }
        else tilemap = player.tilemapFuturo.GetComponent<Tilemap>();
        Vector3 dropGridPosition = tilemap.WorldToCell(player.transform.position) + new Vector3Int(player.facingDirection.x, player.facingDirection.y,0);
        dropGridPosition += new Vector3(0.5f, 0.5f);
        return SpawnItemWorld(dropGridPosition, item);
    }
    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = transform.Find("AmountText").GetComponent<TextMeshPro>();
    }
    public void SetItem(Item item )
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1) text.SetText(item.amount.ToString());
        else text.SetText("");
    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
