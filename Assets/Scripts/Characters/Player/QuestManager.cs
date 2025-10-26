using System;
using System.Collections.Generic;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public List<QuestInstance> questsAssigned = new List<QuestInstance>();
    public List<QuestInstance> questsCompleted = new List<QuestInstance>();
    public int currentQuestId;
    private bool currentlyInDialog;

    public void Start()
    {
        currentQuestId = 3; // replace with savedata
        if (questsCompleted.Count > 0)
        {
            if (GetCurrentQuest() != null)
            {
                currentQuestId = GetCurrentQuest().data.id;
            }
            else
            {
                Debug.Log("Can't tell you yet: [INSERT QUEST ID]");
            }
        }
    }
    public void AddQuestToList(QuestInstance quest)
    {
        questsAssigned.Add(quest);
        currentQuestId = GetCurrentQuest().data.id;
        gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }
    public void SetQuestCompleted(QuestInstance quest)
    {
        questsCompleted.Add(quest);
        questsAssigned.Remove(quest);
        gameObject.GetComponent<QuestManagerGUI>().RefreshQuestGUI();
    }

    public bool PrecheckQuest(QuestInstance questAssigned)
    {
        return questAssigned.CheckConditions();
    }

    public QuestInstance GetCurrentQuest()
    {
        if (questsAssigned.Count <= 0)
            return null;
        else
            return questsAssigned[0];
    }
    public bool CurrentlyInDialog()
    {
        return currentlyInDialog;
    }

    public void SetCurrentlyInDialog(bool v)
    {
        currentlyInDialog = v;
    }
    public void TryToGiveQuest(NPC interactableNPC, DialogGUIManager dialogManager)
    {
        QuestHandler npcQuestHandler = interactableNPC.GetComponent<QuestHandler>();
        QuestInstance questAssigned = npcQuestHandler.GetMostRecentQuest();
        if (questAssigned != null)
        {
            if (GetCurrentQuest() != null)
            {
                Debug.Log($"CurrentQuest ID: {GetCurrentQuest()?.GetQuestID()}");
                Debug.Log($"AssignedQuest ID: {questAssigned?.GetQuestID()}");


                if (GetCurrentQuest().GetQuestID() == questAssigned.GetQuestID())
                {
                    if (GetCurrentQuest().IsCompleted == false) // make sure he doesn't have it already;
                    { // quest is assigned but not done.
                        Debug.Log("Not complete.");
                    }

                    if (GetCurrentQuest().IsCompleted &&
                    questAssigned.IsCompleted)
                    { // quest is assigned and done.
                        if (GetCurrentQuest().relatedQuests.Count > 0)
                        {
                            foreach (QuestInstance relatedQuest in GetCurrentQuest().relatedQuests)
                            {
                                if (!relatedQuest.IsCompleted)
                                {
                                    Debug.Log("Not completed.");
                                    return;
                                }
                            }
                        }
                        // Specific checks for quest types.
                        QuestForker(questAssigned, interactableNPC);


                        Debug.Log("Good Job");
                        if (GetCurrentQuest().postQuestList.Count > 0)
                        {
                            Debug.Log(GetCurrentQuest().data.name);
                        }
                        // redundancy, might want to fix later.

                        SetQuestCompleted(GetCurrentQuest());
                        npcQuestHandler.questsInStock.RemoveAt(0);
                        // throw into dialog gui here.
                    }
                }

                else
                {
                    Debug.Log("Not the same quest.");
                }

            }
            Debug.Log(dialogManager.GetPrint());
            if (questsAssigned.Count == 0 && npcQuestHandler.questsInStock.Count > 0)
            { // add since there is none in quest.
                questAssigned = npcQuestHandler.GetMostRecentQuest();
                Debug.Log($"Adding quest {questAssigned.data.questName} with id {questAssigned.data.id}");
                Debug.Log($"Conditions are {questAssigned.CheckConditions()}.");
                if (questAssigned.CheckConditions())
                {
                    Debug.Log("Ignoring conditions |>");
                    SetQuestCompleted(GetCurrentQuest());
                    npcQuestHandler.questsInStock.RemoveAt(0);
                    // Throw here dialog saying good job.
                    Debug.Log("Dialog for congratulating them");
                    TryToGiveQuest(interactableNPC, dialogManager);
                    interactableNPC.questHandler.GetMostRecentQuest().QuestEventTriggered();
                    dialogManager.SetCharName(GetCurrentQuest().dialogsForQuest[0].characterName);
                    dialogManager.SetDialText(GetCurrentQuest().dialogsForQuest[0].dialogueText);
                }

                else
                {
                    AddQuestToList(questAssigned);
                    // Throw him into a dialog.
                    if (GetCurrentQuest().dialogsForQuest.Count > 0)
                    {
                        GetCurrentQuest().ShowDialog(true);
                        SetCurrentlyInDialog(true);
                        interactableNPC.inDialog = true;
                        dialogManager.SetCharName(GetCurrentQuest().dialogsForQuest[0].characterName);
                        dialogManager.SetDialText(GetCurrentQuest().dialogsForQuest[0].dialogueText);
                    }
                    else
                    {
                        GetCurrentQuest().ShowDialog(false);
                        SetCurrentlyInDialog(false);
                        interactableNPC.inDialog = false;
                    }
                }
            }
            else if (questsAssigned.Count > 0 && npcQuestHandler.questsInStock.Count > 0)
            {
                if (questAssigned is TalkToNPCQuest && questAssigned.data.id == GetCurrentQuest().data.id)
                {
                    Debug.Log("IDs match and its ready to go.");
                }
                else
                {
                    Debug.Log("Quest Assigned in NPC: " + questAssigned.data.id + " Actual: " + GetCurrentQuest().data.id);
                }
                Debug.Log("Can't assign Quest, One in progress already.");
                // hint sys goes here
                // }
            }

            else
            {
                Debug.Log("No quest in stock.");
            }

        }

        else
        {
            Debug.Log("Do a general dialog");
        }
    }
    
    public void ChangeFunction()
    {
        if (GetCurrentQuest()?.postQuestList == null)
        {
            Debug.LogWarning("postQuestList is null!");
            return;
        }
        else
        {
            foreach (AfterQuestDialog change in GetCurrentQuest().postQuestList)
            {
                change.SetChange();
            }

        }
    }

    /** QUEST FORKER - DEFINES QUEST HANDLING BEFORE THEY GO INTO THEIR FINAL SCRIPT CHECKS.
        put in functions later. (or a script)
    **/

    private void QuestForker(QuestInstance questAssigned, NPC interactableNPC)
    {
        if (questAssigned is QuestCollectItem quest)
        {
            if (quest.isGiveQuestType)
            {
                // COLLECT ITEM QUEST
                foreach (Item item in gameObject.GetComponent<Player>().inventory.items) // use ToList() to avoid modifying while iterating
                {
                    if (item.item != null &&
                        item.item.itemName == quest.requiredItem.itemName &&
                        item.quantity > 0)
                    {
                        Debug.Log("Item found.");
                        Debug.Log("Index: " + gameObject.GetComponent<Player>().inventory.items.IndexOf(item));

                        int transferAmount = Mathf.Min(item.quantity, quest.requiredCount);

                        // Give item(s) to NPC
                        interactableNPC.inventory.AddItem(new Item(item.item, transferAmount));

                        // Subtract from player
                        item.quantity -= transferAmount;

                        if (item.quantity <= 0)
                        {
                            int index = gameObject.GetComponent<Player>().inventory.items.IndexOf(item);
                            gameObject.GetComponent<Player>().inventory.items[index] = new Item(null, 0);
                        }

                        break; // stop after transferring
                    }
                }
            }
        }
            // TALKTONPCQUEST
            questAssigned.QuestEventTriggered();
        
    }
}