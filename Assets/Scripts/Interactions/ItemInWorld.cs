using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ChronoQuest.Interactions.World;

public class ItemInWorld : MonoBehaviour, Interaction
{
    // Start is called before the first frame update
    public int amountOfItemsHere;
    public ItemStorable itemInWorld;
    public SpriteRenderer rend;
    public bool takeable;
    private bool itemRecognizesPlayer;
    public GameObject referencePlayer;
    public QuestCollectItem quest;
    public PauseScript pauseCheck;
    public bool inDialog { get; set; }
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
                
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            referencePlayer = other.gameObject;
            itemRecognizesPlayer = true;
            quest = referencePlayer.GetComponent<QuestManager>().GetCurrentQuest() as QuestCollectItem;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            referencePlayer = null;
            itemRecognizesPlayer = false;
        }
    }
    private void Startup()
    {
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
            amountOfItemsHere--;
            quest.QuestEventTriggered(); // causes error
            if (amountOfItemsHere == 0)
            {
                rend.enabled = false;
                takeable = false;


            }
        }
    }
}
