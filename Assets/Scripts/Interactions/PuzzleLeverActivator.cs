using ChronoQuest.Interactions.World;
using UnityEngine;
using ChronoQuest.Quests;
using System.Collections;
using ChronoQuest.UIForInteractions;
public class PuzzleLeverActivator : MonoBehaviour, Interaction, IAvailableActions
{
    public SpriteRenderer sprite;
    public Color savedColor;
    public bool toggled = false;
    public Transform decoderScript;
    public bool playerInTrigger = false;
    private Player player;
    public QuestInstance playerQuest;
    public PauseScript pauseCheck;
    public bool inDialog { get; set; }
    public bool conditionsMet = false; // can be based on this specific lever. idk.
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        savedColor = sprite.color;
        //questAttached.IsCompleted = false;
        //decoderScript = transform.parent;
    }

    void Update()
    {
        InteractionFunction();
    }
    public void InteractionFunction() // Add logic here (fix structuring of if statement later)
    {
        // if (playerQuest == null)
        // {
        //     // Debug.Log("Hello Twan, You have No player quest. Dattebayo!");
        // }
        if (playerInTrigger && !pauseCheck.isInventory && !pauseCheck.isPaused
        && !inDialog)
        {
            if (Input.GetKeyDown(Keybinds.actionKeybind))
            {
                toggled = !toggled;
                if (!conditionsMet)
                {
                    LeverCheck();

                }
            }
        }
    }
        

    void LeverCheck()
    {
        if (toggled)
        {
            conditionsMet = true;
            Debug.Log("Gate opening...");
            sprite.color = Color.white;
            ChangeTheUI("");
            decoderScript.GetComponent<CorrectOrderOpenScript>().CheckOrder(gameObject);                     
        }
        
        // +5 on each as a shift up since the baseplate is set at 5
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))

        {
            player = other.GetComponent<Player>();
            if (!conditionsMet)
            {
                ChangeTheUI("[F] Press "+gameObject.name.ToString());
            }
            playerQuest = other.GetComponent<QuestManager>().GetCurrentQuest();
            // Debug.Log(other.GetComponent<QuestManager>().GetCurrentQuest().data.questName);
            // Debug.Log(other.GetComponent<QuestManager>().GetCurrentQuest().GetType().ToString());
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInTrigger = false;
            ChangeTheUI("");
        }
    }
    public IEnumerator WrongCoroutine()
    {
        Debug.Log("Gate opening...");
        sprite.color = Color.black;
    
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Gate closing...");
        sprite.color = savedColor;
    }
    public void WrongCoroutineExecutor()
    {
        StartCoroutine(WrongCoroutine());
    }

    public void ChangeTheUI(string str)
    {
        if (player != null)
            player.interactionPanel.text = str;
    }

    public void ChangeTheUI(Item item)
    {
        throw new System.NotImplementedException();
    }
}
