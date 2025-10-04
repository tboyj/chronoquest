using System.Collections.Generic;
using UnityEngine;
public class QuestHandler : MonoBehaviour // for npcs
{
    [SerializeField]
    public List<QuestInstance> questsInStock = new List<QuestInstance>();

    public QuestInstance GetMostRecentQuest()
    {
        if (questsInStock.Count > 0)
        {
            return questsInStock[0];
        }
        else
        {
            return null;
        }
    }
    public Item GetRequiredItemOfMostRecentQuest()
    {
        QuestInstance recentQuest = GetMostRecentQuest();
        if (recentQuest is QuestCollectItem collectItemQuest)
        {
            return new Item(collectItemQuest.requiredItem, collectItemQuest.requiredCount);
        }
        
        return null;
    }
    public ItemStorable GetRequiredItemStorableOfMostRecentQuest()
    {
        QuestInstance recentQuest = GetMostRecentQuest();
        if (recentQuest is QuestCollectItem collectItemQuest)
        {
            return collectItemQuest.requiredItem;
        }

        return null;
    }
    public List<QuestInstance> GetQuestList()
    {
        return questsInStock;
    }
}