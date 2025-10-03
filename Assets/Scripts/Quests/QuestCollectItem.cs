

using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
[System.Serializable]
public class QuestCollectItem : QuestInstance, IQuestAction
{
    public int requiredCount;
    public int currentCount;
    public ItemStorable requiredItem;
    public bool isGiveQuestType;

    public QuestCollectItem(Quest q, bool i, List<string> t, List<QuestDialog> d, List<QuestInstance> s, int requiredCount, int currentCount) : base(q, i, t, d, s)
    {
        this.requiredCount = requiredCount;
        this.currentCount = currentCount;
    }

    public void QuestEventTriggered()
    {
        currentCount++;
        if (questManager.GetCurrentQuest().data.id == data.id)
        {
            questManager.gameObject.GetComponent<QuestManagerGUI>().GotoNextTodo();
        }
        IsCompleted = CheckConditions(); // Called when item is collected
    }

    public override bool CheckConditions()
    {
        return currentCount >= requiredCount;
    }
    public void PrecheckInventory()
    {
        foreach (Item checkedItem in questManager.gameObject.GetComponent<Player>().inventory.items)
        {
            if (checkedItem.quantity > 0)
            {
                currentCount += checkedItem.quantity;
            }
        }
    }

}