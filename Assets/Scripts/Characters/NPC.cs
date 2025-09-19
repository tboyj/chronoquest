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
        npc = Initialize("NPC", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, false, true ,0, this.GetComponent<HoldingItemScript>());
        npc.movement = npc.GetComponent<Movement>();

    }
    public void Update()
    {
        
        if (inRange && recievable && Input.GetKeyDown(KeyCode.Q))
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
            if (other.GetComponent<Player>().GetHeldItem().item != null)
                itemGiven = other.GetComponent<Player>().GetHeldItem();
        }
    }
    public void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("How often do you happen to be here?");
            if (other.GetComponent<Player>().GetHeldItem().item != null)
                itemGiven = other.GetComponent<Player>().GetHeldItem();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Goodbye");
            inRange = false;
            itemGiven = null;
        }
    }
}