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

    public void OnTriggerEnter(Collider other)
    {   
        Debug.Log("GO"+other.gameObject);
        Debug.Log("T"+other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hello");
            questManager.questsAssigned.Clear();
            questManager.SetQuestCompleted(questManager.GetCurrentQuest());

            Debug.Log("Hi again");
            LoadNextScene();
            player.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        }
        else if (other.CompareTag("NPC"))
        {
            other.gameObject.SetActive(false);
        }
    }
    string utilitiesSceneName = "UtilityScene"; // or build index

    Scene GetRealGameplayScene()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != utilitiesSceneName)
            {
                return scene; // This is your real active scene
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
        SceneManager.SetActiveScene(newScene);
        isLoading = false;

        // Find the root object named "DefaultRuntimeQuest" in the newly loaded scene
        GameObject[] roots = newScene.GetRootGameObjects();

        GameObject defaultRuntimeQuest = null;
        foreach (GameObject go in roots)
        {
            if (go.name == "DefaultRuntimeQuest")
            {
                defaultRuntimeQuest = go;
                break;
            }
        }

        if (defaultRuntimeQuest != null)
        {
            Debug.LogError("DefaultRuntimeQuest not found in scene: " + newScene.name);
            Transform child = defaultRuntimeQuest.transform.GetChild(0);
            q = child.GetComponent<QuestInstance>();
        }

        // Now safely get the first child


        Debug.Log(q.data.questName);

        questManager.AddQuestToList(q);
        CurrentQIDMonitor.Instance.SetCurrentId(questManager.GetCurrentQuest().data.id);

        Debug.Log(CurrentQIDMonitor.Instance.GetCurrentQuestId());

        questManager.hasCompletedFirstQuest = false;
        // reset ^^^
        //questManager.GetComponent<Player>().


    }
    public override void QuestEventTriggered()
    {
        Debug.Log("Not Default Lol You Dummy");
    }
}
