

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
    public override void QuestEventTriggered()
    {
        if (IsCompleted)
        {
            Debug.Log("Quest already completed, ignoring trigger: " + data.id);
            return;
        }

        currentCount++;
        Debug.Log("Current:" + questManager.GetCurrentQuest().data.id + "This:" + data.id);

        // Only advance if this quest is the current active one
        if (questManager.GetCurrentQuest().data.id == data.id)
        {
            questManager.gameObject.GetComponent<QuestManagerGUI>().GotoNextTodo();
        }

        IsCompleted = CheckConditions();
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