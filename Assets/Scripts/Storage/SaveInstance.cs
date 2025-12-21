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
        SaveGame();
        if (loadOnStart && SaveHandler.Instance != null)
        {
            LoadGame();
        }
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
