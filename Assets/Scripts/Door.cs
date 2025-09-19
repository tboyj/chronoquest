using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isOpen = false;
    public bool doorTrigger = false;
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
        if (doorTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E) && unlocked)
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorTrigger = true;
           
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorTrigger = false;
        }
    }
    


}