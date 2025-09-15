using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSpecificItem : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Item> items = new List<Item>();
    private bool playerInTrigger = false;

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Pressed");
            int index = InventoryScript.instance.GetSelectedIndex();
            Item temp = InventoryScript.instance.GetItemFromIndex(index);

            if (temp != null && index > -1 && temp.item.canBeGiven && InventoryScript.instance.holdingItem)
            {
                items.Add(temp);
                InventoryScript.instance.RemoveItem(index);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

}
