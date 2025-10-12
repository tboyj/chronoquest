using ChronoQuest.Interactions.World;
using UnityEngine;
using ChronoQuest.Quests;
using System.Collections;
public class PuzzleLeverActivator : MonoBehaviour, Interaction
{
    public SpriteRenderer sprite;
    public Color savedColor;
    public bool toggled = false;
    public Transform decoderScript;
    public bool playerInTrigger = false;
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
        if (playerInTrigger && Input.GetKeyDown(Keybinds.actionKeybind)  && !pauseCheck.isInventory && !pauseCheck.isPaused
        && !inDialog)
        {
            toggled = !toggled;
            
            }
            if (!conditionsMet)
            {
                LeverCheck();
                
            }
        }

    void LeverCheck()
    {
        if (toggled)
        {
            conditionsMet = true;
            Debug.Log("Gate opening...");
            sprite.color = Color.green;  
            decoderScript.GetComponent<CorrectOrderOpenScript>().CheckOrder(gameObject);                     
        }
        
        // +5 on each as a shift up since the baseplate is set at 5
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))

        {  
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

}
