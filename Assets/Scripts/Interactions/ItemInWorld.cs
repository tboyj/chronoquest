using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ChronoQuest.Interactions.World;
using UnityEngine.Events;
using System;
using ChronoQuest.UIForInteractions;

public class ItemInWorld : MonoBehaviour, Interaction, IAvailableActions
{
    // Start is called before the first frame update
    public int amountOfItemsHere;
    public ItemStorable itemInWorld;
    public SpriteRenderer rend;
    public bool takeable;
    public bool itemRecognizesPlayer;
    public GameObject referencePlayer;
    public QuestCollectItem quest;
    public PauseScript pauseCheck;
    public bool inDialog { get; set; }
    public bool questRequired;
    public Player player;
    //public UnityEvent<GameObject> OnInteractEvent;
    void Start()
    {
        
        Startup();
        quest.PrecheckInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemRecognizesPlayer)
        {
            if (Input.GetKeyDown(Keybinds.actionKeybind) && !inDialog && !pauseCheck.isInventory && !pauseCheck.isPaused)
            {
                InteractionFunction();
                Debug.Log("I found you...");
                if (gameObject?.GetComponent<AssignNewQuest>())
                    gameObject.GetComponent<AssignNewQuest>().Change();
                 // make sure to have specific class type. no generals.
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            referencePlayer = other.gameObject;
            player = referencePlayer.GetComponent<Player>();
            itemRecognizesPlayer = true;
            quest = referencePlayer.GetComponent<QuestManager>().GetCurrentQuest() as QuestCollectItem;
            ChangeTheUI("[F] Take " + itemInWorld.itemName);

        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemRecognizesPlayer = true;
            ChangeTheUI("[F] Take " + itemInWorld.itemName);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemRecognizesPlayer = false;
            referencePlayer = null;
            ChangeTheUI("");
        }
    }
    private void Startup()
    {
        pauseCheck = GameObject.Find("RealPlayer").GetComponent<PauseScript>();
        rend.sprite = itemInWorld.sprite;
        if (amountOfItemsHere > 0)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
            takeable = false;
        }
    }
    public void InteractionFunction()
    {
        if (takeable)
        {
            if (gameObject.GetComponent<ChestChangeActivity>() != null) // activate extra behavior if there is any
            {
                gameObject.GetComponent<ChestChangeActivity>().Change();
            }
            
            if (quest != null) { 
                quest.QuestEventTriggered();
                amountOfItemsHere--;

            }
            else
            {
                Debug.LogWarning("No quest found for this interaction.");
            }

            // causes error
            //OnInteractEvent?.Invoke(referencePlayer);
            if (amountOfItemsHere <= 0)
            {
                if (rend != null)
                    rend.enabled = false;
                takeable = false;
                if (gameObject.GetComponent<AssignNewQuest>() != null) // activate extra behavior if there is any
                {
                    gameObject.GetComponent<AssignNewQuest>().Change();
                }
            }
        }
    }

    public void ChangeTheUI(Item heldItem)
    {
        ChangeTheUI("");
    }

    public void ChangeTheUI(string str)
    {
        player.interactionPanel.text = str;
    }

}
