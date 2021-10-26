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
    public static ItemWorld DropItem(Item item, PMStateManager player)
    {
        Tilemap tilemap;
        if (player.timePeriod == PMStateManager.timePeriodList.Present)
        {
            tilemap = player.tilemapPresente.GetComponent<Tilemap>();
        }
        else tilemap = player.tilemapFuturo.GetComponent<Tilemap>();
        Vector3 dropGridPosition = CellPositionCenter(player.transform.position - new Vector3(0, player.GetComponent<SpriteRenderer>().bounds.size.y / 2));
        dropGridPosition -= Vector3.Scale(new Vector3(0.5f, 0.5f), tilemap.cellSize);
        if (!item.isAged)
        {
            ItemWorld itemWorld = SpawnItemWorld(dropGridPosition, item);
            itemWorld.GetItem().itemInFuture = ItemWorld.SpawnItemInFuture(item, dropGridPosition).GetItem();
            return itemWorld;
        }
        else
        {
            return SpawnItemWorld(dropGridPosition, item);
        }
    }
    public static Vector3 CellPositionCenter(Vector3 position)
    {
        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();
        Vector3 nearGridPosition = new Vector3(0, 0, 0);
        if (nearGridPosition.x > 0)
        {
            nearGridPosition.x = Mathf.Floor(position.x / grid.cellSize.x) * grid.cellSize.x;
        }
        else
        {
            nearGridPosition.x = Mathf.Ceil(position.x / grid.cellSize.x) * grid.cellSize.x;
        }
        if (nearGridPosition.y > 0)
        {
            nearGridPosition.y = Mathf.Floor(position.y / grid.cellSize.y) * grid.cellSize.y;
        }
        else
        {
            nearGridPosition.y = Mathf.Ceil(position.y / grid.cellSize.y) * grid.cellSize.y;
        }
        return nearGridPosition;
    }
    public static ItemWorld SpawnItemInFuture(Item itemPresent, Vector3 itemPresentGridPosition)
    {
        PMStateManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PMStateManager>();
        return SpawnItemWorld(itemPresentGridPosition + player.tilemapFuturo.transform.position - player.tilemapPresente.transform.position, new Item { itemType = itemPresent.itemType, amount = itemPresent.amount, itemTimeline = Item.ItemTimeline.Future, isAged = true });
    }
    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = transform.Find("AmountText").GetComponent<TextMeshPro>();
    }
    public void SetItem(Item item)
    {
        this.item = item;
        item.itemWorld = this;
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
