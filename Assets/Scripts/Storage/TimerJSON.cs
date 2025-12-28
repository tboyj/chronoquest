using UnityEngine;
using System;
using System.IO;

[Serializable]
public class TimerSaveData
{
    public float elapsedTime;
    public bool isRunning;
    public string lastSaveTime;
}

public class TimerJSON : MonoBehaviour
{
    public static TimerJSON Instance { get; private set; }
    
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private string saveFilePath;

    void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "timer_save.json");
            LoadTimer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public string GetFormattedTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", 
            time.Hours, time.Minutes, time.Seconds);
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void SaveTimer()
    {
        TimerSaveData data = new TimerSaveData
        {
            elapsedTime = elapsedTime,
            isRunning = isRunning,
            lastSaveTime = DateTime.Now.ToString()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Timer saved: {GetFormattedTime()} at {saveFilePath}");
    }

    public void LoadTimer()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            TimerSaveData data = JsonUtility.FromJson<TimerSaveData>(json);
            
            elapsedTime = data.elapsedTime;
            isRunning = data.isRunning;
            
            Debug.Log($"Timer loaded: {GetFormattedTime()}");
            Debug.Log($"Last saved: {data.lastSaveTime}");
        }
        else
        {
            Debug.Log("No saved timer found. Starting fresh.");
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Timer save deleted.");
        }
        ResetTimer();
    }

    // Auto-save when application quits or pauses (mobile)
    void OnApplicationQuit()
    {
        SaveTimer();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveTimer();
        }
    }

    public void SetTimer(bool isPaused)
    {
        if (isPaused == true)
        {
            StopTimer();
        }
        else
        {
            StartTimer();
        }
    }
}