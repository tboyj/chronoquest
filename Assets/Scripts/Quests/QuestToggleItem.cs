

using UnityEngine;
using ChronoQuest.Quests;

[CreateAssetMenu(menuName = "Quests/Toggle Action")]
public class QuestToggleItem : Quest
{
    public bool toggled = false;

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