using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemStorable : ScriptableObject
{
    public GameObject obj;
    public Texture texture;
    public string itemName;
}