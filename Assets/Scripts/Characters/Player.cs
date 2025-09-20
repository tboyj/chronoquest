using System;
using TMPro;
using UnityEngine;
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
    [SerializeField]
    protected RectTransform hotbarHolder;
    [SerializeField]
    protected RectTransform inventoryHolder;
    private int indexOfInventoryHover { get; set; }

    public void Start()
    {

        Initialize("Player", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, 0, this.GetComponent<HoldingItemScript>(), false);
        movement = gameObject.AddComponent<PlayerMovement>();
        InventorySetup();
        guiHandler = gameObject.GetComponent<InventoryGUI>();
        heldItem = inventory.GetItemUsingIndex(itemHeld);
        if (heldItem.item != null)
            Debug.Log(heldItem.item.name);
        else
            Debug.Log("Item is null");
        holdingItemManager.spriteHolderImage.enabled = true;
        holdingItemManager.spriteHolderImage.sprite = heldItem.item.sprite;
        holdingItemManager.spriteTopLeftImage.enabled = true;
        holdingItemManager.spriteTopLeftImage.sprite = heldItem.item.sprite; // UI Image

        //!!! --- ! Inventory GUI Section ! --- !!!//
        InventoryGuiRefresh();
    }
    public void Update()
    {

        if (heldItem.quantity <= 0 || !isHolding)
        {
            holdingItemManager.spriteHolderImage.enabled = false;
            holdingItemManager.spriteTopLeftImage.enabled = false; // Hide the sprite when quantity is 0
        }
        else
        {
            holdingItemManager.spriteHolderImage.enabled = true;
            holdingItemManager.spriteTopLeftImage.enabled = true; // Show the sprite when quantity is greater than 0
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
            TryPickupitem();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryDropItem();
        }
    }

    private void TryPickupitem()
    {
        if (interactableItem != null)
        {
            if (interactableItem.takeable && interactableItem.amountOfItemsHere > 0)
            {
                Item itemAdded = new Item(interactableItem.itemInWorld, 1);
                inventory.AddItem(itemAdded);
                Debug.Log("");
                inventory.SetRefresh(true);
            }
        }
    }

    private void TryDropItem()
    {
        if (heldItem.quantity > 0 && heldItem.item.canBeGiven == true)
        {
            if (interactableItem.itemInWorld == heldItem.item)
            {
                if (interactableItem.amountOfItemsHere > 0)
                {
                    inventory.RemoveOneQuantity(itemHeld);
                    inventory.SetRefresh(true);
                    interactableItem.amountOfItemsHere++;
                }
            }
            if (interactableNPC != null)
            {
                // Do later
            }

        }
    }

    private void CheckForHotbarInput()
    {
        if (!gameObject.GetComponent<PauseScript>().isPaused)
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
        if (updateIndex == itemHeld)
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
            holdingItemManager.spriteHolderImage.sprite = heldItem.item.sprite;
            holdingItemManager.spriteTopLeftImage.sprite = heldItem.item.sprite; // UI Image
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
        movement.MoveWithForce(movement.moveForce);
        spriteRenderer.flipX = movement.flip;

    }

    public override void InventorySetup()
    {
        for (int i = 0; i < 49; i++)
        {
            Debug.Log(i);
            Debug.Log(inventory.GetInventory().Count);
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



        // else if (other.CompareTag("NPC"))
        // {
        //     if (other.GetComponent<NPC>().canTalkTo)
        //     {
        //         if (Input.GetKeyDown(KeyCode.E))
        //         {
        //             other.GetComponent<NPC>().StartDialogue();
        //         }
        //     }       
        // } for later!!!! (KEEP)
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Object") && other.GetComponent<ItemInWorld>().takeable)
        {
            Debug.Log("Interactable item: (false)" + interactableItem.itemInWorld.name);
            interactableItem = null;

        }
        else if (other.CompareTag("NPC") && other.GetComponent<NPC>())
        {
            Debug.Log("Interactable item: (false)" + interactableNPC.name);
            interactableNPC = null;

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

