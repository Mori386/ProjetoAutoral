using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private PlayerInventoryManager playerInventoryManager;
    public RectTransform[] itemsUi;
    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
        playerInventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>();
        itemsUi = new RectTransform[3];
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }
    bool coroutineRunning;
    Coroutine itemActivationCheckClick;
    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        float itemSlotCellSize = 100f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemsUi[x] = itemSlotRectTransform;
            if (playerInventoryManager.activeItem == x + 1) itemSlotRectTransform.Find("Border").GetComponent<Outline>().enabled = true;
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, 0);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI text = itemSlotRectTransform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1) text.SetText(item.amount.ToString());
            else
            {
                text.SetText("");
            }
            x++;
        }
        if (inventory.GetItemList().Count > 0)
        {
            if (!coroutineRunning) itemActivationCheckClick = playerInventoryManager.StartCoroutine(itemActivation());
        }
        else
        {
            if (coroutineRunning)
            {
                playerInventoryManager.StopCoroutine(itemActivationCheckClick);
                coroutineRunning = false;
            }
        }
    }
    IEnumerator itemActivation()
    {
        while (inventory.GetItemList().Count > 0)
        {
            coroutineRunning = true;
            while (!Input.GetKeyDown(KeyCode.Mouse0))
            {
                yield return null;
            }
            inventory.UseItem(playerInventoryManager.activeItem - 1);
            yield return null;
        }
    }
}
