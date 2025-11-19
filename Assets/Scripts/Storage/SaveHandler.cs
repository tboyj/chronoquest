using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<Item> inventoryItems;
    public List<QuestInstance> questsAssigned;
    public List<QuestInstance> questsCompleted;
    public int currentQuestId;
    public bool hasCompletedFirstQuest;
    public int itemHeld;
    public bool isHolding;
    public float timeInDay;
    public Vector3 playerPosition;
}

public class SaveHandler : MonoBehaviour
{
    private static SaveHandler instance;
    public static SaveHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveHandler>();
            }
            return instance;
        }
    }

    private string saveFilePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveGame(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null, cannot save game.");
            return;
        }

        if (player.inventory == null)
        {
            Debug.LogError("Player inventory is null, cannot save game.");
            return;
        }

        SaveData data = new SaveData();

        data.playerPosition = player.transform.position;
        
        // Save inventory
        data.inventoryItems = new List<Item>(player.inventory.items);
        data.itemHeld = player.itemHeld;
        data.isHolding = player.isHolding;

        // Save quest data
        QuestManager questManager = player.GetComponent<QuestManager>();
        if (questManager != null)
        {
            data.questsAssigned = new List<QuestInstance>(questManager.questsAssigned);
            data.questsCompleted = new List<QuestInstance>(questManager.questsCompleted);
            data.currentQuestId = CurrentQIDMonitor.Instance.GetCurrentQuestId();
            data.hasCompletedFirstQuest = questManager.hasCompletedFirstQuest;
        }

        GameObject sunObject = GameObject.Find("Sun");
        if (sunObject != null)
        {
            DayAndNight dayNightCycle = sunObject.GetComponent<DayAndNight>();
            if (dayNightCycle != null)
            {
                data.timeInDay = dayNightCycle.timeInDay;
            }
            else
            {
                Debug.LogWarning("DayAndNight component not found on Sun object.");
            }
        }
        else
        {
            Debug.LogWarning("Sun object not found in the scene.");
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
    }

    public SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"Game loaded from {saveFilePath}");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }

    public void ApplyLoadedData(SaveData data, Player player)
    {
        if (data == null)
        {
            Debug.LogError("SaveData is null, cannot apply loaded data.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player is null, cannot apply loaded data.");
            return;
        }

        player.isInventorySetup = true;
        player.transform.position = data.playerPosition;

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Player inventory is null, cannot apply loaded data.");
            return;
        }

        inventory.SetInventory(data.inventoryItems);
        player.itemHeld = data.itemHeld;
        player.isHolding = data.isHolding;
        inventory.SetRefresh(true);

        // Restore quests
        QuestManager questManager = player.GetComponent<QuestManager>();
        if (questManager != null)
        {
            questManager.questsAssigned = new List<QuestInstance>(data.questsAssigned);
            questManager.questsCompleted = new List<QuestInstance>(data.questsCompleted);
            CurrentQIDMonitor.Instance.SetCurrentId(data.currentQuestId);
            questManager.hasCompletedFirstQuest = data.hasCompletedFirstQuest;
            questManager.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        }
        else
        {
            Debug.LogWarning("QuestManager component not found on Player.");
        }

        GameObject sunObject = GameObject.Find("Sun");
        if (sunObject != null)
        {
            DayAndNight dayNightCycle = sunObject.GetComponent<DayAndNight>();
            if (dayNightCycle != null)
            {
                Debug.Log("Applying time of day from save data: " + data.timeInDay);
                dayNightCycle.SetTimeOfDay(data.timeInDay);
            }
            else
            {
                Debug.LogWarning("DayAndNight component not found on Sun object.");
            }
        }
        else
        {
            Debug.LogWarning("Sun object not found in the scene.");
        }

        Debug.Log("Game data loaded successfully");
    }

    public bool SaveExists()
    {
        return File.Exists(saveFilePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted");
        }
    }
}