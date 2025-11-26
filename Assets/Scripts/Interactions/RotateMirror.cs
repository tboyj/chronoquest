using System.Collections;
using UnityEngine;
using ChronoQuest.Interactions.World;
using ChronoQuest.UIForInteractions;
public class RotateMirror : MonoBehaviour, Interaction, IAvailableActions
{
    public bool amITurnedOn = false;
    public bool timedActivater = false;
    [Range(0.0f, float.MaxValue)]
    public float duration = 1f;
    public float rotateIncrement = 15;
    public bool playerInTrigger = false;
    public QuestInstance instanceToWaitFor;
    public QuestInstance instanceFromPlayer;
    [SerializeField]
    private bool questRequired;
    public Transform affectedObject;
    private Player player;
    public PauseScript pauseCheck;
    public bool inDialog { get; set; }
    private bool waitingOnInput;
    void Start()
    {
        pauseCheck = GameObject.Find("RealPlayer").GetComponent<PauseScript>();
    }

    void Update()
    {
        if (playerInTrigger && !amITurnedOn && !inDialog && !pauseCheck.isInventory && !pauseCheck.isPaused)
        {
            if (questRequired)
            {
                if (instanceToWaitFor != null && instanceFromPlayer != null)
                {
                    if (instanceToWaitFor.data.id <= instanceFromPlayer.data.id)
                    { // ew
                        waitingOnInput = true;
                        if (Input.GetKeyDown(Keybinds.actionKeybind))
                            InteractionFunction(); // valid
                    }
                    else
                    {
                        waitingOnInput = false;
                    }
                }
            }
            else // else ignore
            {
                if (Input.GetKeyDown(Keybinds.actionKeybind))
                {
                    InteractionFunction();
                }

            }
        }
    }
    public void InteractionFunction() // Add logic here
    {
        if (timedActivater)
        {
            StartCoroutine(RotateWithCoolDown());
        }
        else
        {
            amITurnedOn = true; // infinite;
        }
    }

    private IEnumerator RotateWithCoolDown()
    {
        Debug.Log("Rotating 30 degrees");
        ChangeTheUI("");
        amITurnedOn = true;
        affectedObject.Rotate(0,rotateIncrement,0);
    
        yield return new WaitForSeconds(duration);

        Debug.Log("cooldown off");
        amITurnedOn = false;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.GetComponent<Player>();
            if (!amITurnedOn)
            {
                // add functionality to where it shows open/close when needed. thats later though. :)))
                if (questRequired && waitingOnInput)
                    ChangeTheUI("[F] Rotate " + gameObject.name.ToString());
                else if (!questRequired)
                    ChangeTheUI("[F] Rotate " + gameObject.name.ToString());
            }
            if (other.GetComponent<QuestManager>().GetCurrentQuest() != null)
            {
                instanceFromPlayer = other.GetComponent<QuestManager>().GetCurrentQuest();
            }

        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.GetComponent<Player>();
            if (!amITurnedOn)
            {
                // add functionality to where it shows open/close when needed. thats later though. :)))
                if (questRequired && waitingOnInput)
                    ChangeTheUI("[F] Rotate " + gameObject.name.ToString());
                else if (!questRequired)
                    ChangeTheUI("[F] Rotate " + gameObject.name.ToString());
            } else
            {
                ChangeTheUI("");
            }
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
            ChangeTheUI("");
            player = null;
            playerInTrigger = false;
            instanceFromPlayer = null;
        }
    }

    public void ChangeTheUI(string str)
    {
        if (player != null)
        {
            player.interactionPanel.text = str;
        }
    }

    public void ChangeTheUI(Item item)
    {
        throw new System.NotImplementedException();
    }
}