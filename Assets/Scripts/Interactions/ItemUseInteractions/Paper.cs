using System;
using ChronoQuest.UIForInteractions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Paper : BaseUse
{

    public GameObject container;
    [TextArea]
    public string paperText;
    // public ItemStorable itemPreferred; // will likely change up in future but i need this working. SPEED I NEED THIS MY UI'S KIND OF HOMELESS...
    public bool isActive = false;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (GetPlayer() != null)
        {
            if (GetPlayer().GetHeldItem().item != null && GetPlayer().isHolding)
                ChangeTheUI("[E] Use " + GetPlayer().GetHeldItem().item.itemName);
             // only if holding the right thign.
            if (isActive)
            {
                
                // GetPlayer().movement.enabled = false; // bug can occur where character can go outside the trigger zone, disabling.
                GetPlayer().movement.moveSpeed = 0;
                ChangeTheUI("");
            }
            else
            {
                // GetPlayer().movement.enabled = true;
                GetPlayer().movement.moveSpeed = 1; // <-- This is the movement speed of the player. Find another way to do this lol.
            }
        } 
    }
    // Example method for using the paper item
    public override void Use()
    {
        container.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = paperText;
        isActive = !isActive;
        container.SetActive(isActive);
        if (GetPlayer() != null)
        {
            GetPlayer().isUsingItem = isActive;
        }
    }

    
}
