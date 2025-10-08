using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemCategory {Weapon, ProgressionItem, TimeStone, Tool, ReplenishItem, QuestItem, KeyItem, Miscellaneous };
public enum ItemRarity {Common, Uncommon, Rare, Epic, Legendary, Exotic};
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

public class ItemStorable : ScriptableObject
{
    [SerializeField]
    private UnityEvent itemUseAction;
    public GameObject prefab;
    public ItemCategory category;
    
    public Sprite sprite;
    
    public string itemName;
    public int id;
    public int maxStackSize;
    public bool stackable;
    public ItemRarity rarity;
    public string description;
    public bool canBeTaken; // im a new soul, i came to this strange world
    public bool canBeGiven; // hoping i could learn a bit on how to give and take (items)

    public void Use()
    {
        Debug.LogWarning("Not implemented. Will do nothing.\nFind in item: "+this.itemName+"\nID: "+this.id);
        // throw new System.NotImplementedException();
    }
}

