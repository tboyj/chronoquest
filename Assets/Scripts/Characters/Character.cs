using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour
{
    // Properties
    protected string Name;
    public Inventory inventory;
    public GameObject objRendered;
    // public Movement movement;
    protected Item itemTest;
    public bool inDialog { get; set; }
    public int itemHeld = 0;
    public bool isHolding = true;
    public HoldingItemScript holdingItemManager;
    public Animator animatorSetup;
    // Constructor
    protected virtual Character Initialize(string name, Inventory inv, GameObject objRendered, int itemHeld, 
    HoldingItemScript holdingItemScript, bool isHolding, bool inDialog, Animator animatorSetup)
    {
        Name = name;
        inventory = inv;
        this.objRendered = objRendered;
        // movement = mov;
        this.itemHeld = itemHeld;
        this.holdingItemManager = holdingItemScript;
        this.isHolding = isHolding;
        this.inDialog = inDialog;

        return this;
    }

    // Abstract methods that must be implemented by derived classes
    public virtual void InventorySetup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            inventory.AddToList(new Item(null, 1));
        }
    }
    
}

