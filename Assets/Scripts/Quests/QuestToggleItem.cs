

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using ChronoQuest.Interactions.World;
[System.Serializable]
public class QuestToggleItem : QuestInstance
{
    public bool toggled; // List of QuestInstance
    public override void QuestEventTriggered()
    {
       
            toggled = true;
            Debug.Log("toggled: " + toggled);
            IsCompleted = CheckConditions();
            if (IsCompleted)
            {
                if (questManager.gameObject.GetComponent<QuestManagerGUI>() != null)
                {
                    if (questManager.GetCurrentQuest().data.id == data.id)
                    {
                        questManager.gameObject.GetComponent<QuestManagerGUI>().GotoNextTodo();
                        // bring next end to its position.
                    }
                    questManager.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
                }
            }
    }
    
         // Called when something is toggled


    public override bool CheckConditions()
    {
        foreach (QuestInstance quest in relatedQuests) {
            if (!quest.relatedQuests.Contains(this)) {
                if (!quest.CheckConditions())
                {
                    return false;
                }
            }
        }
        return toggled;
    }
}