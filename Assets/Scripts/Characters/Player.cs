using System;
using System.Collections.Generic;
using ChronoQuest.Interactions.World;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
public class Player : Character, Interaction
{
    // protected Character player;
    // Item Handling
    public ItemStorable itemPaketTest;         // Reference to a storable item package (test or template)
    protected Item heldItem;                   // Currently held item by the player or NPC
    public GameObject reciever;         // Component that receives item usage (e.g., doors, NPCs)

    // Interaction Targets
    protected ItemInWorld interactableItem;    // Item in the world that can be interacted with or picked up
    protected NPC interactableNPC;             // NPC that can be interacted with
    public bool isUsingItem;
    // Inventory UI
    protected InventoryGUI guiHandler;         // Inventory GUI logic handler
    public RectTransform hotbarHolder;         // UI container for hotbar slots
    public RectTransform inventoryHolder;      // UI container for inventory slots
    private int indexOfInventoryHover { get; set; } // Index of currently hovered inventory slot

    // Quest & Dialog Systems
    private QuestManager manager;              // Manages active and completed quests
    public GameObject questPanelContainer;     // UI container for displaying quest panels
    public GameObject containerHiddenDuringDialog; // UI elements hidden during dialog sequences
    public GameObject dialogPanel;             // Dialog UI panel
    public DialogGUIManager dialogManager;     // Handles dialog GUI logic and flow

    private bool inventoryInteractionPermission;

    public void Start()
    {
        manager = gameObject.GetComponent<QuestManager>();
        Initialize("Player", gameObject.GetComponent<Inventory>(), base.spriteRenderer, null, 0, gameObject.GetComponent<HoldingItemScript>(), false, false, transform.GetChild(0).GetComponent<Animator>());
        inventoryInteractionPermission = true;
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
        dialogManager = dialogPanel.GetComponent<DialogGUIManager>();
        //!!! --- ! Inventory GUI Section ! --- !!!//
        InventoryGuiRefresh();
    }
    public void Update()
    {
        inventory.SetRefresh(true);
        if (heldItem.quantity <= 0 || !isHolding)
        {
            holdingItemManager.Activate(false);
             // Hide the sprite when quantity is 0
        }
        else
        {
            holdingItemManager.Activate(true); // Show the sprite when quantity is greater than 0
        }
        if (manager.questsAssigned.Count == 0)
        {
            questPanelContainer.SetActive(false);
        }
        else
        {
            questPanelContainer.SetActive(true);
        }
        CheckKeyInputInteraction();
        if (!manager.CurrentlyInDialog())
        {
            if (gameObject.GetComponent<PauseScript>().isPaused == false)
            {
                CheckForHotbarInput();

                if (inventory.GetRefresh() == true)
                {
                    InventoryGuiRefresh();
                }
            }
            inDialog = false;

            containerHiddenDuringDialog.SetActive(true);
            dialogPanel.SetActive(false);
        }
        else if (manager.CurrentlyInDialog())
            
        {
            containerHiddenDuringDialog.SetActive(false);
            inDialog = true;
            interactableNPC.inDialog = true;
            dialogPanel.SetActive(true);
        }
    }
    private void CheckKeyInputInteraction()
    {
        if (!manager.CurrentlyInDialog() && gameObject.GetComponent<PauseScript>().isPaused == false) {
            if (Input.GetKeyDown(Keybinds.actionKeybind))
            {
                AttemptInteraction();
                CheckForHotbarInput();
            }
            if (Input.GetKeyDown(Keybinds.useKeybind))
            {
                TryToUse();
                CheckForHotbarInput();
            }

            if (Input.GetKeyDown(Keybinds.giveKeybind))
            {
                TryDrop();
            }
        }
            if (Input.GetKeyDown(Keybinds.continueKeybind) && manager.CurrentlyInDialog() && gameObject.GetComponent<PauseScript>().isPaused == false)
            { // bummy boy code o-o
            if (interactableNPC != null)
            {
                Debug.Log(interactableNPC.GetComponent<QuestHandler>().GetMostRecentQuest());
                Debug.Log(manager.GetCurrentQuest());
                if (interactableNPC.GetComponent<QuestHandler>().GetMostRecentQuest() == manager.GetCurrentQuest())
                {
                    var currentQuest = manager.GetCurrentQuest();
                    if (manager.GetCurrentQuest().dialogsForQuest.Count > 1)
                    {
                        Debug.Log("Count is > 1.");
                        manager.GetCurrentQuest().DialogAdvance();
                        manager.SetCurrentlyInDialog(true);
                        interactableNPC.inDialog = true;
                        dialogManager.SetCharName(manager.GetCurrentQuest().dialogsForQuest[0].characterName);
                        dialogManager.SetDialText(manager.GetCurrentQuest().dialogsForQuest[0].dialogueText);
                    }

                    else if (manager.GetCurrentQuest().dialogsForQuest.Count == 1)
                    {
                        Debug.Log("Count is 1.");
                        currentQuest.ShowDialog(false);
                        manager.SetCurrentlyInDialog(false);
                        interactableNPC.inDialog = false;
                        currentQuest.DialogAdvance();
                        manager.ChangeFunction();
                    }
                    //manager.GetCurrentQuest().ShowDialog(true);
                }
                else
                {
                    Debug.Log("Wrong Character!!!");
                }
            }

            else
            {
                Debug.Log("No Character Found!!!");
            }
        }
    }

    // quest system will be one at a time. linear story. i cant produce a branching story narrative in 3 months :PPPPP
    private void AttemptInteraction()
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
        if (interactableNPC != null)
        {
            manager.TryToGiveQuest(interactableNPC, dialogManager);
        }
        // if not recognizing item to pick up, look for npc to interact with
    }
    private void TryToUse()
    {
        if (isHolding)
        {  
            if (reciever != null)
            {
                heldItem.item.useConnector.Apply(reciever);
            } else
            {
                heldItem.item.useConnector.ApplyAlone();
            }
        }
    }
    private void TryDropForItem() {
    if (heldItem.item != null)
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
        if (!gameObject.GetComponent<PauseScript>().isPaused && indexOfInventoryHover >= 0 && inventoryInteractionPermission)
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
        if (!manager.CurrentlyInDialog() && !isUsingItem)
        {
            movement.controller.enabled = true;
            animatorSetup.speed = 1;
            movement.MoveWithForce();
            animatorSetup.SetFloat("SpeedX", 1); // Add Z animation to this at a later time.// input.magnitude.
            spriteRenderer.flipX = movement.flip;
        }
        else
        {
            movement.controller.enabled = false;
            animatorSetup.speed = 0;
        }
    }

    public override void InventorySetup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Debug.Log(i);
            // Debug.Log(inventory.GetInventory().Count);
            inventory.AddToList(new Item(null, 1));
        }
        // Item paket = new Item(itemPaketTest, 67);
        // inventory.AddItem(paket); Good bye, my lover... Good bye, my paket... 
        Debug.Log("Player inventory setup.");
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
                    Item basicItem = inventory.GetItemUsingIndex(imageSetupIndex);
                    if (basicItem.item != null)
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
        if (other.CompareTag("Object"))
        {
            if (other.GetComponent<ItemInWorld>() != null)
            {
                if (other.GetComponent<ItemInWorld>().takeable)
                {
                    interactableItem = other.GetComponent<ItemInWorld>();
                    Debug.Log("Interactable item: " + interactableItem.itemInWorld.name);
                }
            }
        }
        else if (other.CompareTag("NPC") && other.GetComponent<NPC>())
        {

            interactableNPC = other.GetComponent<NPC>();

        }
        else if (other.CompareTag("Teleport"))
        {
            other.GetComponent<TeleportScript>()?.Teleport(gameObject);
        }

        reciever = other.gameObject;
        Debug.Log(reciever.name);
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Object"))
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
        reciever = null; 
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
    public void InteractionFunction()
    {
    
    }

    public void SetInventoryPermission(bool v)
    {
        inventoryInteractionPermission = v;
    }
    public bool GetInventoryPermission()
    {
        return inventoryInteractionPermission;
    }
}

