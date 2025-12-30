using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMM : MonoBehaviour
{
    // Method to call when button is clicked
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("RealTitleScreen");
    }
}