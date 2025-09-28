

using UnityEngine;
using ChronoQuest.Quests;

[CreateAssetMenu(menuName = "Quests/Collect Action")]
public class QuestCollectItem : Quest
{
    public int requiredCount;
    public int currentCount;
    

    public void QuestEventTriggered()
    {
        currentCount++;
        IsCompleted = CheckConditions(); // Called when item is collected
    }

    public override bool CheckConditions()
    {
        return currentCount >= requiredCount;
    }

}