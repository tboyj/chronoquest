using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Fields
    public List<Item> items;
    public Item itemEquipped;
    private bool refresh = false;
    // Constructor
    public bool GetRefresh()
    {
        return refresh;

    }
    public void SetRefresh(bool refresh)
    {
        this.refresh = refresh;
    }
    public Inventory()
    {
        items = new List<Item>();
    }
    /**
We can add a expanding inventory later. Currently fixed size right now!!!!
    **/
    public void AddToList(Item thisItem)
    {
        items.Add(thisItem);
    }
    public void AddItem(Item thisItem)
    {
        { // if inventory is not full, add item
            foreach (Item itemInInventory in items) // check if item is already in inventory
            {
                int index = GetItemIndex(itemInInventory);
                Debug.Log("Index gathered: " + index);
                if (itemInInventory.item != null)
                {
                    Debug.Log("Trying to access non null item");
                    if (itemInInventory.item == thisItem.item &&
                    thisItem.quantity < thisItem.item.maxStackSize)
                    {
                        Debug.Log("Trying to add quantity");
                        AddQuantity(thisItem, items[index]);// if item is already in inventory, add to quantity
                        Debug.Log("Item "+index+" name: "+items[index].item.name);
                        Debug.Log("New item "+index+" quantity: "+items[index].quantity);
                        break;
                    }
                    else if (itemInInventory.item == thisItem.item &&
                    thisItem.quantity >= thisItem.item.maxStackSize)
                    {
                        Debug.Log("Slot is full!");

                    }
                }
                else if (itemInInventory.item == null)
                {
                    Debug.Log("Trying to access null item");
                    if (thisItem.quantity < thisItem.item.maxStackSize)
                    {
                        Debug.Log("Trying to add index " + index);
                        // if item is already in inventory, add to quantity
                        items[index] = thisItem;
                        Debug.Log("Added item to index " + index + "");
                        Debug.Log("Item " + index + " name: " + items[index].item.name);
                        Debug.Log("Item "+index+" quantity: "+items[index].quantity);
                    }
                    else

                    {
                        Debug.Log("Trying to add stack to index " + index);
                        Item stack = new Item(thisItem.item, thisItem.item.maxStackSize);
                        items[index] = stack;
                        thisItem.quantity -= stack.quantity;
                        AddItem(thisItem);
                    }
                    break;

                }
            }

        }
    }
    public Inventory SwapItem(Item item, Item item2)
    {
        int i1 = GetItemIndex(item);
        int i2 = GetItemIndex(item2);
        items[i1] = item2;
        items[i2] = item;
        return this;
    }
    public void AddQuantity(Item item, Item itemInInventory) // sets inventory item (used instead of AddItem)
    {
        while (item.quantity > 0 && itemInInventory.quantity < itemInInventory.item.maxStackSize)
        {
            itemInInventory.quantity++;
            item.quantity--;
        }
        if (item.quantity > 0)
        {
            AddItem(item);
        } // while quantity is greater than 0 and item quantity is less than itemInInventory.item.maxStackSize)
    }
    public Item GetItemUsingIndex(int index) // gets item using index
    {
        return items[index];
    }
    public Item GetItem(Item item) // gets item using item
    {
        return items.Find(x => x == item);
    }
    public int GetItemIndex(Item item) // gets item index using item //?
    {
        return items.IndexOf(item);
    }
    public int GetItemStorableIndex(ItemStorable item) // gets item index using item //?
    {
        return items.FindIndex(x => x.item == item);
    }

    public List<Item> GetInventory()
    {
        return items;
    }

    public void RemoveOneQuantity(int itemHeld)
    {
        if (items[itemHeld].quantity > 0)
        {
            items[itemHeld].quantity--;
        }
        else
        {
            items.RemoveAt(itemHeld);
            AddToList(new Item());
        }
    }
    // public int GetCapacity()
    // {
    //     return maxCapacity;
    // }

    /** Inventory Equip / Hotbar **/


}
