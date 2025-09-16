using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : ItemHandler
{
    public ItemStorable itemWanted;
    public SpriteRenderer door;
    public BoxCollider doorPassageWay;
    public int quantityNeeded;
    public bool unlocked = false;
    public bool opened = false;
    void Start()
    {
        door = transform.Find("Object").GetComponent<SpriteRenderer>();
        doorPassageWay = transform.Find("Object").GetComponent<BoxCollider>();
    }
    protected override void doCheck()
{
    if (quantityNeeded <= 0 && !unlocked)
    {
        unlocked = true;
        openDoor();
        Debug.Log("Door unlocked.");
    }

    // Handle unlocking with item
    if (playerInTrigger && Input.GetKeyDown(KeyCode.Q) && !unlocked)
    {
        Debug.Log("Q Pressed");
        int index = InventoryScript.instance.GetSelectedIndex();
        Item temp = InventoryScript.instance.GetItemFromIndex(index);

        if (temp != null && index > -1 && temp.item.canBeGiven && InventoryScript.instance.holdingItem)
        {
            if (temp.item == itemWanted && quantityNeeded <= temp.quantity)
            {
                if (quantityNeeded > 0)
                {
                    quantityNeeded--;
                    items.Add(temp);
                    InventoryScript.instance.RemoveItem(index);
                }
            }
        }
    }

    // Handle toggling door open/close after unlocked
    if (unlocked && playerInTrigger && Input.GetKeyDown(KeyCode.E))
    {
        if (opened)
            closeDoor();
        else
            openDoor();
    }
}
    // turns black (temp, will replace with open door texture)
    protected void openDoor()
    {
        door.color = new Color(0, 0, 0);
        opened = true;
        doorPassageWay.enabled = false;
    }
    protected void closeDoor()
    {
        door.color = new Color(255, 255, 255);
        opened = false;
        doorPassageWay.enabled = true;
    }
}
