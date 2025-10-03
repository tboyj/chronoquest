using System.Collections;
using UnityEngine;
using ChronoQuest.Interactions.World;
public class GateActivator : MonoBehaviour, Interaction
{
    public SpriteRenderer sprite;
    public bool amITurnedOn = false;
    public bool timedActivater = false;
    [Range(0.5f, float.MaxValue)]
    public float duration = 1f;
    public bool playerInTrigger = false;
    public Transform affectedObject;

    void Start()
    {
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(Keybinds.actionKeybind) && !amITurnedOn)
        {
            InteractionFunction();
        }
    }
    public void InteractionFunction() // Add logic here
    {
        if (timedActivater)
        {
            StartCoroutine(OpenGateRoutine());
        }
        else
        {
            sprite.color = Color.red;
            amITurnedOn = true; // infinite;
        }
    }

    private IEnumerator OpenGateRoutine()
    {
        Debug.Log("Gate opening...");
        sprite.color = Color.red;
        amITurnedOn = true;
        affectedObject.position += Vector3.up * 3;
    
        yield return new WaitForSeconds(duration);

        Debug.Log("Gate closing...");
        sprite.color = Color.green;
        amITurnedOn = false;
        affectedObject.position += Vector3.down * 3;
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