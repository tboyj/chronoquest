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
    public bool inDialog { get; set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseCheck.isInventory && !pauseCheck.isPaused)
        {
            if (Input.GetKeyUp(Keybinds.talkKeybind))
            {
                Debug.Log("test???");
                InteractionFunction();
            }
            else
            {
                Debug.Log("It's deeper than this");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            referencePlayer = other.gameObject;
            quest = referencePlayer.GetComponent<QuestManager>().GetCurrentQuest() as TalkToNPCQuest;
        }
    }
    // void OnTriggerExit(Collider other)
    // {

    //     if (other.CompareTag("Player"))
    //     {
    //         referencePlayer = null;
    //     }
    // }
    public void InteractionFunction()
    {            
        quest.QuestEventTriggered(); // causes error
    }
    
}

