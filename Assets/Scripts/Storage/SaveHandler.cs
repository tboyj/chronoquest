using System.Collections.Generic;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public List<Item> inventoryItems;
    public List<QuestInstance> questsAssigned;
    public List<QuestInstance> questsCompleted;
    public List<TodoObject> currentTodo;
    public int currentQuestId;
    public bool hasCompletedFirstQuest;
    public int itemHeld;
    public bool isHolding;
    public float timeInDay;
    public Vector3 playerPosition;
    public Dictionary<string, Vector3> npcPositions = new Dictionary<string, Vector3>();
    public string currentSceneName;
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
    
    // Move this here from Start()
    saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
}

    void Start()
    {
        //saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
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
        // save scene
        data.currentSceneName = player.gameObject.scene.name;
        // save player position
        data.playerPosition = player.gameObject.transform.position;

        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            
            NPC npcComponent = npc.GetComponent<NPC>();
            if (npcComponent != null)
            {
                data.npcPositions.Add(npcComponent.gameObject.name, npcComponent.gameObject.transform.position);
            }
        }

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
            var currentQuest = questManager.GetCurrentQuest();
            if (currentQuest != null && currentQuest.todo != null && currentQuest.todo.Count > 0)
            {
                data.currentTodo = currentQuest.todo;
            }
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
        //checks (for if i need debugging)


        // Load UtilityScene additively if not already loaded
        UnityEngine.SceneManagement.Scene utilityScene = SceneManager.GetSceneByName("UtilityScene");
        if (!utilityScene.isLoaded)
        {
            SceneManager.LoadScene("UtilityScene", LoadSceneMode.Additive);
        }

        if (SceneManager.GetActiveScene().name != data.currentSceneName)
        {
            // Load the saved scene
            SceneManager.LoadScene(data.currentSceneName);
        }

        foreach (var npc in data.npcPositions)
        {
            GameObject npcObject = GameObject.Find(npc.Key);
            if (npcObject != null)
            {
                npcObject.transform.position = npc.Value;
            } else
            {
                Debug.LogWarning($"NPC with name {npc.Key} not found in the scene.");
            }
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
        
        // Re-initialize conditions for all quests
        foreach (var quest in questManager.questsAssigned)
        {
            if (quest != null)
            {
                quest.ReinitializeConditions();
            }
        }
        
        var currentQuest = questManager.GetCurrentQuest();
        if (currentQuest != null)
        {
            CurrentQIDMonitor.Instance.SetCurrentId(currentQuest.data.id);
            
            if (data.currentTodo != null)
            {
                currentQuest.todo = data.currentTodo;
            }
        }
        
        questManager.hasCompletedFirstQuest = data.hasCompletedFirstQuest;
        questManager.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        
        // Force NPC sync after loading
        questManager.StartCoroutine(questManager.SyncNPCsWithQuestState());
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
        foreach (var quest in questManager.questsAssigned)
        {
            if (quest != null)
            {
                quest.ReinitializeConditions();
            }
        }

        foreach (var quest in questManager.questsCompleted)
        {
            if (quest != null)
            {
                quest.ReinitializeConditions();
            }
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