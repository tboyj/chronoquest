using UnityEngine;
using ChronoQuest.Interactions.World;
public class Door : MonoBehaviour, Interaction
{
    public SpriteRenderer spriteRenderer;
    public bool isOpen = false;
    public bool playerInTrigger = false;
    public bool unlocked = false;
    public int keysNeeded = 0;
    
    public void Start()
    {

    }

    public void OpenDoor()
    {
        isOpen = true;
        spriteRenderer.color = Color.black;
    }

    public void CloseDoor()
    {
        isOpen = false;
        spriteRenderer.color = Color.white;
    }
    public void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractionFunction();
            }

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
           
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    public void InteractionFunction()
    {
        if (unlocked)
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }
}