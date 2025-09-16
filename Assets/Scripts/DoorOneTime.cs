using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorOneTime : ItemHandler
{
    public ItemStorable itemWanted;
    public SpriteRenderer door;
    public BoxCollider doorPassageWay;
    public int quantityNeeded;
    public bool opened = false;
    void Start()
    {
        door = transform.Find("Object").GetComponent<SpriteRenderer>();
        doorPassageWay = transform.Find("Object").GetComponent<BoxCollider>();
    }
    protected override void doCheck()
    {

        if (quantityNeeded <= 0)
        {
            openDoor();
        }

        if (playerInTrigger && Input.GetKeyDown(KeyCode.Q))
            {

                Debug.Log("Q Pressed");
                int index = InventoryScript.instance.GetSelectedIndex();
                Item temp = InventoryScript.instance.GetItemFromIndex(index);
                // Keys adding to the list to check
                if (temp != null && index > -1 && temp.item.canBeGiven && InventoryScript.instance.holdingItem)
                {
                    if (temp.item == itemWanted && quantityNeeded <= temp.quantity)
                    {
                        if (quantityNeeded > 0)
                        {
                            quantityNeeded--;
                            InventoryScript.instance.GetItemFromIndex(index).quantity--;
                            items.Add(temp);
                        }
                    }


                }
            }
    }


    // turns black (temp, will replace with open door texture)
    protected void openDoor()
    {
        door.color = new Color(0, 0, 0);
        opened = true;
        doorPassageWay.enabled = false;
    }
}
