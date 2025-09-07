using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    // Start is called before the first frame update

    public List<ItemStorable> items = new List<ItemStorable>();
    
    public void AddItem(ItemStorable item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemStorable item)
    {
        items.Remove(item);
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }


}

