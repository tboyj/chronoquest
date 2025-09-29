

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Quests/Toggle Action")]
public class QuestToggleItem : QuestInstance
{
    public bool toggled = false;

    public QuestToggleItem(Quest q, bool i, List<string> t, bool toggled) : base(q, i, t)
    {
        this.toggled = toggled;
    }

    public void QuestEventTriggered()
    {
        toggled = true;
        IsCompleted = CheckConditions(); // Called when something is toggled
    }

    public override bool CheckConditions()
    {
        return toggled;
    }

}