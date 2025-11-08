using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceSceneQuestScript : QuestInstance
{
    [Header("Persistent Objects")]
    public GameObject player;
    public GameObject canvas;
    public Vector3 teleportToPosition;
    public bool isLoading;
    public QuestInstance nextQuestAfterSceneLoads;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hello");
            IsCompleted = true;
            player.GetComponent<QuestManagerGUI>().GotoNextTodo();
            Debug.Log("Hi again");
            LoadNextScene();
            player.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        }
        else if (other.CompareTag("NPC"))
        {
            other.gameObject.SetActive(false);
        }
    }
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
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
        // Start loading in background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Get the new scene reference
        Scene newScene = SceneManager.GetSceneByBuildIndex(next);
        if (!newScene.IsValid())
        {
            Debug.LogError($"Scene failed to load!");
            yield break;
        }

        // Move persistent objects into the new scene
        SceneManager.MoveGameObjectToScene(canvas, newScene);
        SceneManager.MoveGameObjectToScene(player, newScene);

        // Wait a frame so scene objects initialize properly
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

        // Finally unload the old scene
        SceneManager.UnloadSceneAsync(currentScene);

        isLoading = false;
    }
}
