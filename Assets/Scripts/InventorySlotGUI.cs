using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class ItemGUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Item slot;
    public Image itemImage;
    public TextMeshProUGUI quantityText;
    public int slotIndex;
    void Start()
    {
        RefreshFromInventory();    

    }
    public void RefreshFromInventory()
    {
            if (slotIndex < InventoryScript.instance.ReturnInventorySize())
        {
            slot = InventoryScript.instance.GetItemFromIndex(slotIndex); // just data

            if (slot.item != null && slot.quantity > 0)
            {
                itemImage.sprite = slot.item.sprite;
                itemImage.enabled = true;

                if (slot.item.stackable && slot.quantity > 1)
                    quantityText.text = "x" + slot.quantity;
                else
                    quantityText.text = "";
            }
            else
            {
                itemImage.sprite = null;

                itemImage.enabled = false;
                quantityText.text = "";

            }
        }
        else
        {

            Debug.LogWarning($"Slot index {slotIndex} out of range!");
        }
    }
    // Update is called once per frame
    // void Update()
    // {
    //         UpdateSlotGUI(slot);
        
    // }
    public void UpdateSlotGUI(Item slot)
    {
        if (slot.item != null && slot.quantity > 0)
        {
            if (slot.item.stackable)
            {
                itemImage.sprite = slot.item.sprite;
                itemImage.enabled = true;
                quantityText.text = "x" + slot.quantity;
            }
            else
            {
                itemImage.sprite = slot.item.sprite;
                itemImage.enabled = true;
                quantityText.text = "";
            }
        }
        else
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            quantityText.text = "";
        }
    }
}
