using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> playerQuests = new List<Quest>();
    public List<Quest> playerQuestsCompleted = new List<Quest>();
    public Quest currentQuest;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questResponsibilitiesText;

    void Start()
    {
        questNameText.text = "";
        questResponsibilitiesText.text = "";
    }

    // Update is called once per frame

    public void CheckUpOnQuest(int id, QuestGiver giver)
    {
        switch (id)
        {
            case 0:
                CollectItemQuest questVariable = giver.CollectItem();
                questVariable.SetQuestName("Gather 5 Pakets");
                if (!playerQuests.Contains(questVariable))
                {
                    playerQuests.Add(questVariable);
                    currentQuest = playerQuests[playerQuests.IndexOf(questVariable)];
                    Debug.Log("Quest added: " + playerQuests[playerQuests.IndexOf(questVariable)]);
                    UpdateQuestGui(playerQuests.IndexOf(questVariable));
                }
                else
                {
                    if (!questVariable.isCompleted)
                    {
                        {
                            if (currentQuest == questVariable)
                                Debug.Log("Already in progress. Please work on: " + playerQuests[playerQuests.IndexOf(questVariable)]);
                            else // if (currentQuest != questVariable)
                            {
                                currentQuest = questVariable;

                                Debug.Log("Assigned.");
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Good Job!");
                        UpdateQuestGui(playerQuests.IndexOf(questVariable));
                    }
                }
                break;
        }
    }

    public void UpdateQuestGui(int indexOfQuest)
    {
        Quest baseQuest = playerQuests[indexOfQuest];
        Debug.Log(baseQuest.questName+" Is the baseQuests name");
        if (baseQuest is CollectItemQuest collectQuest)
        {
            questNameText.text = collectQuest.questName+"";
            questResponsibilitiesText.text =
            "Collect " + collectQuest.itemsCollected + "/" + collectQuest.requiredAmount + " " + collectQuest.itemNeeded.itemName;


            // Use collectQuest directly
        }
    }

    void PrintOutData()
    {
        
    }
}

