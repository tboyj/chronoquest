using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class InventorySlot : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemStorable item;
    [Range(0, 9999)]
    public int quantity;



    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }
    public InventorySlot(ItemStorable item)
    {
        this.item = item;
        quantity = 1;
    }
    public InventorySlot(ItemStorable item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }


}
