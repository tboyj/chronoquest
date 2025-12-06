using System;
using ChronoQuest.UIForInteractions;
using UnityEngine;

public class NPC : Character, IAvailableActions
{

    // public Item itemGiven;
    public bool inRange;
    public Player player;
    public QuestHandler questHandler;
    public NPCMovement movement;
    [Header("NPC AI Systems")]
    [Range(0,100)]
    public int trust = 50;
    public void Start()
    {
        Initialize("NPC", gameObject.GetComponent<Inventory>(), base.gameObject, 0, this.GetComponent<HoldingItemScript>(), false, false, null);
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

        animatorSetup.SetFloat("SpeedX", Mathf.Clamp01(movement.GetAgent().velocity.magnitude));
        // facing direction based on where player is (if current quest is controlled by npc).
        if (movement.status == "IDLE") {
            if (player != null && player.GetQuestManager() != null) {
                if (questHandler?.GetMostRecentQuest()?.data.id == player?.GetQuestManager()?.GetCurrentQuest()?.data.id) {
                    if (player.transform.position.x < transform.position.x)
                    {
                       
                        holdingItemManager.spriteHolderImage.flipX=true;
                    } else
                    {

                        holdingItemManager.spriteHolderImage.flipX=false;
                    }
                }
                 else
                {
                    // Debug.Log("Not for me!");
                }
            } else
            {
                Debug.Log("Definitely not for me!");
            }
           
        }
        
        if (movement.status == "QUEST" || movement.status == "MOVING")
        {
            Vector3 destination = movement.GetAgent().destination;
            float directionX = destination.x - transform.position.x;

            if (Mathf.Abs(directionX) > 0.01f) // small threshold to avoid jitter
            {
                // spriteRenderer.flipX = directionX < 0; // flip if destination is left
            }
        }
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
            player = other.GetComponent<Player>();
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
            ChangeTheUI(""); // reset
            // player = null;
        }
    }


    public void ChangeTheUI(string str)
    {
        if (player != null)
            player.interactionPanel.text = str;
        
    }

    public void ChangeTheUI(Item item)
    {
        if (player != null)
            player.interactionPanel.text = item.item.itemName;
    }
}