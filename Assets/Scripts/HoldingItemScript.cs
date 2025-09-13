using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldingItemScript : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer itemImage;
    public SpriteRenderer jimSprite;
    public InventorySlot heldItem;
    public ItemStorable fauxItem;
    public InventorySlot faux;
    public int hotbarItem;
    void Start()
    {
        faux = new InventorySlot(fauxItem, 1);
        heldItem = InventoryScript.instance.GetItemFromIndex(0); // just data
    }

    // Update is called once per frame
    void Update()
    {
        itemImage.flipX = jimSprite.flipX;

        CheckHotbarKey(KeyCode.Alpha1, 0);
        CheckHotbarKey(KeyCode.Alpha2, 1);
        CheckHotbarKey(KeyCode.Alpha3, 2);
        CheckHotbarKey(KeyCode.Alpha4, 3);


        if (heldItem != null && heldItem.item != null)
        {
            itemImage.sprite = heldItem.item.sprite;
            itemImage.enabled = true;

        }
        else
        {
            itemImage.enabled = false;

        }
    }
    void CheckHotbarKey(KeyCode key, int index)
    {
        if (Input.GetKeyDown(key))
        {
            InventorySlot slot = InventoryScript.instance.GetItemFromIndex(index);
            hotbarItem = index;

            // If same item is already held → clear it
            if (heldItem != null && slot.item == heldItem.item)
            {
                heldItem = null;
            }
            else if (slot.item != null) // Switch to new item
            {
                heldItem = slot;
                Debug.Log("Switched to: " + heldItem.item.name);
            }
        }

    }
}


