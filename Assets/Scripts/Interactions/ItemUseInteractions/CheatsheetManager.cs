using System;
using UnityEngine;
using UnityEngine.UI; // ✅ Not UIElements

public class CheatsheetManager : MonoBehaviour// had to be hardcoded cuz unity was poopy dooping itself :(((
{
    public PauseScript pauseCheck;
    public Player player;
    public ItemStorable item;
    public Image image;
    public bool isActive = false;

    public bool inDialog { get; set; }

    public void Activate()
    {
        
        
        isActive = !isActive;
        image.enabled = isActive;

    }

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.enabled = isActive;
    }
    private void Update()
    {
        if (Input.GetKeyDown(Keybinds.useKeybind) && !inDialog && !pauseCheck.isInventory && !pauseCheck.isPaused)
        {
            if (player.GetHeldItem().item.id == item.id && player.GetHeldItem().quantity > 0)
            {
                Activate();
                player.SetInventoryPermission(false);
            }
        }
    }
    // Example method for using the paper item
}
