using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvanceSceneQuestScript : QuestInstance
{
    [Header("Persistent Objects")]
    public GameObject player;
    public GameObject canvas;
    public GameObject questsHolder;
    public Vector3 teleportToPosition;
    public bool isLoading;
    private QuestInstance q;
    private bool hasTriggered = false; // Prevent multiple triggers

    public void OnTriggerEnter(Collider other)
    {   
        if (hasTriggered) return; // Prevent double-trigger
        
        Debug.Log("GO: " + other.gameObject);
        Debug.Log("Tag: " + other.tag);
        
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Player entered scene transition trigger");
            
            // Complete the current quest BEFORE clearing
            QuestInstance currentQuest = questManager.GetCurrentQuest();
            if (currentQuest != null && !currentQuest.IsCompleted)
            {
                Debug.Log("Quest Completed: "+currentQuest.data.name);
                questManager.SetQuestCompleted(currentQuest);
            }
            
            // Now safe to clear
            questManager.questsAssigned.Clear();
            questManager.questsCompleted.Clear();
            Debug.Log("Loading next scene...");
            LoadNextScene();
            player.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        }
        else if (other.CompareTag("NPC"))
        {
            other.gameObject.SetActive(false);
        }
    }
    
    string utilitiesSceneName = "UtilityScene";

    Scene GetRealGameplayScene()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != utilitiesSceneName)
            {
                return scene;
            }
        }
        Debug.LogError("No non-utility scene is loaded!");
        return default(Scene);
    }

    public void LoadNextScene()
    {
        Scene realScene = GetRealGameplayScene();

        int currentIndex = realScene.buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadYourAsyncScene(currentIndex, nextIndex));
        }
        else
        {
            Debug.LogWarning("No more scenes in Build Settings.");
        }
    }
    
    IEnumerator LoadYourAsyncScene(int current, int next)
    {
        isLoading = true;

        Scene currentScene = SceneManager.GetSceneByBuildIndex(current);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene newScene = SceneManager.GetSceneByBuildIndex(next);
        if (!newScene.IsValid())
        {
            Debug.LogError($"Scene at index {next} failed to load!");
            yield break;
        }

        // Move persistent objects
        SceneManager.MoveGameObjectToScene(canvas, newScene);
        SceneManager.MoveGameObjectToScene(player, newScene);
        yield return null;

        // Link Day/Night System
        GameObject timeCube = GameObject.Find("TimeCube");
        if (timeCube != null)
        {
            TextMeshProUGUI dateText = canvas.transform.Find("HideForDialogContainer/Bg_Date/Date")
                ?.GetComponent<TextMeshProUGUI>();

            if (dateText != null)
                timeCube.GetComponent<TimeCube>().dateText = dateText;
            else
                Debug.LogWarning("Couldn't find Date Text in Canvas hierarchy!");
        }
        else
        {
            Debug.LogWarning("TimeCube not found in new scene!");
        }

        // Unload old scene
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.SetActiveScene(newScene);

        questManager.hasCompletedFirstQuest = false;
        isLoading = false;
        hasTriggered = false; // Reset for next time
        
        Debug.Log("Scene transition complete. AssignNewQuest will handle quest assignment.");
        
    }
    
    public override void QuestEventTriggered()
    {
        // Enable the collider to allow scene transition
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
            Debug.Log($"Scene advancement enabled for quest: {data.questName}");
        }
        
        // Optional: Visual feedback
        // GameObject exitIndicator = transform.Find("ExitIndicator")?.gameObject;
        // if (exitIndicator != null)
        // {
        //     exitIndicator.SetActive(true);
        // }
    }
}