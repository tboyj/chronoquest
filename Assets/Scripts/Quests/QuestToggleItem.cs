

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using ChronoQuest.Interactions.World;
[System.Serializable]
public class QuestToggleItem : QuestInstance,  IQuestAction
{
    public bool toggled; // List of QuestInstance
    public QuestToggleItem(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s, bool toggled) : base(q, i, t, d, s)
    {
        this.toggled = toggled;
    }

    public void QuestEventTriggered()
    {
        Debug.Log("toggled: " + toggled);
        IsCompleted = CheckConditions();
        if (IsCompleted && todo.Count > 1) {
            questManager.transform.GetComponent<QuestManagerGUI>().GotoNextTodo();
        }
         // Called when something is toggled
    }

    public override bool CheckConditions()
    {
        foreach (QuestInstance quest in relatedQuests)
        {
            Debug.Log("Trying to check conditions");
            if (!quest.CheckConditions())
            {
                return false;
            }
        }
        return toggled;
        
    }
}