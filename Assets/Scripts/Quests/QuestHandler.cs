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

    public List<QuestInstance> GetQuestList()
    {
        return questsInStock;
    }
}