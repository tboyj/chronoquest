using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character
{

    // public Item itemGiven;
    public bool inRange;
    public Inventory playerInventory;
    public QuestHandler questHandler;
    public NPCMovement movement;
    [Header("NPC AI Systems")]
    [Range(0,100)]
    public int trust = 50;
    public void Start()
    {
        Initialize("NPC", gameObject.GetComponent<Inventory>(), base.spriteRenderer, 0, this.GetComponent<HoldingItemScript>(), false, false, null);
        movement = gameObject.GetComponent<NPCMovement>();
        inventory = GetComponent<Inventory>();
        if (gameObject.GetComponent<QuestHandler>() != null)
        {
            questHandler = gameObject.GetComponent<QuestHandler>();
        }
        else
        {
            questHandler = gameObject.AddComponent<QuestHandler>();
            Debug.Log("Added default quest handler. Likely doesn't have any quests.");
        }
        InventorySetup(49);

    }
    public override void InventorySetup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            inventory.AddToList(new Item(null, 1));
        }
        // Item paket = new Item(itemPaketTest, 67);
        // inventory.AddItem(paket); Good bye, my lover... Good bye, my paket... 
        Debug.Log($"NPC {gameObject.name} inventory setup.");
    }
    public void Update()
    {

        if (inDialog)
        {
            movement.enabled = false;
            movement.transform.position = movement.transform.position;
        }
        else
        {
            movement.enabled = true;
        }

    }
    public void FixedUpdate()
    {
        
    }

    public bool GetInRange()
    {
        return inRange;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("[TRIGGER EVENT / NPC] Trigger entered by: " + other.name + " Tag: " + other.tag);
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<Inventory>();
            inRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("[TRIGGER EVENT / NPC] Trigger exited by: " + other.name + " Tag: " + other.tag);
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            inRange = false;
            playerInventory = null;
        }
    }
}