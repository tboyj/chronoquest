using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemStorable item;
    public InventorySlot realItem;
    public SpriteRenderer rend;
    void Start()
    {
        realItem = new InventorySlot(item);
        rend.sprite = realItem.item.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
