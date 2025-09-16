using UnityEngine;
using System.Collections;


public class FourDigitNumlock_PP : PuzzlePanel
{
    private bool playerInTrigger = false;
    public int[] response = new int[4];
    public int[] solution = new int[4];
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
            TogglePanel(!active);
    }

    private void TogglePanel(bool isActive)
    {
        active = isActive;
        if (gui != null)
            gui.gameObject.SetActive(active);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }
}