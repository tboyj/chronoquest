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
    [SerializeField]
    private AudioSource rotateSFX;
    void Start()
    {
        rotateSFX = transform.Find("AudioSources/Rotate").GetComponent<AudioSource>();
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
                        InteractionFunction();

                    }
                    else
                    {
                        waitingOnInput = false;
                    }
                }
            }
            else // else ignore
            {
                InteractionFunction();
            }
        }
    }
    public void InteractionFunction(int multiplier) // Add logic here
    {
        if (timedActivater)
        {
            StartCoroutine(RotateWithCoolDown(multiplier));
        }
        else
        {
            amITurnedOn = true; // infinite;
        }
    }

    private IEnumerator RotateWithCoolDown(int multiplier)
    {
        Debug.Log($"Rotating {rotateIncrement * multiplier} degrees");
        ChangeTheUI("");
        amITurnedOn = true;
        if (multiplier > 0)
        {
            rotateSFX.pitch = 1.2f;
        }
        else if (multiplier < 0)
        {
            rotateSFX.pitch = 0.8f;
        }
        if (!pauseCheck.isInventory && !pauseCheck.isPaused)
            rotateSFX.Play();
        affectedObject.Rotate(0,rotateIncrement*multiplier,0);
        
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
                    ChangeTheUI("[F] Rotate Clockwise\n[E] Rotate Counterclockwise");
                else if (!questRequired)
                    ChangeTheUI("[F] Rotate Clockwise\n[E] Rotate Counterclockwise");
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
                    ChangeTheUI("[F] Rotate Clockwise\n[E] Rotate Counterclockwise");
                else if (!questRequired)
                    ChangeTheUI("[F] Rotate Clockwise\n[E] Rotate Counterclockwise");
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

    public void InteractionFunction()
    {
        if (Input.GetKeyDown(Keybinds.actionKeybind))
            InteractionFunction(1); // valid
        else if (Input.GetKeyDown(Keybinds.useKeybind))
            InteractionFunction(-1); // valid
    }
}