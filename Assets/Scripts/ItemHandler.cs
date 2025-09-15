using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemHandler : MonoBehaviour
{
    // This script lets the player give the ItemHandler an item. Will be expanded into doors, interaction checks, etc.
    public List<Item> items = new List<Item>();
    protected bool playerInTrigger = false;

    void Update()
    {
        doCheck();
    }
    protected virtual void doCheck()
    {
                if (playerInTrigger && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Pressed");
            int index = InventoryScript.instance.GetSelectedIndex();
            Item temp = InventoryScript.instance.GetItemFromIndex(index);

            if (temp != null && index > -1 && temp.item.canBeGiven && InventoryScript.instance.holdingItem)
            {
                items.Add(temp);
                InventoryScript.instance.RemoveItem(index);
            }
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

}
