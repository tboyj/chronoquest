using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingScripts : MonoBehaviour
{
    [Header("Persistent Objects")]
    public GameObject player;
    public GameObject canvas;
    public GameObject quests;

    [Header("Next Scene Name")]
    public string nextScene;
    private bool isLoading = false;
    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isLoading && sceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            StartCoroutine(LoadYourAsyncScene());
        }
    } // fix. works for now.


    IEnumerator LoadYourAsyncScene()
    {
        isLoading = true;

        Scene currentScene = SceneManager.GetActiveScene();

        // Start loading in background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Get the new scene reference
        Scene newScene = SceneManager.GetSceneByName(nextScene);
        if (!newScene.IsValid())
        {
            Debug.LogError($"Scene {nextScene} failed to load!");
            yield break;
        }

        // Move persistent objects into the new scene
        SceneManager.MoveGameObjectToScene(canvas, newScene);
        SceneManager.MoveGameObjectToScene(player, newScene);
        SceneManager.MoveGameObjectToScene(quests, newScene);

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
