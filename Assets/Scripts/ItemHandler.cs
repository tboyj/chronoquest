using System.Collections.Generic;
using UnityEngine;

public abstract class ItemHandler : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    protected bool playerInTrigger = false;

    void Update()
    {
        HandleInteraction();
    }

    protected virtual void HandleInteraction() // handles interaction
    {
        if (!playerInTrigger || !Input.GetKeyDown(KeyCode.Q)) return;

        int index = InventoryScript.instance.GetSelectedIndex();
        Item item = InventoryScript.instance.GetItemFromIndex(index);

        if (IsValidItem(item, index))
        {
            OnItemReceived(item, index);
        }
    }

    protected virtual bool IsValidItem(Item item, int index)
    {
        return item != null &&
               index > -1 &&
               item.item.canBeGiven &&
               InventoryScript.instance.holdingItem;
    }

    protected virtual void OnItemReceived(Item item, int index)
    {
        items.Add(item);
        InventoryScript.instance.RemoveItem(index);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }
}