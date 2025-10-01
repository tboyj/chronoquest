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
    public void InteractionFunction() // Add logic here
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && quest.GetQuestID() == playerQuest.GetQuestID())
        {
            toggled = !toggled;
            quest.QuestEventTriggered();
            LeverCheck();
        }
        else if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
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
        {
            Debug.Log(other.GetComponent<QuestManager>().GetCurrentQuest().data.questName);
            Debug.Log(other.GetComponent<QuestManager>().GetCurrentQuest().GetType().ToString());
            playerQuest = other.GetComponent<QuestManager>().GetCurrentQuest() as QuestToggleItem;
        
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
