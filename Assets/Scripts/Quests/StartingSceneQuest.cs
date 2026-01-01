using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingSceneQuest : MonoBehaviour
{
    public QuestManager questManager;
    [SerializeField]
    private bool shallPlay;
    public AudioSource a;

    void Start()  // Changed to Start for better timing
    {
        
        a = gameObject.GetComponent<AudioSource>();
        if (a == null) return;
        if (shallPlay) {
            Debug.Log("Playing audio");
            a.Play();
        }
        // if (questManager == null)
        // {
        //     Debug.LogError("QuestManager not set in scene.");
        //     return;
        // }
        
        // Debug.Log($"Found QuestManager on {questManager.gameObject.name}");
        
        // // Just call it directly - no need to Find ourselves
        // Debug.Log(questManager.questsAssigned.Count);
        // if (questManager.questsAssigned.Count == 0)
        // {
        //     Debug.Log("No quests assigned yet, assigning starting quest.");
        //     RuntimeQuest();
        // }
    }

    public void RuntimeQuest(int next)
    {
        var a = SceneManager.GetSceneByBuildIndex(next);
        GameObject[] allObjs = a.GetRootGameObjects();
        
        foreach (GameObject obj in allObjs)
        {
            if (obj.GetComponent<QuestManager>() != null)
            {
                questManager = obj.GetComponent<QuestManager>();
                break;
            }
        }

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
        for (int i = 0; i < questManager.questsAssigned.Count; i++)
        {
            if (questManager.questsAssigned[i] == null)
                Debug.Log($"Currently assigned quest at index {i} is null");
        }
        questManager.AddQuestToList(firstQuest);
        CurrentQIDMonitor.Instance.SetCurrentId(firstQuest.data.id);
        SaveHandler.Instance.SaveGame(questManager.gameObject.GetComponent<Player>());
        
        if (questManager.gameObject.GetComponent<QuestManagerGUI>() == null) return;
        questManager.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        
        Debug.Log($"Assigned quest: {firstQuest.data.questName}");
    }
    public void RuntimeQuest()
    {
        Debug.Log(gameObject.scene.name+": current scene name");
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
        } else {
            
            // Mark current quest as completed if it exists
            if (CurrentQIDMonitor.Instance.GetCurrentQuestId() > firstQuest.data.id)
            {
                firstQuest.IsCompleted = true;
            }
            if (!firstQuest.IsCompleted)
            {
                questManager.AddQuestToList(firstQuest);
                CurrentQIDMonitor.Instance.SetCurrentId(firstQuest.data.id);
                SaveHandler.Instance.SaveGame(questManager.gameObject.GetComponent<Player>());
                if (questManager.gameObject.GetComponent<QuestManagerGUI>() == null) return;
                questManager.gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
            }
        }
        
        // Add the new quest
        for (int i = 0; i < questManager.questsAssigned.Count; i++)
        {
            if (questManager.questsAssigned[i] == null)
                Debug.Log($"Currently assigned quest at index {i} is null");
        }
        
        Debug.Log($"Assigned quest: {firstQuest.data.questName}");
    }
}