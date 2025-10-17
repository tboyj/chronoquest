using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ChronoQuest.Interactions.World;
using UnityEngine.Events;

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
    public bool questRequired;
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
                if (gameObject?.GetComponent<ExtraBase>())
                {
                    gameObject?.GetComponent<ExtraBase>().Change();
                }
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
            if (questRequired)
            {
                if (quest != null)
                    quest.QuestEventTriggered();
                else
                {
                    Debug.LogWarning("No quest found for this interaction.");
                }
            }
            else
            {
                //referencePlayer.GetComponent<Inventory>().AddItem(new Item(itemInWorld, 1));
            }

            // causes error
            //OnInteractEvent?.Invoke(referencePlayer);
            if (amountOfItemsHere == 0)
            {
                if (rend != null)
                    rend.enabled = false;
                takeable = false;


            }
        }
    }
}
