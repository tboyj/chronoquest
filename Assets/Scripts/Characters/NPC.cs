using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public bool inRange = false;
    public Item itemGiven;
    protected Character npc;
    
    public void Start()
    {
        npc = Initialize("NPC", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, 0, this.GetComponent<HoldingItemScript>(), false, null);
        npc.movement = npc.GetComponent<Movement>();

    }
    public void Update()
    {

        if (inRange && Input.GetKeyDown(KeyCode.Q))
        {
            if (itemGiven.item.canBeGiven == true)
                npc.inventory.AddItem(itemGiven);
        }
    }
    // protected override void Initialize(string name, Inventory inv, SpriteRenderer spriteR)
    // {
    //     Name = name;
    //     inventory = inv;
    //     spriteRenderer = spriteR;
    // }
    public override void InventorySetup()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Welcoem");
            inRange = true;
            if (other.GetComponent<Player>().GetHeldItem().item != null && other.GetComponent<Player>().isHolding == true)
            {
                itemGiven = other.GetComponent<Player>().GetHeldItem(); // This causes it twan
                Debug.Log(itemGiven.item.name);
            }
            else
                itemGiven = null;
        }
    }
    public void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if (other.GetComponent<Player>().GetHeldItem().item != null && other.GetComponent<Player>().isHolding == true)
            {
                itemGiven = other.GetComponent<Player>().GetHeldItem(); // This causes it twan
                // Debug.Log(itemGiven.item.name);
            }
            else
                itemGiven = null;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Goodbye");
            inRange = false;
            itemGiven = new Item();

        }
    }
}