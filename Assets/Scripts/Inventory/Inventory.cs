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
    {// if inventory is not full, add item
            int stackableIndex = -1;
int emptyIndex = -1;

for (int i = 0; i < items.Count; i++)
{
    Item itemInInventory = items[i];

    if (itemInInventory.item != null)
    {
        if (itemInInventory.item == thisItem.item &&
            itemInInventory.quantity < itemInInventory.item.maxStackSize)
        {
            stackableIndex = i;
            break; // Found a valid stackable slot, no need to keep searching
        }
    }
    else if (emptyIndex == -1)
    {
        emptyIndex = i; // Save first empty slot in case stacking isn't possible
    }
}

// 🧠 Now apply the logic
if (stackableIndex != -1)
{
    Debug.Log("Stacking onto slot " + stackableIndex);
    AddQuantity(thisItem, items[stackableIndex]);
    Debug.Log("Item " + stackableIndex + " name: " + items[stackableIndex].item.name);
    Debug.Log("New item " + stackableIndex + " quantity: " + items[stackableIndex].quantity);
}
else if (emptyIndex != -1)
{
    Debug.Log("Placing into empty slot " + emptyIndex);
    if (thisItem.quantity <= thisItem.item.maxStackSize)
    {
        items[emptyIndex] = thisItem;
    }
    else
    {
        Item stack = new Item(thisItem.item, thisItem.item.maxStackSize);
        items[emptyIndex] = stack;
        thisItem.quantity -= stack.quantity;
        AddItem(thisItem); // recursive call to place remainder
    }
}
else
{
    Debug.Log("Inventory full — cannot add item.");
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
