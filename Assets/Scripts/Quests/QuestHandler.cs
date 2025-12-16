using System.Collections.Generic;
using UnityEngine;
public class QuestHandler : MonoBehaviour // for npcs
{
    [SerializeField]
    public List<QuestInstance> questsInStock = new List<QuestInstance>();

     public void Start()
    {
        // Remove quests that have already been completed
        QuestManager questManager = GameObject.FindGameObjectWithTag("Player")?.GetComponent<QuestManager>();
        
        if (questManager != null)
        {
            // Remove any quest from stock that's already completed
            questsInStock.RemoveAll(quest => 
                quest != null && 
                questManager.questsCompleted.Exists(completed => 
                    completed != null && completed.data.id == quest.data.id
                )
            );
            
            Debug.Log($"NPC has {questsInStock.Count} quests remaining in stock");
        }
    }
    
    public QuestInstance GetMostRecentQuest()
    {
        if (questsInStock.Count > 0)
        {
            return questsInStock[0];
        }
        return null;
    }

    public List<QuestInstance> GetQuestList()
    {
        return questsInStock;
    }
}