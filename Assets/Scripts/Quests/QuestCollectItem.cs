

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
        currentCount++;
        Debug.Log("Current:" + questManager.GetCurrentQuest().data.id + "This:" + data.id);
        if (questManager.GetCurrentQuest().data.id == data.id)
        {
            questManager.gameObject.GetComponent<QuestManagerGUI>().GotoNextTodo();
            // Also go to the next pointer position.
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