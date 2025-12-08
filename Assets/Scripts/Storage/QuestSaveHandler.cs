using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class QuestSaveData
{
    public List<int> assignedQuestIds = new List<int>();
    public List<int> completedQuestIds = new List<int>();
    public int currentQuestId;
    public bool hasCompletedFirstQuest;
}

public class QuestSaveHandler : MonoBehaviour
{
    private static QuestSaveHandler instance;
    public static QuestSaveHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestSaveHandler>();
                if (instance == null)
                {
                    GameObject go = new GameObject("QuestSaveHandler");
                    instance = go.AddComponent<QuestSaveHandler>();
                }
            }
            return instance;
        }
    }

    private string questSaveFilePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            questSaveFilePath = Path.Combine(Application.persistentDataPath, "quests.json");
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveQuests(QuestManager questManager)
    {
        if (questManager == null)
        {
            Debug.LogError("QuestManager is null, cannot save quests.");
            return;
        }

        QuestSaveData data = new QuestSaveData();
        
        foreach (var q in questManager.questsAssigned)
        {
            if (q != null && q.data != null)
                data.assignedQuestIds.Add(q.data.id);
        }

        foreach (var q in questManager.questsCompleted)
        {
            if (q != null && q.data != null)
                data.completedQuestIds.Add(q.data.id);
        }

        if (CurrentQIDMonitor.Instance != null)
            data.currentQuestId = CurrentQIDMonitor.Instance.GetCurrentQuestId();
            
        data.hasCompletedFirstQuest = questManager.hasCompletedFirstQuest;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(questSaveFilePath, json);
        Debug.Log($"Quests saved to {questSaveFilePath}");
    }

    public QuestSaveData LoadQuests()
    {
        if (File.Exists(questSaveFilePath))
        {
            string json = File.ReadAllText(questSaveFilePath);
            QuestSaveData data = JsonUtility.FromJson<QuestSaveData>(json);
            Debug.Log($"Quests loaded from {questSaveFilePath}");
            return data;
        }
        else
        {
            Debug.Log("No quest save file found, starting fresh.");
            return null;
        }
    }

    public bool QuestSaveExists()
    {
        return File.Exists(questSaveFilePath);
    }

    public void DeleteQuestSave()
    {
        if (File.Exists(questSaveFilePath))
        {
            File.Delete(questSaveFilePath);
            Debug.Log("Quest save file deleted");
        }
    }
}
