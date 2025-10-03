using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public bool timeIsStopped = false;
    public bool isPaused = false;
    public GameObject inventoryPanel;
    public Sprite inventorySpriteWhileOpen;
    public Sprite inventorySpriteWhileClosed;
    public GameObject inventoryButton;
    public bool isInventory = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(Keybinds.pauseKeybind))
        {
            activatePause();
        }
        else if (Input.GetKeyDown(Keybinds.inventoryKeybind))
        {
            activateInventory();
        }
        else if (Input.GetKeyDown(Keybinds.continueKeybind))
        {
            // do nothing;
        }
    }
    public void activatePause()
    {
        if (!isInventory)
        {
            isPaused = !isPaused;
            timeIsStopped = !timeIsStopped;
        }
        pauseMenu.SetActive(isPaused);
        if (timeIsStopped)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void activateInventory()
    {
        if (!isPaused)
        {
            isInventory = !isInventory;
            timeIsStopped = !timeIsStopped;

        }

        if (!isInventory)
        {
            inventoryButton.GetComponent<UnityEngine.UI.Image>().sprite = inventorySpriteWhileClosed;
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryButton.GetComponent<UnityEngine.UI.Image>().sprite = inventorySpriteWhileOpen;
            inventoryPanel.SetActive(true);
        }
        if (timeIsStopped)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
