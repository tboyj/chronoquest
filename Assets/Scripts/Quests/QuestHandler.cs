using System;
using System.Collections.Generic;
using UnityEngine;
public class QuestHandler : MonoBehaviour // for npcs
{
    [SerializeField]
    private List<Quest> questsInStock = new List<Quest>();
    
    public Quest GetMostRecentQuest()
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

    public List<Quest> GetQuestList()
    {
        return questsInStock;
    }
}