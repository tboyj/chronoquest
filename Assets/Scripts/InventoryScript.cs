using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryScript : MonoBehaviour
{
    public static List<InventorySlot> inventory = new List<InventorySlot>();
    public List<RectTransform> panels = new List<RectTransform>();
    public Transform inventoriesRoot;
    
    public void AddItem(InventorySlot item)
    {
        inventory.Add(item);
    }

    public void RemoveItem(InventorySlot item)
    {
        inventory.Remove(item);
    }

    void Start()
    {
        ScanInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
            


    }

    void ScanInventory()
    {
        foreach (Transform panel in inventoriesRoot)
        {
            foreach (Transform slot in panel)
            {
                
            }
        }
    }
}

