using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropIntoHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    
    public int slotIndex;
    public bool isHovered = false;
    public Transform currentHolder;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {

        currentHolder = transform.Find("Holder");
        player = GameObject.Find("RealPlayer").GetComponent<Player>();
        if (currentHolder.transform.parent.parent.name.Equals("Inventory"))
        {
            slotIndex = currentHolder.transform.parent.GetSiblingIndex() + 4;
        }
        else
        {
            slotIndex = currentHolder.transform.parent.GetSiblingIndex();
        }
    }

    // Update is called once per frame
    void Update()
    {

        // do checks on if you can do this within something idk.
        if (isHovered && slotIndex >= 4)
        {
            player.SetIndexOfInventoryHover(slotIndex);
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player.inventory.SwapItem(player.inventory.GetItemUsingIndex(0), player.inventory.GetItemUsingIndex(slotIndex));
                player.inventory.SetRefresh(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                player.inventory.SwapItem(player.inventory.GetItemUsingIndex(1), player.inventory.GetItemUsingIndex(slotIndex));
                player.inventory.SetRefresh(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                player.inventory.SwapItem(player.inventory.GetItemUsingIndex(2), player.inventory.GetItemUsingIndex(slotIndex));
                player.inventory.SetRefresh(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                player.inventory.SwapItem(player.inventory.GetItemUsingIndex(3), player.inventory.GetItemUsingIndex(slotIndex));
                player.inventory.SetRefresh(true);
            }
        }

        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        Debug.Log("Entered slot " + slotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        Debug.Log("Exited slot " + slotIndex);
    }

}
