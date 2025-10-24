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
    public QuestInstance instanceToWaitFor;
    public QuestInstance instanceFromPlayer;
    [SerializeField]
    private bool questRequired;
    public Transform affectedObject;
    public PauseScript pauseCheck;
    public bool inDialog { get; set; }

    void Start()
    {
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(Keybinds.actionKeybind) && !amITurnedOn && !inDialog  && !pauseCheck.isInventory && !pauseCheck.isPaused)
        {
            if (questRequired)
            {
                if (instanceToWaitFor != null && instanceFromPlayer != null)
                {
                    if (instanceToWaitFor.data.id == instanceFromPlayer.data.id)
                        InteractionFunction();
                }
            } else
            {
                InteractionFunction();
            }
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
        affectedObject.position += Vector3.down * 3;
    
        yield return new WaitForSeconds(duration);

        Debug.Log("Gate closing...");
        sprite.color = Color.green;
        amITurnedOn = false;
        affectedObject.position += Vector3.up * 3;
    }
    


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (other.GetComponent<QuestManager>().GetCurrentQuest() != null)
            {
                instanceFromPlayer = other.GetComponent<QuestManager>().GetCurrentQuest();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            instanceFromPlayer = null;
        }
    }





}