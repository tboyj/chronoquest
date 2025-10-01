

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using ChronoQuest.Interactions.World;
[System.Serializable]
public class QuestToggleItem : QuestInstance,  IQuestAction
{
    public bool toggled;

    public QuestToggleItem(Quest q, bool i, List<string> t, List<QuestDialog> d, bool toggled) : base(q, i, t, d)
    {
        this.toggled = toggled;
    }

    public void QuestEventTriggered()
    {
        toggled = true;
        Debug.Log("toggled: "+toggled);
        IsCompleted = CheckConditions();
         // Called when something is toggled
    }

    public override bool CheckConditions()
    {
        return toggled;
    }
}