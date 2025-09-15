using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;
    public bool holdingItem = true;
    public List<Item> inventory = new List<Item>();
    // public List<RectTransform> panels = new List<RectTransform>();
    public Transform inventoriesRoot;
    public Transform hotbarTransform;
    private int inventorySize = 49;
    [Tooltip("DEVELOPER USE ONLY - Item to be assigned to player inventory on game start. For testing purposes.")]
    public ItemStorable itemTemporary;
    public ItemStorable itemTemporary2;
    private bool objectInTrigger = false;
    private ItemInWorld currentStorable;
    private Collider currentCollision;

    private int indexOfSelected = 0;
    public void AddItem(Item addingItem)
    {
        Item existingItem = inventory.Find(i => i.item == addingItem.item && i.item.stackable);
        if (existingItem != null)
        {
            if (addingItem.item.stackable)
            {
                if (addingItem.quantity + existingItem.quantity < addingItem.item.maxStackSize) // can be expanded into more robust system (later)
                {
                    existingItem.quantity += addingItem.quantity;
                    Debug.Log("Quantity of comparison: " + existingItem.quantity);
                    Debug.Log("Quantity of addingItem: " + addingItem.quantity);
                }
                else
                { // can be expanded on later, would require algorithm (forloops :P)
                    Debug.Log("stack full, attempting into next available");
                    AssignSlot(addingItem);
                }
            }
            else
            {

                AssignSlot(addingItem);
            }
        }
        else
        {
            AssignSlot(addingItem);
        }
        RefreshAllGUISlots();
    }
    private void AssignSlot(Item addingItem)
    {
        int i = 0;
        while (i < inventory.Count)
        {
            if (inventory[i].item == null)
            {
                inventory[i] = addingItem;
                break;
            }
            i++;
        }
        Debug.Log("i at the end (index): " + i);

    }
    public Item GetItemFromIndex(int index)
    {
        return inventory[index];
    }
    public void SetSelectedIndex(int index)
    {
        indexOfSelected = index;
    }
    public int GetSelectedIndex()
    {
        return indexOfSelected;
    }
    public bool GetHoldingCondition()
    {
        return holdingItem;
    }
    public void SetHoldingCondition(bool setter)
    {
        holdingItem = setter;
    }
    public int ReturnInventorySize()
    {
        return inventory.Count;
    }
    public void RemoveItem(int index)
    {
        Item item = GetItemFromIndex(index);
        if (item.quantity > 1)
        {
            item.quantity--;
        }
        else
        {
            inventory.Remove(item);
            inventory.Add(new Item());
        }
        RefreshAllGUISlots();
    }

    void Awake()
    {
        instance = this;
        // Fill inventory with empty slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(new Item());
        }

        // assign a starter item

        AddItem(new Item(itemTemporary2, 1)); // jim's magnifying glass
    }
    void Start()
    {
        Debug.Log("Size: " + inventory.Count);
    }

    // Update is called once per frame
    private void Update()
    {
        if (objectInTrigger && Input.GetKeyDown(KeyCode.E) && currentStorable != null && currentStorable.takeable)
        {
            if (currentStorable.amountOfItemsHere > 0)
                AddItem(currentStorable.ReturnItemForInventory());
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            currentStorable = other.GetComponent<ItemInWorld>();
            objectInTrigger = true;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            objectInTrigger = false;
            currentStorable = null;
        }
    }
    public void RefreshAllGUISlots()
    {
        ItemGUI[] allGUIs = FindObjectsOfType<ItemGUI>();
        foreach (ItemGUI gui in allGUIs)
        {
            gui.RefreshFromInventory();
        }
    }
    public void checkIfIGiveSomething()
    {
        if (objectInTrigger && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Pressed");
            int index = GetSelectedIndex();
            Item temp = GetItemFromIndex(index);

            if (temp != null && index > -1 && temp.item.canBeGiven && holdingItem)
            {
                currentCollision.GetComponent<InventoryScript>().AddItem(temp);
                RemoveItem(index);
            }
        }
    }


    // void ScanInventory()
    // {

    // }
}

