using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : Character
{

    // public Item itemGiven;
    

    public void Start()
    {
        Initialize("NPC", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, 0, this.GetComponent<HoldingItemScript>(), false, null);
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        Debug.Log(inventory.items.Count);
        InventorySetup(49);
    }
    public void Update()
    {


    }


    // public void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // Debug.Log("Welcoem");
    //         inRange = true;
    //         if (other.GetComponent<Player>().GetHeldItem().item != null && other.GetComponent<Player>().isHolding == true)
    //         {
    //             itemGiven = other.GetComponent<Player>().GetHeldItem(); // This causes it twan
    //             Debug.Log(itemGiven.item.name);
    //         }
    //         else
    //             itemGiven = null;
    //     }
    // }
    // public void OnTriggerStay(Collider other)
    // {

    //     if (other.CompareTag("Player"))
    //     {

    //         if (other.GetComponent<Player>().GetHeldItem().item != null && other.GetComponent<Player>().isHolding == true)
    //         {
    //             itemGiven = other.GetComponent<Player>().GetHeldItem(); // This causes it twan
    //             // Debug.Log(itemGiven.item.name);
    //         }
    //         else
    //             itemGiven = null;
    //     }
    // }
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("Goodbye");
    //         inRange = false;
    //         itemGiven = new Item();

    //     }
    // }
}