using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackToMenu : MonoBehaviour
{
    public void ReturnToMenu()
{
    // Resume time in case the game was paused
    Time.timeScale = 1f;
    
    // Get the current active scene
    Scene currentScene = SceneManager.GetActiveScene();
    
    // Only unload if it's not the UtilityScene
    if (currentScene.name != "UtilityScene")
    {
        // Load the menu scene first
        SceneManager.LoadScene("RealTitleScreen");
        
        // Then unload the current scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
    else
    {
        // If we're somehow in UtilityScene, just load the menu
        SceneManager.LoadScene("RealTitleScreen");
    }
}
}