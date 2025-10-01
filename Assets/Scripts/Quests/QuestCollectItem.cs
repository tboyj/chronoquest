

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
[System.Serializable]
public class QuestCollectItem : QuestInstance, IQuestAction
{
    public int requiredCount;
    public int currentCount;
    public ItemStorable requiredItem;

    public QuestCollectItem(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s, int requiredCount, int currentCount) : base(q, i, t, d, s)
    {
        if (questManager.GetComponentInParent<Player>().inventory.items != null)
        {
            foreach (Item item in questManager.GetComponentInParent<Player>().inventory.items)
            {
                if (item.item == requiredItem)
                {
                    currentCount += item.quantity;
                }
            }
        
        }
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