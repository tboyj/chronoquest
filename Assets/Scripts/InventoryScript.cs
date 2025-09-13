using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    public List<InventorySlot> inventory = new List<InventorySlot>();
    public List<RectTransform> panels = new List<RectTransform>();
    public Transform inventoriesRoot;
    public Transform hotbarTransform;
    public int inventorySize = 49;
    [Tooltip("DEVELOPER USE ONLY - Item to be assigned to player inventory on game start. For testing purposes.")]
    public ItemStorable itemTemporary;
    public ItemStorable itemTemporary2;


    public void AddItem(InventorySlot item)
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
    public InventorySlot GetItemFromIndex(int index)
    {
        return inventory[index];
    }
    public int returnInventorySize()
    {
        return inventory.Count;
    }
    public void RemoveItem(InventorySlot item)
    {
        inventory.Remove(item);
    }

    void Awake()
    {
        instance = this;
        // Fill inventory with empty slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(new InventorySlot());
        }

        // assign a starter item
        inventory[0] = new InventorySlot(itemTemporary, 67);
        inventory[1] = new InventorySlot(itemTemporary2, 67);
    }

    // Update is called once per frame
    void Update()
    {
        



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            Debug.Log("Dattebayo!");
            ItemStorable storable = other.GetComponent<ItemInWorld>().item;
            AddItem(new InventorySlot(storable, 1));
        }
    }

    // void ScanInventory()
    // {

    // }
}

