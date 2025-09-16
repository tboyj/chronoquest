using UnityEngine;
using ChronoQuest.Interactions.World;

public class MagnifyIntoPuzzle : MonoBehaviour, IInteractions
{
    public bool playerInTrigger = false;
    public PuzzlePanel puzzlePanel;
    void Start()
    {
        puzzlePanel.gui.setActive(false);
    }
    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
            InteractionFunction();
    }
    public void InteractionFunction() {
        Item temp = InventoryScript.instance.GetItemSelected();
        if (temp.item.id == 0 && temp.quantity > 0 && temp.GetHoldingCondition()) // id 0 = magnifying glass
        {
            puzzlePanel.gui.SetActive(true);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }
}