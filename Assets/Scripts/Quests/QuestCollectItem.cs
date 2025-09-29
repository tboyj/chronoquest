

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;

public class QuestCollectItem : QuestInstance
{
    public int requiredCount;
    public int currentCount;

    public QuestCollectItem(Quest q, bool i, List<string> t, int requiredCount, int currentCount) : base(q, i, t)
    {
        this.requiredCount = requiredCount;
        this.currentCount = currentCount;
    }

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