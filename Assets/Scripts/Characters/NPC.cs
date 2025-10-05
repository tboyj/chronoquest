using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character
{

    // public Item itemGiven;
    private Inventory playerInventory;
    private QuestHandler questHandler;

    public void Start()
    {
        Initialize("NPC", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, 0, this.GetComponent<HoldingItemScript>(), false, null);
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
        if (inventory.items.Contains(questHandler.GetRequiredItemOfMostRecentQuest()))
        {
            Debug.Log("NPC has item");
        }
        
        
    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Welcoem");
            playerInventory = other.GetComponent<Inventory>();

        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<Inventory>();
        }
    }
}