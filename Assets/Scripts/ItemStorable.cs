using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory {Weapon, ProgressionItem, TimeStone, Tool, ReplenishItem, QuestItem, KeyItem, Miscellaneous };
public enum ItemRarity {Common, Uncommon, Rare, Epic, Legendary, Exotic};
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

public class ItemStorable : ScriptableObject
{
    public GameObject prefab;
    public ItemCategory category;
    
    public Sprite sprite;
    
    public string itemName;
    public ItemRarity rarity;
    public string description;
    public bool canBeDropped;
    public bool canBeGiven;
}