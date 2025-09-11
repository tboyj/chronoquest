using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public List<RectTransform> panels = new List<RectTransform>();
    public Transform inventoriesRoot;
    public Transform hotbarTransform;
    public int inventorySize = 48;
    [Tooltip("DEVELOPER USE ONLY - Item to be assigned to player inventory on game start. For testing purposes.")]
    public ItemStorable itemTemporary;


    public void AddItem(InventoryItem item)
    {
        if (inventory.Contains(item))
        {
            if (item.item.stackable)
            {
                if (item.quantity < item.item.maxStackSize)
                {
                    item.quantity += 1;
                }
                else
                {
                    Debug.Log("Item stack full");
                    inventory.Add(item);
                }
            }
        }
        else
        {
            int ic = inventory.Count;
            while (ic < inventory.Count)
            {
                if (inventory[ic].item == null)
                {
                    inventory[ic] = item;
                    ic = inventory.Count;
                    return;

                }
                ic++;
            }

        }
    }
    public InventoryItem GetItemFromIndex(int index)
    {
        return inventory[index];
    }
    public int returnInventorySize()
    {
        return inventory.Count;
    }
    public void RemoveItem(InventoryItem item)
    {
        inventory.Remove(item);
    }

    void Awake()
{
    instance = this;
    // Fill inventory with empty slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(new InventoryItem());
        }

    // assign a starter item
    inventory[0] = new InventoryItem(itemTemporary, 67);
}

    // Update is called once per frame
    void Update()
    {
        
            for (int i = 0; i < inventory.Count; i++)
            {
            Debug.Log(inventory[i].item.name + " x" + inventory[i].quantity+"\nSlot: "+i);
            }


    }

    // void ScanInventory()
    // {

    // }
}

