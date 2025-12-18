using System;
using ChronoQuest.UIForInteractions;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character, IAvailableActions
{

    // public Item itemGiven;
    public bool inRange;
    public Player player;
    public QuestHandler questHandler;
    public NPCMovement movement;
    NavMeshAgent agent;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Initialize("NPC", gameObject.GetComponent<Inventory>(), base.gameObject, 0, 
        this.GetComponent<HoldingItemScript>(), false, false, null);
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
    private bool IsGrounded()
    {
        
        
        // Cast a ray slightly below the agent
        float rayDistance = agent.height / 2 + 0.2f;
        return Physics.Raycast(transform.position, Vector3.down, rayDistance);
    }
    public void FixedUpdate()
    {

        animatorSetup.SetFloat("SpeedX", Mathf.Clamp01(movement.GetAgent().velocity.magnitude));
        animatorSetup.SetBool("Grounded", IsGrounded());
        // facing direction based on where player is (if current quest is controlled by npc).
        if (movement.status == "IDLE") {
            if (player != null && player.GetQuestManager() != null) {
                if (questHandler?.GetMostRecentQuest()?.data.id == player?.GetQuestManager()?.GetCurrentQuest()?.data.id) {
                    
                }
                 else
                {
                    // Debug.Log("Not for me!");
                }
            } else
            {
                // Debug.Log("Definitely not for me!");
            }
           
        }
        
        if (movement.status == "QUEST" || movement.status == "MOVING")
        {
            NavMeshAgent navAgent = movement.GetAgent();
            
            if (navAgent.velocity.sqrMagnitude > 0.1f)
            {
                // Get the movement direction
                Vector3 direction = navAgent.velocity.normalized;
                direction.y = 0f; // Keep on horizontal plane
                
                // Find the metarig (adjust the path to match your NPC's hierarchy)
                Transform metarig = transform.Find("NPCsprite").Find("metarig");
                
                if (metarig != null)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    
                    // Apply the same offset as the player
                    metarig.rotation = targetRotation;
                    metarig.rotation = Quaternion.Euler(-90f, metarig.rotation.eulerAngles.y, metarig.rotation.eulerAngles.z);
                }
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
            player = null;

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