using System;
using UnityEngine;

public class AddItemPostDialog : AfterQuestDialog
{
    public NPC sendOver;
    public Player recieve;
    public override void SetChange()
    {
        Debug.Log("HELP ME.");
        if (sendOver != null && recieve != null) // if both has inventory
        {
            if (sendOver.inventory.items.Count > 0)
            {
                recieve.inventory.AddItem(sendOver.inventory.GetItemUsingIndex(0)); // change to get most recentItem maybe later
                Debug.Log("Last chance.");
            }
            else
            {
                Debug.Log("what?");
            }

        } else
        {
            Debug.Log("Wrong, can't add item.");
        }
    }
}