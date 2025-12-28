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
    
    private bool hasPreChecked = false; // Add this flag

    public override void QuestEventTriggered()
    {
        if (IsCompleted)
        {
            Debug.Log("Quest already completed, ignoring trigger: " + data.id);
            return;
        }
        if (currentCount < requiredCount)
            currentCount++;
        // Debug.Log("Current:" + questManager.GetCurrentQuest().data.id + "This:" + data.id);

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
        // Only precheck once
        if (hasPreChecked)
            return;
            
        hasPreChecked = true;
        
        Player player = questManager.gameObject.GetComponent<Player>();
        
        foreach (Item checkedItem in player.inventory.items)
        {
            // Make sure we're checking the RIGHT item
            if (checkedItem.item == requiredItem && checkedItem.quantity > 0)
            {
                currentCount += checkedItem.quantity;
            }
        }
        
        // Check if already completed after precheck
        IsCompleted = CheckConditions();
    }
}