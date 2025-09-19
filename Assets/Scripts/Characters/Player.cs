using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Player : Character
{
    protected Character player;
    public ItemStorable itemPaketTest;

    public Item heldItem;
    private ItemInWorld takeableItem;
    public Transform parentOfInventory;
    public Transform parentOfHotbar;
    public InventoryGUI guiHandler;

    public void Start()
    {

        Initialize("Player", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, true, true, 0, this.GetComponent<HoldingItemScript>());
        movement = gameObject.AddComponent<PlayerMovement>();
        InventorySetup();
        guiHandler = gameObject.AddComponent<InventoryGUI>();
        heldItem = inventory.GetItemUsingIndex(itemHeld);
        holdingItemManager.spriteHolderImage.sprite = heldItem.item.sprite;
        holdingItemManager.spriteTopLeftImage.sprite = heldItem.item.sprite;

    }
    public void Update()
    {


        if (recievable && Input.GetKeyDown(KeyCode.E))
        {
            if (takeableItem.takeable && takeableItem.amountOfItemsHere > 0)
            {
                Item itemAdded = new Item(takeableItem.itemInWorld, 1);
                inventory.AddItem(itemAdded);
                // Debug.Log("item added: " + takeableItem.itemInWorld.name + ",inv index: " + inventory.GetItemIndex(itemAdded));
            }
        }
        if (canGive && Input.GetKeyDown(KeyCode.Q))
        {
            inventory.GetItemUsingIndex(itemHeld).quantity--;
        }
        CheckForHotbarInput();




        //     

        //             if (slot == null || slot.item == null)
        //             {
        //                 Debug.LogWarning($"[Hotbar] Ignored null item at index {index}");
        //                 return;
        //             }

        //             // Deselect if same item
        //             if (player.heldItem != null && slot.item == player.heldItem.item)
        //             {
        //                 player.heldItem = null;
        //                 hotbarItem = -1;
        //                 selected.enabled = false;
        //                 itemImage.enabled = false;
        //                 InventoryScript.instance.SetHoldingCondition(false);
        //                 Debug.Log("[Hotbar] Deselected held item.");
        //             }
        //             else
        //             {
        //                 player.heldItem = slot;
        //                 hotbarItem = index;
        //                 selected.enabled = true;
        //                 itemImage.enabled = true;
        //                 InventoryScript.instance.SetHoldingCondition(true);
        //                 Debug.Log($"[Hotbar] Switched to: {player.heldItem.item.name}");
        //             }

        //             InventoryScript.instance.SetSelectedIndex(index);
        //         }
        //     }


    }

    private void CheckForHotbarInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemHeld = 0;
            UpdateSelectedItem(itemHeld);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemHeld = 1;
            UpdateSelectedItem(itemHeld);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            itemHeld = 2;
            UpdateSelectedItem(itemHeld);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            itemHeld = 3;
            UpdateSelectedItem(itemHeld);
        }
        // if (player.heldItem.item == null || player.heldItem.quantity <= 0)
        //     return;
        // else
        //     Debug.Log("Held item: " + player.heldItem.item.name + " x" + player.heldItem.quantity);

    }

    public void UpdateSelectedItem(int itemHeld)
    {
        if (inventory.GetItemUsingIndex(itemHeld).item != null && inventory.GetItemUsingIndex(itemHeld).quantity > 0)
        {
            heldItem = inventory.GetItemUsingIndex(itemHeld);
            Debug.Log("Held item: " + heldItem.item.name + " x" + heldItem.quantity);
            holdingItemManager.SetSelectedImage(heldItem);
        }
        else
        {
            Debug.Log("Selected null spot.");
        }
    }

    public void FixedUpdate()
    {
        movement.MoveWithForce(movement.moveForce);
        spriteRenderer.flipX = movement.flip;

    }

    public override void InventorySetup()
    {
        for (int i = 0; i < 49; i++)
        {
            Debug.Log(i);
            Debug.Log(inventory.items.Count);
            inventory.AddToList(new Item(null,1));
        }
            Item paket = new Item(itemPaketTest, 67);
            inventory.AddItem(paket);
            Debug.Log("Player inventory setup.");
            //player.inventory = player.inventory.SwapItem(Item item, Item item2);
            // comment
        }

    

    /** Interaction handler **/
        // I LOVE HAMBURGERS
        void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object") && other.GetComponent<ItemInWorld>().takeable)
        {
            recievable = true;
            takeableItem = other.GetComponent<ItemInWorld>();
        }


        // else if (other.CompareTag("NPC"))
        // {
        //     if (other.GetComponent<NPC>().canTalkTo)
        //     {
        //         if (Input.GetKeyDown(KeyCode.E))
        //         {
        //             other.GetComponent<NPC>().StartDialogue();
        //         }
        //     }       
        // } for later!!!! (KEEP)
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object") && other.GetComponent<ItemInWorld>().takeable)
        {
            recievable = false;
            takeableItem = null;
        }
    }

    public Item GetHeldItem()
    {
        return heldItem;
    }
}
// Only update player.heldItem if index is va
