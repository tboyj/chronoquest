using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
public class Player : Character
{
    // protected Character player;
    public ItemStorable itemPaketTest;
    protected Item heldItem;
    protected ItemInWorld interactableItem;
    protected NPC interactableNPC;
    protected InventoryGUI guiHandler;

    public RectTransform hotbarHolder;

    public RectTransform inventoryHolder;
    private int indexOfInventoryHover { get; set; }
    private QuestManager manager;
    public void Start()
    {
        manager = gameObject.GetComponent<QuestManager>();
        Initialize("Player", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, 0, this.GetComponent<HoldingItemScript>(), false, transform.GetChild(0).GetComponent<Animator>());
        movement = gameObject.AddComponent<PlayerMovement>();
        InventorySetup(49);
        guiHandler = gameObject.GetComponent<InventoryGUI>();
        heldItem = inventory.GetItemUsingIndex(itemHeld);
        if (heldItem.item != null)
            Debug.Log(heldItem.item.name);
        else
            Debug.Log("Item is null");
        holdingItemManager.EnableWithSprite(heldItem.item.sprite);
 // UI Image

        //!!! --- ! Inventory GUI Section ! --- !!!//
        InventoryGuiRefresh();
    }
    public void Update()
    {

        if (heldItem.quantity <= 0 || !isHolding)
        {
            holdingItemManager.Activate(false);
             // Hide the sprite when quantity is 0
        }
        else
        {
            holdingItemManager.Activate(true); // Show the sprite when quantity is greater than 0
        }

        if (manager.questsAssigned.Count > 0)
        {
            if (manager.questsAssigned[0].IsCompleted && manager.questsAssigned[0].todo.Count == 1)
            {
                Debug.Log("Completed: True.");
                if (manager.questsAssigned[0].todo.Count > 1)
                    gameObject.GetComponent<QuestManagerGUI>().GotoNextTodo();
            }
        }
        CheckKeyInputInteraction();
        CheckForHotbarInput();
        if (inventory.GetRefresh() == true)
        {
            InventoryGuiRefresh();
        }
    }

    private void CheckKeyInputInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryUseEkey();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryDrop();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Dialog();
            if (manager.questsAssigned.Count == 0)
            {
                TryToGiveQuest();
            }
            else
            {
                Debug.Log("You have a quest underway");
            }
        }
    }

    private void Dialog()
    {
        throw new NotImplementedException();
    }

    private void TryToGiveQuest()
    {
        if (interactableNPC != null)
        {
            QuestHandler npcQuestHandler = interactableNPC.GetComponent<QuestHandler>();
            QuestInstance questAssigned = npcQuestHandler.GetMostRecentQuest();
            Debug.Log(questAssigned.IsCompleted);
            if (questAssigned != null)
            {
                if (manager.questsAssigned.Contains(questAssigned) && !manager.questsCompleted.Contains(questAssigned)) // make sure he doesn't have it already;
                { // quest is assigned but not done.
                    Debug.Log("Not complete.");
                }
                else if (!manager.questsAssigned.Contains(questAssigned) && !manager.questsCompleted.Contains(questAssigned))
                { // add since there is none in quest.
                    manager.AddQuestToList(questAssigned);
                }
                else
                { // quest is done.
                    
                }
            }
        }
    }
// quest system will be one at a time. linear story. i cant produce a branching story narrative in 3 months :PPPPP
    private void TryUseEkey()
    {
        if (interactableItem != null) // picking up items
        {
            if (interactableItem.takeable && interactableItem.amountOfItemsHere > 0)
            {
                Item itemAdded = new Item(interactableItem.itemInWorld, 1);
                inventory.AddItem(itemAdded);
                
                
                inventory.SetRefresh(true);
            }
        }
        
        // if not recognizing item to pick up, look for npc to interact with
    }
    private void TryDropForItem() {

    if (interactableItem.itemInWorld == heldItem.item)
        {
            if (interactableItem.amountOfItemsHere > 0)
            {
                inventory.RemoveOneQuantity(itemHeld);
                inventory.SetRefresh(true);
                interactableItem.amountOfItemsHere++;
            }
        }
    }
    private void TryDrop()
    {
        if (heldItem.quantity > 0 && heldItem.item.canBeGiven == true)
        {
            if (interactableItem != null)
            {
                TryDropForItem();
            }
            else
            {
                Debug.Log("No item found");
            }
        }
    }


    private void CheckForHotbarInput()
{
    if (!gameObject.GetComponent<PauseScript>().isPaused && indexOfInventoryHover >= 0)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateSelectedItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateSelectedItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateSelectedItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UpdateSelectedItem(3);
        }
    }

}

    public void UpdateSelectedItem(int updateIndex)
    {
        Item candidate = inventory.GetItemUsingIndex(updateIndex);
        if (candidate.item != null)
            Debug.Log(candidate.item.name);
        else
            Debug.Log("null item selected");
        bool isValidCandidate = candidate != null && candidate.item != null && candidate.quantity > 0;

        // If selecting the same index again, toggle holding state
        if (updateIndex == itemHeld && candidate.item != null && candidate.item == heldItem.item)
        {

            isHolding = !isHolding;
            Debug.Log($"Toggled holding state: {isHolding}");
        }
        // If selecting a new valid item
        else if (isValidCandidate)
        {
            heldItem = candidate;
            itemHeld = updateIndex;
            isHolding = true;
            holdingItemManager.SetSprite(heldItem.item.sprite);
 // UI Image
            Debug.Log($"Held item: {heldItem.item.name} x{heldItem.quantity}");
        }
        // If selecting an invalid or empty slot
        else
        {
            // heldItem = null;
            // itemHeld = -1;
            isHolding = false;
            Debug.Log("Selected null or empty slot.");
        }
        // Optional: refresh visuals here

    }

    public void FixedUpdate()
    {
        movement.MoveWithForce();
        animatorSetup.SetFloat("SpeedX", Math.Abs(movement.rb.velocity.x)); // Add Z animation to this at a later time.
        spriteRenderer.flipX = movement.flip;

    }

    public override void InventorySetup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Debug.Log(i);
            // Debug.Log(inventory.GetInventory().Count);
            inventory.AddToList(new Item(null, 1));
        }
        Item paket = new Item(itemPaketTest, 67);
        inventory.AddItem(paket);
        Debug.Log("Player inventory setup.");
        //player.inventory = player.inventory.SwapItem(Item item, Item item2);
        // comment
    }

    public void InventoryGuiRefresh()
    {
        int imageSetupIndex = 0;

        // -- ! Hotbar section ! -- //
        foreach (RectTransform slot in hotbarHolder)
        {
            foreach (RectTransform imageContainer in slot)
            {
                if (imageContainer.name.Equals("Holder"))
                {
                    // Debug.Log("Got to image detector");
                    Image imageOfItem = imageContainer.GetComponent<Image>();
                    TextMeshProUGUI text = imageContainer.GetChild(0).GetComponent<TextMeshProUGUI>();
                    if (inventory.GetItemUsingIndex(imageSetupIndex).item != null && inventory.GetItemUsingIndex(imageSetupIndex).quantity > 0)
                    {
                        imageOfItem.sprite = inventory.GetItemUsingIndex(imageSetupIndex).item.sprite;
                        imageOfItem.enabled = true;
                        text.text = "x" + inventory.GetItemUsingIndex(imageSetupIndex).quantity.ToString();
                        text.enabled = true;
                    }
                    else
                    {
                        imageOfItem.enabled = false;
                        text.enabled = false;
                    }
                    // Debug.Log(imageSetupIndex + "out of " + inventory.GetInventory().Count);
                    imageSetupIndex++;
                }


            }
        }

        // --- ! Inventory section ! --- //

        foreach (RectTransform slot in inventoryHolder)
        {
            foreach (RectTransform imageContainer in slot)
            {
                if (imageContainer.name.Equals("Holder"))
                {
                    // Debug.Log("Got to image detector");
                    Image imageOfItem = imageContainer.GetComponent<Image>();
                    TextMeshProUGUI text = imageContainer.GetChild(0).GetComponent<TextMeshProUGUI>();
                    if (inventory.GetItemUsingIndex(imageSetupIndex).item != null && inventory.GetItemUsingIndex(imageSetupIndex).quantity > 0)
                    {
                        imageOfItem.sprite = inventory.GetItemUsingIndex(imageSetupIndex).item.sprite;
                        imageOfItem.enabled = true;
                        text.text = "x" + inventory.GetItemUsingIndex(imageSetupIndex).quantity.ToString();
                        text.enabled = true;
                    }
                    else
                    {
                        imageOfItem.enabled = false;
                        text.enabled = false;
                    }
                    // Debug.Log(imageSetupIndex + "out of " + inventory.GetInventory().Count);
                    imageSetupIndex++;
                }
            }
        }
        inventory.SetRefresh(false);
    }

    /** Interaction handler **/
    // I LOVE HAMBURGERS
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Object") && other.GetComponent<ItemInWorld>().takeable)
        {


            interactableItem = other.GetComponent<ItemInWorld>();
            Debug.Log("Interactable item: " + interactableItem.itemInWorld.name);
        }
        else if (other.CompareTag("NPC") && other.GetComponent<NPC>())
        {

            interactableNPC = other.GetComponent<NPC>();

        }
        else if (other.CompareTag("Teleport"))
        {
            this.transform.position = other.GetComponent<TeleportScript>().teleportToPosition;
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Object") && other.GetComponent<ItemInWorld>().takeable)
        {
            if (interactableItem != null)
            {
                Debug.Log("Interactable item: (false)" + interactableItem.itemInWorld.name);
                interactableItem = null;
            }

        }
        if (other.CompareTag("NPC") && other.GetComponent<NPC>())
        {
            if (interactableNPC != null)
            {
                Debug.Log("Interactable item: (false)" + interactableNPC.name);
                interactableNPC = null;
            }
        }
    }

    public Item GetHeldItem()
    {

        return heldItem;
    }

    public void SetIndexOfInventoryHover(int slotIndex)
    {
        indexOfInventoryHover = slotIndex;
    }
    public int GetIndexOfInventoryHover()
    {
        return indexOfInventoryHover;
    }
}

