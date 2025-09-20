using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    // protected Character player;
    public ItemStorable itemPaketTest;
    public Item heldItem;
    private ItemInWorld takeableItem;
    public Transform parentOfInventory;
    public Transform parentOfHotbar;
    public InventoryGUI guiHandler;

    public void Start()
    {

        Initialize("Player", gameObject.AddComponent<Inventory>(), base.spriteRenderer, null, true, true, 0, this.GetComponent<HoldingItemScript>(), false);
        movement = gameObject.AddComponent<PlayerMovement>();
        InventorySetup();
        guiHandler = gameObject.AddComponent<InventoryGUI>();
        heldItem = inventory.GetItemUsingIndex(itemHeld);
        holdingItemManager.spriteHolderImage.enabled = true;
        holdingItemManager.spriteHolderImage.sprite = heldItem.item.sprite;
        holdingItemManager.spriteTopLeftImage.enabled = true;
        holdingItemManager.spriteTopLeftImage.sprite = heldItem.item.sprite; // UI Image
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
        if (recievable && Input.GetKeyDown(KeyCode.E))
        {
                if (takeableItem.takeable && takeableItem.amountOfItemsHere > 0)
                {
                    Item itemAdded = new Item(takeableItem.itemInWorld, 1);
                    inventory.AddItem(itemAdded);
                    Debug.Log("item added: " + takeableItem.itemInWorld.name + ",inv index: " + inventory.GetItemIndex(itemAdded));
                }
        }
        if (canGive && Input.GetKeyDown(KeyCode.Q))
        {
            if (isHolding && heldItem.quantity > 0)
            {
                inventory.GetItemUsingIndex(itemHeld).quantity--;
            }
        }
        CheckForHotbarInput();
        //     

        //             if (slot == null || slot.item == null)
        //             {
        //                 Debug.LogWarning($"[Hotbar] Ignored null item at index {index}");
        //                 return;
        //             }

        //             // Deselect if same item
        //             if (player.heldItem != null && slot.item == player.heldItem.item)
        //             {
        //                 player.heldItem = null;
        //                 hotbarItem = -1;
        //                 selected.enabled = false;
        //                 itemImage.enabled = false;
        //                 InventoryScript.instance.SetHoldingCondition(false);
        //                 Debug.Log("[Hotbar] Deselected held item.");
        //             }
        //             else
        //             {
        //                 player.heldItem = slot;
        //                 hotbarItem = index;
        //                 selected.enabled = true;
        //                 itemImage.enabled = true;
        //                 InventoryScript.instance.SetHoldingCondition(true);
        //                 Debug.Log($"[Hotbar] Switched to: {player.heldItem.item.name}");
        //             }

        //             InventoryScript.instance.SetSelectedIndex(index);
        //         }
        //     }


    }

    


    private void CheckForHotbarInput()
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
        // if (player.heldItem.item == null || player.heldItem.quantity <= 0)
        //     return;
        // else
        //     Debug.Log("Held item: " + player.heldItem.item.name + " x" + player.heldItem.quantity);

    }

    public void UpdateSelectedItem(int updateIndex)
    {
        Item candidate = inventory.GetItemUsingIndex(updateIndex);
        Debug.Log(candidate.item.name);
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
            Debug.Log(inventory.items.Count);
            inventory.AddToList(new Item(null,1));
        }
            Item paket = new Item(itemPaketTest, 67);
            inventory.AddItem(paket);
            Debug.Log("Player inventory setup.");
            //player.inventory = player.inventory.SwapItem(Item item, Item item2);
            // comment
        }

    

    /** Interaction handler **/
        // I LOVE HAMBURGERS
        void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object") && other.GetComponent<ItemInWorld>().takeable)
        {
            recievable = true;
            takeableItem = other.GetComponent<ItemInWorld>();
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
            recievable = false;
            takeableItem = null;
        }
    }

    public Item GetHeldItem()
    {
        
        return heldItem;
    }
}
// Only update player.heldItem if index is va
