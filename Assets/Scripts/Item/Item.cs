using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class Item
{
    // Start is called before the first frame update
    public ItemStorable item;
    [Range(0, 9999)]
    public int quantity;
    
    public Item()
    {
        item = null;
        quantity = 0;
    }
    public Item(ItemStorable item)
    {
        this.item = item;
        quantity = 1;
    }
    public Item(ItemStorable item, int quantity)
    {
        if (item != null)
        {
            if (quantity <= item.maxStackSize)
            {
                this.item = item;
                this.quantity = quantity;
            }
            else
            {
                this.item = item;
                this.quantity = item.maxStackSize;

            }
        }
    }


}
