using ChronoQuest.Interactions.World;
using UnityEngine;
public class LeverActivator : MonoBehaviour, Interaction
{
    public SpriteRenderer sprite;
    public bool toggled = false;
    public Transform affectedObject;
    public bool playerInTrigger = false;

    void Start()
    {
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }

    void Update()
    {
        InteractionFunction();
    }
    public void InteractionFunction() // Add logic here
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            toggled = !toggled;
            leverCheck();
        }
    }
    void leverCheck()
    {
        if (toggled)
        {
            Debug.Log("Gate opening...");
            sprite.color = Color.red;
            affectedObject.position = new Vector3(affectedObject.position.x, (affectedObject.position.y + 3), affectedObject.position.z); //Vector3.up * 3;
        }
        else
        {
            Debug.Log("Gate closing...");
            sprite.color = Color.green;
            affectedObject.position = new Vector3(affectedObject.position.x, (affectedObject.position.y - 3), affectedObject.position.z);
        }
        // +5 on each as a shift up since the baseplate is set at 5
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
