using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class InventoryItem
{
    // Start is called before the first frame update
    public ItemStorable item;
    [Range(0, 9999)]
    public int quantity;
    
  

    public InventoryItem()
    {
        item = null;
        quantity = 0;
    }
    public InventoryItem(ItemStorable item)
    {
        this.item = item;
        quantity = 1;
    }
    public InventoryItem(ItemStorable item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }


}
