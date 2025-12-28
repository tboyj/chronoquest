using UnityEngine;

public class SaveInstance : MonoBehaviour
{
    public Player player;
    public bool loadOnStart = true;
    public bool autoSave = true;
    public float autoSaveInterval = 10f;
    private float autoSaveTimer = 0f;

    void Start()
    {
        Debug.Log($"=== SaveInstance Start ===");
        Debug.Log($"SaveInstance GameObject: {gameObject.name}");
        Debug.Log($"SaveInstance Scene: {gameObject.scene.name}");
        
        if (player != null)
        {
            Debug.Log($"Assigned Player: {player.gameObject.name}");
            Debug.Log($"Assigned Player Scene: {player.gameObject.scene.name}");
        }
        else
        {
            Debug.LogError("Player reference is NULL in SaveInstance!");
        }
        
        // FIXED: Only load on start, don't save immediately!
        if (loadOnStart && SaveHandler.Instance != null)
        {
            LoadGame();
        }
        TimerJSON.Instance.StartTimer();
    }

    void Update()
    {
        if (autoSave)
        {
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer >= autoSaveInterval)
            {
                SaveGame();
                autoSaveTimer = 0f;
            }
        }
    }

    public void LoadGame()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set in SaveInstance!");
            return;
        }

        SaveData data = SaveHandler.Instance.LoadGame();
        if (data != null)
        {
            SaveHandler.Instance.ApplyLoadedData(data, player);
            Debug.Log("Game loaded successfully from SaveInstance");
        }
        else
        {   
            Debug.Log("No save file found, starting fresh game");
        }
    }

    public void SaveGame()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set in SaveInstance!");
            return;
        }

        SaveHandler.Instance.SaveGame(player);
    }
}
