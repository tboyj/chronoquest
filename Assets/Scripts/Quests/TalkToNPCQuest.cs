

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using System;
[System.Serializable]
public class TalkToNPCQuest : QuestInstance, IQuestAction
{
    // public int requiredCount;
    // public int currentCount;
    public NPC npc;
    public NPC questAssignerNPC;
  public override void Start()
    {
        base.Start();

        // Try to assign NPC if null (MAGIC)
        if (npc == null)
        {
            npc = gameObject.GetComponent<NPC>();
            if (npc == null)
            {
                Debug.LogWarning($"NPC reference is null on {gameObject.name}. Assign questAssignerNPC in inspector!");
            }
            else
            {
                Debug.Log($"NPC automatically assigned from {gameObject.name}");
            }
        }
    }

        
        // requiredCount = requiredCount;
        // currentCount = currentCount;
    public override void QuestEventTriggered()
    {
        IsCompleted = CheckConditions();
        if (IsCompleted)
        {
            Debug.Log("Completed");
        }
        else
        {
            ShowDialog(false);
            npc.inDialog = false;
            questManager.gameObject.GetComponent<Player>().inDialog = false;
        }  
        
            // Called when item is collected
    }

    public override bool CheckConditions()
    {
        Debug.Log(name);
        Debug.Log(questManager.name);
        if (questManager.GetCurrentQuest() != null && questAssignerNPC.questHandler.GetMostRecentQuest() != null)
        {
            if (questManager.GetCurrentQuest().data.id == questAssignerNPC.questHandler.GetMostRecentQuest().data.id)
            {
                Debug.Log("Is quest in range? : ");
                if (npc != null && npc.GetInRange())
                {
                    Debug.Log("Quest Being Completed: "+questManager.GetCurrentQuest().data.name);
                    questManager.SetQuestCompleted(questManager.GetCurrentQuest());
                    questAssignerNPC.questHandler.questsInStock.RemoveAt(0);
                    questManager.TryToGiveQuest(npc, questManager.gameObject.GetComponent<Player>().dialogManager);
                    return true;
                }
            }
            else
            {
                Debug.Log("Quest ID mismatch: " + questManager.GetCurrentQuest().data.id + " vs " + data.id);
            }
        }
        else
        {
            Debug.Log("Current quest is null: " + Environment.StackTrace.ToString()); // Thank you Dad <3

        }
        return false;
    }
}