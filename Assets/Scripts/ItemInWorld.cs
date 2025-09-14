using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public int amountOfItemsHere;
    public ItemStorable item;
    public SpriteRenderer rend;
    public bool takeable;
    void Start()
    {
        rend.sprite = item.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item ReturnItemForInventory()
    {
        amountOfItemsHere--;
        return new Item(item,1);
    }

}
