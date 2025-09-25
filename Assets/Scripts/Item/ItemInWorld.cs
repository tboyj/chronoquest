using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public int amountOfItemsHere;
    public ItemStorable itemInWorld;
    public SpriteRenderer rend;
    public bool takeable;
    private bool itemRecognizesPlayer;
    public GameObject referencePlayer;

    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (itemRecognizesPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E) && takeable)
            {
                amountOfItemsHere--;
                QuestManager manager = referencePlayer.GetComponent<QuestManager>();
                    if (manager.currentQuest is CollectItemQuest collectItemQuest)
                    {
                        if (itemInWorld == collectItemQuest.itemNeeded)
                        {
                            collectItemQuest.ReportItemCollected(1);
                            manager.UpdateQuestGui(manager.playerQuests.IndexOf(manager.currentQuest));
                        }
                    }
                if (amountOfItemsHere == 0)
                {
                    rend.enabled = false;
                    takeable = false;


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
}
