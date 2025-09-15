using UnityEngine;
using UnityEngine.UI;

public class HoldingItemScript : MonoBehaviour
{
    [Header("Visual References")]
    public SpriteRenderer itemImage;
    public SpriteRenderer jimSprite;
    public Image selected;

    [Header("Hotbar Settings")]
    public int hotbarSize = 4;
    public int hotbarItem = 0;

    [Header("Fallback Item")]
    public ItemStorable fauxItem;
    private Item faux;

    private Item heldItem;

    void Start()
    {
        faux = new Item(fauxItem, 1);
        heldItem = GetHotbarItem(hotbarItem);
    }

    void Update()
    {
        // Only update heldItem if index is valid
        if (hotbarItem >= 0 && hotbarItem < hotbarSize)
        {
            heldItem = GetHotbarItem(hotbarItem);
        }

        // Sync visuals
        selected.sprite = itemImage.sprite;

        if (heldItem != null && heldItem.item != null)
        {
            itemImage.flipX = jimSprite.flipX;
            itemImage.sprite = heldItem.item.sprite;
            selected.enabled = true;
            itemImage.enabled = true;
        }
        else
        {
            itemImage.flipX = jimSprite.flipX;
            itemImage.enabled = false;
            selected.enabled = false;
        }

        // Hotbar key checks
        CheckHotbarKey(KeyCode.Alpha1, 0);
        CheckHotbarKey(KeyCode.Alpha2, 1);
        CheckHotbarKey(KeyCode.Alpha3, 2);
        CheckHotbarKey(KeyCode.Alpha4, 3);
    }

    void CheckHotbarKey(KeyCode key, int index)
    {
        if (Input.GetKeyDown(key))
        {
            Item slot = GetHotbarItem(index);

            if (slot == null || slot.item == null)
            {
                Debug.LogWarning($"[Hotbar] Ignored null item at index {index}");
                return;
            }

            // Deselect if same item
            if (heldItem != null && slot.item == heldItem.item)
            {
                heldItem = null;
                hotbarItem = -1;
                selected.enabled = false;
                itemImage.enabled = false;
                InventoryScript.instance.SetHoldingCondition(false);
                Debug.Log("[Hotbar] Deselected held item.");
            }
            else
            {
                heldItem = slot;
                hotbarItem = index;
                selected.enabled = true;
                itemImage.enabled = true;
                InventoryScript.instance.SetHoldingCondition(true);
                Debug.Log($"[Hotbar] Switched to: {heldItem.item.name}");
            }

            InventoryScript.instance.SetSelectedIndex(index);
        }
    }

    Item GetHotbarItem(int index)
    {
        return InventoryScript.instance.GetItemFromIndex(index);
    }
}