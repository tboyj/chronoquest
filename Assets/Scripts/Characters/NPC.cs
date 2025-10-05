using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character
{

    // public Item itemGiven;
    private bool inRange = false;
    private Inventory playerInventory;
    private QuestHandler questHandler;

    public void Start()
    {
        Initialize("NPC", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, 0, this.GetComponent<HoldingItemScript>(), false, false, null);
        movement = gameObject.AddComponent<NPCMovement>();
        inventory = GetComponent<Inventory>();
        Debug.Log(inventory.items.Count);
        if (gameObject.GetComponent<QuestHandler>() != null)
        {
            questHandler = gameObject.GetComponent<QuestHandler>();
        }
        else
        {
            questHandler = gameObject.AddComponent<QuestHandler>();
        }
        InventorySetup(49);

    }
    public void Update()
    {
        
        if (inDialog)
        {
            movement.enabled = false;
            movement.rb.position = movement.rb.position;
        }
        else
        {
            movement.enabled = true;
        }

    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Welcoem");
            playerInventory = other.GetComponent<Inventory>();
            inRange = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<Inventory>();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            playerInventory = null;
        }
    }
}