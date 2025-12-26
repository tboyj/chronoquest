using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// NEW: Serializable item data that stores actual info, not instance IDs
[Serializable]
public class SerializableItem
{
    public string itemName;
    public int quantity;
    
    public SerializableItem(Item item)
    {
        itemName = item.item != null ? item.item.name : "";
        quantity = item.quantity;
    }
}

// NEW: Serializable quest data
[Serializable]
public class SerializableQuest
{
    public int questId;
    public string questName;
    public bool isCompleted;
    
    public SerializableQuest(QuestInstance quest)
    {
        questId = quest.data.id;
        questName = quest.data.questName;
        isCompleted = quest.IsCompleted;
    }
}

// NEW: Serializable todo data
[Serializable]
public class SerializableTodo
{
    public string todoObjectName;
    
    public SerializableTodo(TodoObject todo)
    {
        todoObjectName = todo != null ? todo.name : "";
    }
}

[Serializable]
public class SaveData
{
    public List<SerializableItem> inventoryItems = new List<SerializableItem>();
    public List<SerializableQuest> questsAssigned = new List<SerializableQuest>();
    public List<SerializableQuest> questsCompleted = new List<SerializableQuest>();
    public List<SerializableTodo> currentTodo = new List<SerializableTodo>();
    public int currentQuestId;
    public bool hasCompletedFirstQuest;
    public int itemHeld;
    public bool isHolding;
    public float timeInDay;
    public SerializableVector3 playerPosition = new SerializableVector3();
    public List<string> npcNames = new List<string>();
    public List<SerializableVector3> npcPositions = new List<SerializableVector3>();
    public string currentSceneName;
    public bool isInventorySetup;
}

[Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 v) {
        x = v.x; y = v.y; z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
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
        Debug.Log($"=== SAVE GAME DEBUG ===");
        Debug.Log($"Player GameObject: {player.gameObject.name}");
        Debug.Log($"Player Scene: {player.gameObject.scene.name}");
        Debug.Log($"Player Scene IsValid: {player.gameObject.scene.IsValid()}");
        Debug.Log($"Player Scene IsLoaded: {player.gameObject.scene.isLoaded}");
        Debug.Log($"Player Position: {player.gameObject.transform.position}");
        SaveData data = new SaveData();
        
        // Save scene
        data.currentSceneName = player.gameObject.scene.name;
        
        // Save player position
        data.playerPosition = new SerializableVector3(player.gameObject.transform.position);

        // Save NPCs
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            NPC npcComponent = npc.GetComponent<NPC>();
            if (npcComponent != null)
            {
                data.npcNames.Add(npcComponent.gameObject.name);
                data.npcPositions.Add(new SerializableVector3(npcComponent.gameObject.transform.position));
            }
        }

        // Save inventory - FIXED to save actual data
        foreach (Item item in player.inventory.items)
        {
            data.inventoryItems.Add(new SerializableItem(item));
        }
        
        data.itemHeld = player.itemHeld;
        data.isHolding = player.isHolding;


        data.isInventorySetup = player.isInventorySetup;


        // Save quest data - FIXED
        QuestManager questManager = player.GetComponent<QuestManager>();
        if (questManager != null)
        {
            foreach (QuestInstance quest in questManager.questsAssigned)
            {
                data.questsAssigned.Add(new SerializableQuest(quest));
            }
            
            foreach (QuestInstance quest in questManager.questsCompleted)
            {
                data.questsCompleted.Add(new SerializableQuest(quest));
            }
            
            var currentQuest = questManager.GetCurrentQuest();
            if (currentQuest != null && currentQuest.todo != null && currentQuest.todo.Count > 0)
            {
                foreach (TodoObject todo in currentQuest.todo)
                {
                    data.currentTodo.Add(new SerializableTodo(todo));
                }
            }
            
            data.currentQuestId = CurrentQIDMonitor.Instance.GetCurrentQuestId();
            data.hasCompletedFirstQuest = questManager.hasCompletedFirstQuest;
        }

        // Save time
        GameObject sunObject = GameObject.Find("Sun");
        if (sunObject != null)
        {
            DayAndNight dayNightCycle = sunObject.GetComponent<DayAndNight>();
            if (dayNightCycle != null)
            {
                data.timeInDay = dayNightCycle.timeInDay;
            }
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

        // Load UtilityScene additively if not already loaded
        Scene utilityScene = SceneManager.GetSceneByName("UtilityScene");
        if (!utilityScene.isLoaded)
        {
            SceneManager.LoadScene("UtilityScene", LoadSceneMode.Additive);
        }

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Player inventory is null, cannot apply loaded data.");
            return;
        }

        // Restore inventory - FIXED to load by name
        List<Item> loadedItems = new List<Item>();
        foreach (SerializableItem savedItem in data.inventoryItems)
        {
            if (!string.IsNullOrEmpty(savedItem.itemName))
            {
                // Find the ItemStorable asset by name
                ItemStorable itemStorable = Resources.Load<ItemStorable>($"Items/{savedItem.itemName}");
                if (itemStorable == null)
                {
                    // Try without path
                    itemStorable = Resources.Load<ItemStorable>(savedItem.itemName);
                }
                
                if (itemStorable != null)
                {
                    Item item = new Item(itemStorable, savedItem.quantity);
                    loadedItems.Add(item);
                }
                else
                {
                    Debug.LogWarning($"Could not find ItemStorable: {savedItem.itemName}");
                    loadedItems.Add(new Item(null, 0)); // Empty slot
                }
            }
            else
            {
                loadedItems.Add(new Item(null, 0)); // Empty slot
            }
        }
        
        inventory.SetInventory(loadedItems);
        player.itemHeld = data.itemHeld;
        player.isHolding = data.isHolding;
        inventory.SetRefresh(true);
        player.isInventorySetup = data.isInventorySetup;
        // Restore quests - FIXED to load by ID
        QuestManager questManager = player.GetComponent<QuestManager>();
        
        if (questManager != null)
        {
            questManager.isLoadingFromSave = true;

            foreach (QuestInstance quest in questManager.questsAssigned)
            {
                Debug.Log($"assigned quest before load: {quest.data.questName} (ID: {quest.data.id})");
                
            }
            foreach (QuestInstance quest in questManager.questsCompleted)
            {
                Debug.Log($"completed quest before load: {quest.data.questName} (ID: {quest.data.id})");
            }
            
            questManager.questsAssigned.Clear();
            questManager.questsCompleted.Clear();
            
             // After clear it bugs. If you have time rewrite this logic so that only one call is needed. Thanks
            // Find and restore assigned quests

            // Find and restore completed quests
            foreach (SerializableQuest savedQuest in data.questsCompleted)
            {
                QuestInstance quest = FindQuestById(savedQuest.questId);
                if (quest != null)
                {
                    quest.IsCompleted = true;
                    questManager.questsCompleted.Add(quest);
                }
            }

            
                    
            StartingSceneQuest startingQuest = FindObjectOfType<StartingSceneQuest>();
            if (startingQuest != null)
            {
                Debug.Log("Manually triggering RuntimeQuest for new scene");
                startingQuest.RuntimeQuest(questManager);
            }
            else
            {
                Debug.LogWarning("No StartingSceneQuest found in new scene!");
            }

            foreach (SerializableQuest savedQuest in data.questsAssigned)
            {
                QuestInstance quest = FindQuestById(savedQuest.questId);
                if (quest != null)
                {
                    quest.IsCompleted = savedQuest.isCompleted;
                    questManager.questsAssigned.Add(quest);
                }
                else
                {
                    Debug.LogWarning($"Could not find quest with ID: {savedQuest.questId}");
                }
            }
            
            // Re-initialize conditions
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
                
                // Restore todos if needed
                if (data.currentTodo != null && data.currentTodo.Count > 0)
                {
                    // This part depends on how your TodoObject system works
                    // You may need to find todos by name similar to quests
                }
            }
            
            questManager.hasCompletedFirstQuest = data.hasCompletedFirstQuest;
            questManager.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
            
            questManager.StartCoroutine(questManager.SyncNPCsWithQuestState());
        }

        // Restore time
        GameObject sunObject = GameObject.Find("Sun");
        if (sunObject != null)
        {
            DayAndNight dayNightCycle = sunObject.GetComponent<DayAndNight>();
            if (dayNightCycle != null)
            {
                dayNightCycle.SetTimeOfDay(data.timeInDay);
            }
        }
        
        // Restore player position
        player.movement.controller.enabled = false;
        player.transform.position = data.playerPosition.ToVector3();
        player.movement.controller.enabled = true;
        
        // Restore NPC positions
        for (int i = 0; i < data.npcNames.Count; i++)
        {
            GameObject npcObject = GameObject.Find(data.npcNames[i]);
            if (npcObject != null)
            {
                NavMeshAgent agent = npcObject.GetComponent<NavMeshAgent>();
                CharacterController controller = npcObject.GetComponent<CharacterController>();
                
                if (agent != null) agent.enabled = false;
                if (controller != null) controller.enabled = false;
                
                npcObject.transform.position = data.npcPositions[i].ToVector3();
                
                if (agent != null) agent.enabled = true;
                if (controller != null) controller.enabled = true;
            }
        }
        
        Debug.Log("Game data loaded successfully");
        questManager.isLoadingFromSave = false;
    }

    // Helper method to find quests by ID
    private QuestInstance FindQuestById(int questId)
    {
        // Search all QuestInstance objects in the scene
        QuestInstance[] allQuests = FindObjectsOfType<QuestInstance>();
        foreach (QuestInstance quest in allQuests)
        {
            if (quest.data != null && quest.data.id == questId)
            {
                return quest;
            }
        }
        return null;
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

    public void SaveGameAdvancingScene(Player player, int next)
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
        string sceneName = SceneManager.GetSceneByBuildIndex(next).name;

        Scene utilityScene = SceneManager.GetSceneByName("UtilityScene");
        if (!utilityScene.isLoaded)
        {
            SceneManager.LoadScene("UtilityScene", LoadSceneMode.Additive);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(next));

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        Debug.Log(SceneManager.GetActiveScene().name);
        Player newPlayer = null;
        foreach (GameObject obj in rootObjects)
        {
            Debug.Log(obj.name);
            if (obj.CompareTag("Player"))
            {
                newPlayer = obj.GetComponent<Player>();
                break;
            }
        } // this finds the new player in the new scene

        SaveData data = new SaveData();
        
        if (newPlayer == null)
        {
            Debug.LogError("New Player is null, cannot save game.");
            return;
        }

        data.currentSceneName = sceneName;
        data.playerPosition = new SerializableVector3(newPlayer.gameObject.transform.position); // New Player position


        foreach (GameObject npc in rootObjects)
        {
            // Check to see if there's any npcs in NEW scene.
            if (npc.CompareTag("NPC"))
            {
                NPC npcComponent = npc.GetComponent<NPC>();
                if (npcComponent != null)
                {
                    data.npcNames.Add(npcComponent.gameObject.name);
                    data.npcPositions.Add(new SerializableVector3(npcComponent.gameObject.transform.position));
                }
            }
        }

        foreach (Item item in player.inventory.items) // Grab inventory from old player
        {
            data.inventoryItems.Add(new SerializableItem(item));
        }

        data.itemHeld = player.itemHeld;
        data.isHolding = player.isHolding;
        data.isInventorySetup = player.isInventorySetup;

        QuestManager questManagerOld = player.GetComponent<QuestManager>();
        QuestManager questManagerNew = newPlayer.GetComponent<QuestManager>(); // Grab quests from old player
        if (questManagerOld != null)
        {
            List<QuestInstance> assignedCopy = new List<QuestInstance>(questManagerOld.questsAssigned);
            foreach (QuestInstance quest in assignedCopy)
            {
                if (quest != null)
                {
                    questManagerOld.SetQuestCompleted(quest);
                }
            }

            foreach (QuestInstance quest in questManagerOld.questsCompleted)
            {
                Debug.Log($"Saving completed quest: {quest.data.questName} (ID: {quest.data.id})");
                data.questsCompleted.Add(new SerializableQuest(quest));
            }
            questManagerNew.questsAssigned.Clear();
            questManagerNew.questsCompleted.Clear();
            
            StartingSceneQuest startingQuest = FindObjectOfType<StartingSceneQuest>();
            // if (startingQuest != null)
            // {
            //     Debug.Log("Manually triggering RuntimeQuest for new scene");
            //     startingQuest.RuntimeQuest(questManagerNew);
            // }
            // else
            // {
            //     Debug.LogWarning("No StartingSceneQuest found in new scene!");
            // }
            // for now..
            data.currentQuestId = CurrentQIDMonitor.Instance.GetCurrentQuestId();
            data.hasCompletedFirstQuest = questManagerNew.hasCompletedFirstQuest;
        } 
        foreach (GameObject sun in rootObjects)
        {
            if (sun.name == "Sun") {
                GameObject sunObject = GameObject.Find("Sun");
                if (sunObject != null)
                {   
                    DayAndNight dayNightCycle = sunObject.GetComponent<DayAndNight>();
                    if (dayNightCycle != null)
                    {
                        data.timeInDay = dayNightCycle.timeInDay;
                    }
                }
            }
        }

        Debug.Log("Game saved after scene transition and quest assignment");    
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
        player.GetComponent<QuestManagerGUI>()?.RefreshQuestGUI();
    }
}