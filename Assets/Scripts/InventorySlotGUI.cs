using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class InventorySlotGUI : MonoBehaviour
{
    // Start is called before the first frame update
    public InventorySlot slot;
    public Image itemImage;
    public TextMeshProUGUI quantityText;
    void Start()
    {
        slot = transform.GetComponent<InventorySlot>();
        itemImage.sprite = slot.item?.sprite;
        quantityText.text = slot.quantity > 1 ? "x" + slot.quantity : "";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlotGUI();
    }
    public void UpdateSlotGUI()
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
