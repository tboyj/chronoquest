using ChronoQuest.Interactions.World;
using UnityEngine;
using ChronoQuest.Quests;
public class LeverActivator : MonoBehaviour, Interaction
{
    public SpriteRenderer sprite;
    public bool toggled = false;
    public Transform affectedObject;
    public bool playerInTrigger = false;
    public QuestToggleItem quest;
    public QuestToggleItem playerQuest;
    public PauseScript pauseCheck;
    public bool inDialog { get; set; }
    public bool firstTime = true; // can be based on this specific lever. idk.
    void Start()
    {
        sprite = transform.Find("Object").GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
        //questAttached.IsCompleted = false;
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
            if (quest != null)
            {
                quest.toggled = toggled;
                Debug.Log(quest.toggled + " Hello America, You have a q- q- q- quest!");
                quest.QuestEventTriggered();
            }
            LeverCheck();
            if (firstTime)
            {
                firstTime = false;
            }
        }
        else if (playerInTrigger && Input.GetKeyDown(Keybinds.actionKeybind) && quest == null)
        {
            toggled = !toggled;
            LeverCheck(); // ignore case if not part of a quest
        }

    
    }
    void LeverCheck()
    {
        if (toggled)
        {
            Debug.Log("Gate opening...");
            sprite.color = Color.red;
            affectedObject.position = new Vector3(affectedObject.position.x, (affectedObject.position.y - 3), affectedObject.position.z); //Vector3.up * 3;
        }
        else
        {
            Debug.Log("Gate closing...");
            sprite.color = Color.green;
            affectedObject.position = new Vector3(affectedObject.position.x, (affectedObject.position.y + 3), affectedObject.position.z);
        }
        // +5 on each as a shift up since the baseplate is set at 5
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))

        {  
            playerQuest = other.GetComponent<QuestManager>().GetCurrentQuest() as QuestToggleItem;
            Debug.Log(other.GetComponent<QuestManager>().GetCurrentQuest().data.questName);
            Debug.Log(other.GetComponent<QuestManager>().GetCurrentQuest().GetType().ToString());
            
        
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



}
