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
        if (!pauseCheck.isInventory && !pauseCheck.isPaused && Input.GetKeyUp(Keybinds.actionKeybind))
        {
            InteractionFunction();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            referencePlayer = other.gameObject;
            if (referencePlayer.GetComponent<QuestManager>().GetCurrentQuest() is TalkToNPCQuest)
                quest = referencePlayer.GetComponent<QuestManager>().GetCurrentQuest() as TalkToNPCQuest;
            qm = other.GetComponent<QuestManager>();
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            referencePlayer = null;
            quest = null;
            qm = null;
        }
    }
    public void InteractionFunction()
    {

        if (referencePlayer != null)
        {
            if (qm.GetCurrentQuest() != null)
            {
                if (!inDialog) // quest?.data?.id == qm.GetCurrentQuest()?.data?.id (used to be here, but it's not necessary rn lol)
                {
                    Debug.Log("Moves to here.");
                    quest.QuestEventTriggered();
                }
                else
                {
                    if (inDialog)
                        Debug.Log("In dialog. Don't try.");
                    Debug.Log("Inequal id. Please check..." + quest.data.id + ":" + qm.GetCurrentQuest().data.id);
                    // try to do a general dialog ?????
                }
            }
        }
    }   
}

