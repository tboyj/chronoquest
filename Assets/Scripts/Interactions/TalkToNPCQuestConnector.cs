using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ChronoQuest.Interactions.World;

public class TalkToNPCQuestConnector : MonoBehaviour, Interaction
{
    // Start is called before the first frame update

    public GameObject referencePlayer;
    public TalkToNPCQuest quest;
    public PauseScript pauseCheck;
    public QuestManager qm;
    public bool inDialog { get; set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseCheck.isInventory && !pauseCheck.isPaused && Input.GetKeyDown(Keybinds.talkKeybind))
        {
            InteractionFunction();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            referencePlayer = other.gameObject;
            quest = referencePlayer.GetComponent<QuestManager>().GetCurrentQuest() as TalkToNPCQuest;
            qm = referencePlayer.GetComponent<QuestManager>();
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            referencePlayer = null;
        }
    }
    public void InteractionFunction()
{
    if (quest != null && referencePlayer != null)
    {
        if (quest.data.id == referencePlayer.GetComponent<QuestManager>().GetCurrentQuest().data.id)
        {
            Debug.Log("Moves to here.");
            quest.QuestEventTriggered();
        }
    }
}
}

