using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvanceSceneQuestScript : QuestInstance
{
    [Header("Scene References")]
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
            other.GetComponent<QuestManagerGUI>()?.RefreshQuestGUI();
            LoadNextScene();
            
            
            
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

        yield return null;
        
        // Set the new scene as active FIRST
        SceneManager.SetActiveScene(newScene);
        
        // Wait one more frame to ensure Start() methods have run
        yield return null;
        
        // NOW find references in the new scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject canvas = GameObject.Find("Canvas");
        
        if (player == null)
        {
            Debug.LogError("Player not found in new scene!");
        }
        
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in new scene!");
        }

        // Link Day/Night System
        GameObject timeCube = GameObject.Find("TimeCube");
        if (timeCube != null && canvas != null)
        {
            Transform hideForDialog = canvas.transform.Find("HideForDialogContainer");
            if (hideForDialog != null)
            {
                Transform bgDate = hideForDialog.Find("Bg_Date");
                if (bgDate != null)
                {
                    Transform dateTransform = bgDate.Find("Date");
                    if (dateTransform != null)
                    {
                        TextMeshProUGUI dateText = dateTransform.GetComponent<TextMeshProUGUI>();
                        if (dateText != null)
                        {
                            timeCube.GetComponent<TimeCube>().dateText = dateText;
                            Debug.Log("Successfully linked TimeCube to Date text!");
                        }
                    }
                }
            }
        }

        // Teleport player to spawn position
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = teleportToPosition;
                controller.enabled = true;
            }
            else
            {
                player.transform.position = teleportToPosition;
            }
            
            // Reset quest manager state
            QuestManager playerQuestManager = player.GetComponent<QuestManager>();
            if (playerQuestManager != null)
            {
                playerQuestManager.hasCompletedFirstQuest = false;
            }
            
            // Manually trigger the starting quest assignment
            StartingSceneQuest startingQuest = FindObjectOfType<StartingSceneQuest>();
            if (startingQuest != null)
            {
                Debug.Log("Manually triggering RuntimeQuest for new scene");
                startingQuest.RuntimeQuest();
            }
            else
            {
                Debug.LogWarning("No StartingSceneQuest found in new scene!");
            }
            
            // **WAIT ONE MORE FRAME** to ensure RuntimeQuest has fully completed
            yield return null;
            
            // **NOW SAVE** - after quest is assigned
            player.GetComponent<QuestManagerGUI>()?.RefreshQuestGUI();
            
            Debug.Log("Game saved after scene transition and quest assignment");
        }

        // Unload old scene LAST, after everything is set up
        SceneManager.UnloadSceneAsync(currentScene);
        
        isLoading = false;
        hasTriggered = false; // Reset for next time
        SaveHandler.Instance.SaveGame(player.GetComponent<Player>());
        Debug.Log("Scene transition complete.");
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
    }
}