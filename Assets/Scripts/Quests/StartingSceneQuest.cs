using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingSceneQuest : MonoBehaviour
{
    public QuestManager questManager;
    
    void Start()  // Changed to Start for better timing
    {
        
        
        if (questManager == null)
        {
            Debug.LogError("QuestManager not set in scene.");
            return;
        }
        
        Debug.Log($"Found QuestManager on {questManager.gameObject.name}");
        
        // Just call it directly - no need to Find ourselves
        RuntimeQuest();
    }

    public void RuntimeQuest()
    {
        if (transform.childCount == 0)
        {
            Debug.LogError("No starting quest found. You may be corrupting save data!");
            return;
        }

        if (questManager == null)
        {
            Debug.LogError("QuestManager unassigned (null ex)");
            return;
        }

        Debug.Log("Running RuntimeQuest - assigning first quest");
        QuestInstance firstQuest = transform.GetChild(0).GetComponent<QuestInstance>();
        
        if (firstQuest == null)
        {
            Debug.LogError("First child has no QuestInstance component!");
            return;
        }
        
        // Mark current quest as completed if it exists
        if (questManager.GetCurrentQuest() != null)
        {
            questManager.GetCurrentQuest().IsCompleted = true;
        }
        
        // Add the new quest
        questManager.AddQuestToList(firstQuest);
        CurrentQIDMonitor.Instance.SetCurrentId(firstQuest.data.id);
        questManager.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        
        Debug.Log($"Assigned quest: {firstQuest.data.questName}");
    }
}