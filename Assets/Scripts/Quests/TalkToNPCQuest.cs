

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
    public TalkToNPCQuest(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s, NPC npc) : base(q, i, t, d, s)
    {
        this.npc = npc;
        // this.requiredCount = requiredCount;
        // this.currentCount = currentCount;
    }
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
        Debug.Log(this.name);
        Debug.Log(questManager.name);
        if (questManager.GetCurrentQuest() != null && questAssignerNPC.questHandler.GetMostRecentQuest() != null)
        {
            if (questManager.GetCurrentQuest().data.id == questAssignerNPC.questHandler.GetMostRecentQuest().data.id)
            {
                Debug.Log("Are you correct here");
                Debug.Log(questManager.GetCurrentQuest());
                if (npc.GetInRange())
                {
                    questManager.SetQuestCompleted(questAssignerNPC.questHandler.GetMostRecentQuest());
                    questAssignerNPC.questHandler.questsInStock.RemoveAt(0);
                    questManager.TryToGiveQuest(npc, questManager.gameObject.GetComponent<Player>().dialogManager);
                    Debug.Log("T");
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