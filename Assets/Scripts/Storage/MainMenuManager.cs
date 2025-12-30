using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    
    [Header("Buttons")]
    public Button continueButton;
    public Button newGameButton;
    public Button optionsButton;
    public Button quitButton;
    
    [Header("New Game Confirmation")]
    public GameObject newGameWarningPanel;
    public Button confirmNewGameButton;
    public Button cancelNewGameButton;
    
    [Header("Settings")]
    public string firstSceneName = "Scene1"; // Set this to your first gameplay scene
    

    void Start()
    {
        // Check if a save file exists
        bool saveExists = SaveHandler.Instance != null && SaveHandler.Instance.SaveExists();
        
        // Enable/disable continue button based on save existence
        if (continueButton != null)
        {
            continueButton.interactable = saveExists;
            
            
            // Optional: Add visual feedback for disabled button
            TextMeshProUGUI buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null && !saveExists)
            {
                buttonText.color = Color.gray;
            }
        }
        
        // Setup button listeners
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueGame);
            
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameClick);
            
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OnOptions);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuit);
            
        if (confirmNewGameButton != null)
            confirmNewGameButton.onClick.AddListener(OnConfirmNewGame);
            
        if (cancelNewGameButton != null)
            cancelNewGameButton.onClick.AddListener(OnCancelNewGame);
        
        // Hide panels initially
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
            
        if (newGameWarningPanel != null)
            newGameWarningPanel.SetActive(false);
            
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void OnContinueGame()
    {
        if (SaveHandler.Instance == null)
        {
            Debug.LogError("SaveHandler not found!");
            return;
        }
        
        SaveData data = SaveHandler.Instance.LoadGame();
        
        if (data == null)
        {
            Debug.LogError("No save data found!");
            return;
        }
        
        // Load the utility scene first (additively)
        StartCoroutine(LoadGameFromSave(data));
    }

    public void OnNewGameClick()
    {
        // Check if save exists
        bool saveExists = SaveHandler.Instance != null && SaveHandler.Instance.SaveExists();
        
        if (saveExists && newGameWarningPanel != null)
        {
            // Show warning that save will be deleted
            mainMenuPanel.SetActive(false);
            newGameWarningPanel.SetActive(true);
        }
        else
        {
            // No save exists, start new game directly
            StartNewGame();
            
        }
    }

    public void OnConfirmNewGame()
    {
        // Delete existing save
        if (SaveHandler.Instance != null)
        {
            SaveHandler.Instance.DeleteSave();
            Debug.Log("Previous save deleted");
        }
        
        StartNewGame();
    }

    public void OnCancelNewGame()
    {
        // Return to main menu
        if (newGameWarningPanel != null)
            newGameWarningPanel.SetActive(false);
            
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    private void StartNewGame()
{
    // Delete existing save file
    if (SaveHandler.Instance != null && SaveHandler.Instance.SaveExists())
    {
        SaveHandler.Instance.DeleteSave();
        Debug.Log("Deleted existing save file for new game");
        TimerJSON.Instance.ResetTimer();
        Debug.Log("Timer reset.");
    }
    
    // Reset CurrentQIDMonitor
    if (CurrentQIDMonitor.Instance != null)
    {
        CurrentQIDMonitor.Instance.SetCurrentId(2);
    }
    
    StartCoroutine(LoadNewGame());
}

/// <summary>
/// Load a new game from the main menu.
/// Unloads the main menu scene and loads the first gameplay scene.
/// </summary>
/// <remarks>
/// First loads the UtilityScene additively, then the first gameplay scene.
/// Waits for the scene to finish loading, then sets the first gameplay scene as active.
/// Waits a frame for Start() methods to run, then unloads the main menu scene.
/// </remarks>
    private System.Collections.IEnumerator LoadNewGame()
    {
        // Load UtilityScene first (if needed)
        Scene utilityScene = SceneManager.GetSceneByName("UtilityScene");
        if (!utilityScene.isLoaded)
        {
            AsyncOperation utilityLoad = SceneManager.LoadSceneAsync("UtilityScene", LoadSceneMode.Additive);
            yield return utilityLoad;
        }
        
        // Then load first gameplay scene
        AsyncOperation gameplayLoad = SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
        yield return gameplayLoad;
        
        // Set the gameplay scene as active
        Scene gameplayScene = SceneManager.GetSceneByName(firstSceneName);
        if (gameplayScene.IsValid())
        {
            SceneManager.SetActiveScene(gameplayScene);
        }
        
        // Wait a frame for Start() methods to run
        yield return null;
        
        StartingSceneQuest startingQuest = FindObjectOfType<StartingSceneQuest>();
        if (startingQuest != null)
        {
            Debug.Log("New game - assigning starting quest");
            startingQuest.RuntimeQuest();
        }
        else
        {
            Debug.LogError("No StartingSceneQuest found in scene!");
        }
        // Unload main menu
        SceneManager.UnloadSceneAsync("RealTitleScreen");
        
        Debug.Log("New game started successfully");
    }

    private System.Collections.IEnumerator LoadGameFromSave(SaveData data)
    {
        // Load UtilityScene first
        
        // Load the saved scene
        AsyncOperation savedSceneLoad = SceneManager.LoadSceneAsync(data.currentSceneName, LoadSceneMode.Additive);
        yield return savedSceneLoad;
        
        // Set saved scene as active
        Scene savedScene = SceneManager.GetSceneByName(data.currentSceneName);
        if (savedScene.IsValid())
        {
            SceneManager.SetActiveScene(savedScene);
        }
        
        // Wait for Start() methods
        yield return null;
        
        // Find player and apply save data
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player != null)
            {
                SaveHandler.Instance.ApplyLoadedData(data, player);
                Debug.Log("Save data applied successfully");
            }
            else
            {
                Debug.LogError("Player component not found!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
        
        // Unload main menu
        SceneManager.UnloadSceneAsync("RealTitleScreen");
        
        Debug.Log("Game loaded from save successfully");
    }

    public void OnOptions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
            
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void OnBackFromOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
            
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void OnQuit()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}