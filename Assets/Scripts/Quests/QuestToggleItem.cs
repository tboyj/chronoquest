

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using ChronoQuest.Interactions.World;
[System.Serializable]
public class QuestToggleItem : QuestInstance
{
    public bool toggled; // List of QuestInstance
    public bool firstTime = false;
    public QuestToggleItem(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s, bool toggled) : base(q, i, t, d, s)
    {
        this.toggled = toggled;
    }

    public void QuestEventTriggered()
    {
        if (firstTime == false)
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
                        firstTime = true;
                    }
                    questManager.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
                }
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