using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignNewQuest : ExtraBase
{
    QuestHandler qh;
    
    public override void Change()
    {
        Debug.Log("Changing right now... Dattebayo!");
        
        if (qh == null || qh.GetQuestList().Count == 0)
        {
            Debug.LogWarning("No quests available in QuestHandler");
            return;
        }
        
        // If there's no current quest OR current quest is completed, assign next one
        QuestInstance currentQuest = qm.GetCurrentQuest();
        
        if (currentQuest == null)
        {
            // No quest assigned, add the first one
            qm.AddQuestToList(qh.GetMostRecentQuest());
            CurrentQIDMonitor.Instance.SetCurrentId(qm.GetCurrentQuest().data.id);
            Debug.Log($"[QuestManager] Assigned new quest {qm.GetCurrentQuest().data.id}");
        }
        else if (currentQuest.IsCompleted)
        {
            // Current quest is done, move to next
            qm.SetQuestCompleted(currentQuest);
            
            if (qh.GetQuestList().Count > 0)
            {
                qm.AddQuestToList(qh.GetMostRecentQuest());
                CurrentQIDMonitor.Instance.SetCurrentId(qm.GetCurrentQuest().data.id);
                Debug.Log($"[QuestManager] Advanced to quest {qm.GetCurrentQuest().data.id}");
            }
        }
        else
        {
            Debug.Log("Current quest not completed yet");
        }
        
        qm.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }

    public void Start()
    {
        base.Start();
        qh = gameObject.GetComponent<QuestHandler>();
        
        if (qh == null)
        {
            Debug.LogError("QuestHandler not found on " + gameObject.name);
        }
        else
        {
            Debug.Log("QuestHandler found: " + qh);
        }
    }
}