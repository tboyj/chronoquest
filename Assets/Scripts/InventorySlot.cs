using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    public ItemStorable item;
    [Range (0, 9999)]
    public int quantity;
    public bool hoverEnabledFilled = false;
    public bool hotbarSlot;
    public Image itemImage;
    public TextMeshProUGUI quantityText;

    void Start()
    {
        UpdateSlot();
        if (gameObject.tag.Equals("Hotbar"))
            hotbarSlot = true;
        else hotbarSlot = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlot();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {

        // follow the mouse
    }
    public void OnEndDrag(PointerEventData eventData)
    {

    }
    public void UpdateSlot()
    {
        if (item != null && quantity > 0)
        {
            if (item.stackable)
            {
                itemImage.sprite = item.sprite;
                itemImage.enabled = true;
                quantityText.text = "x" + quantity;
            }
            else
            {
                itemImage.sprite = item.sprite;
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
