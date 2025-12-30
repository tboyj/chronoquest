using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using System;

[System.Serializable]
public class EndGameQuest : QuestInstance, IQuestAction
{

    
    public override void Start()
    {
        base.Start();
    }

    public override void QuestEventTriggered()
    {
        // This should be called when dialog is finished or conditions are met
        Debug.Log($"QuestEventTriggered for {data.questName}");
        
        if (CheckConditions())
        {
            IsCompleted = true;
            Debug.Log($"Quest {data.questName} completed!");
        }
    }

    public override bool CheckConditions()
    {
        // ONLY check conditions, don't modify quest state
        Debug.Log($"Checking conditions for {data.questName}");
        
        if (questManager == null)
        {
            Debug.LogWarning("QuestManager is null");
            return false;
        }
        
        QuestInstance currentQuest = questManager.GetCurrentQuest();
        if (currentQuest == null)
        {
            Debug.Log("No current quest assigned");
            return false;
        }
        
        
        
        // Check if this is the active quest
        if (currentQuest.data.id != data.id)
        {
            Debug.Log($"Quest ID mismatch: Current={currentQuest.data.id}, This={data.id}");
            return false;
        }
        
        return true;
    }
}