using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour
{
    // Properties
    protected string Name;
    public Inventory inventory;
    public SpriteRenderer spriteRenderer;
    public Movement movement;
    protected Item itemTest;
    public bool recievable = true;
    public bool canGive = true;
    public int itemHeld = 0;
    public HoldingItemScript holdingItemManager;

    // Constructor
    protected virtual Character Initialize(string name, Inventory inv, SpriteRenderer spriteR, Movement mov, bool recievable, bool canGive, int itemHeld, HoldingItemScript holdingItemScript)
    {
        Name = name;
        inventory = inv;
        spriteRenderer = spriteR;
        movement = mov;
        this.recievable = recievable;
        this.canGive = canGive;
        this.itemHeld = itemHeld;
        this.holdingItemManager = holdingItemScript;
        return this;
    }

    // Abstract methods that must be implemented by derived classes
    public abstract void InventorySetup();
}

