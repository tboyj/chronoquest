using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuestManager : MonoBehaviour
{
    public List<QuestInstance> questsAssigned = new List<QuestInstance>();
    public List<QuestInstance> questsCompleted = new List<QuestInstance>();
    private bool currentlyInDialog;
    public bool hasCompletedFirstQuest;
    private NPC currentNPC;
    
    public int generalDialogCounter;

    public void SetCurrentNPC(NPC npc)
    {
        currentNPC = npc;
    }

    public NPC GetCurrentNPC()
    {
        return currentNPC;
    }

    public void Start()
    {   
        if (GetCurrentQuest() != null)
        {
            Debug.Log("attempt @ cqidm");
            CurrentQIDMonitor.Instance.SetCurrentId(GetCurrentQuest().data.id);
            
            // ADD: Re-initialize conditions for all assigned quests
            foreach (var quest in questsAssigned)
            {
                if (quest != null)
                {
                    quest.ReinitializeConditions();
                }
            }
            
            // ADD: Force update all NPCs to sync with current quest state
            StartCoroutine(SyncNPCsWithQuestState());
        }
        else
        {
            Debug.Log("Can't tell you yet: [INSERT QUEST ID]");
        }
        
        hasCompletedFirstQuest = false;
    }

    // ADD this new method
    public System.Collections.IEnumerator SyncNPCsWithQuestState()
    {
        // Wait one frame for all NPCs to finish their Start()
        yield return null;
        
        // Find all NPCs and remove completed quests from their stock
        NPC[] allNPCs = FindObjectsOfType<NPC>();
        foreach (NPC npc in allNPCs)
        {
            QuestHandler handler = npc.GetComponent<QuestHandler>();
            if (handler != null)
            {
                handler.questsInStock.RemoveAll(quest => 
                    quest != null && 
                    questsCompleted.Exists(completed => 
                        completed != null && completed.data.id == quest.data.id
                    )
                );
            }
        }
        
        Debug.Log("NPCs synced with quest state");
    }
    public void AddQuestToList(QuestInstance quest)
    {
        questsAssigned.Add(quest);
        CurrentQIDMonitor.Instance.SetCurrentId(quest.data.id);
        GetComponent<QuestManagerGUI>().RefreshQuestGUI();
        // set as false in editor
        hasCompletedFirstQuest = true;
    }
    public void SetQuestCompleted(QuestInstance quest)
    {
        if (quest == null)
        {
            Debug.LogError("Cannot complete a null quest.");
            return;
        }
        
        Debug.Log($"Completing quest {quest.data.id}");
        
        questsCompleted.Add(quest);
        quest.IsCompleted = true;
        questsAssigned.Remove(quest);
        
        // Update to next quest
        QuestInstance newCurrentQuest = GetCurrentQuest();
        if (newCurrentQuest != null)
        {
            CurrentQIDMonitor.Instance.SetCurrentId(newCurrentQuest.data.id);
            Debug.Log($"New current quest: {newCurrentQuest.data.id}");
        }
        else
        {
            CurrentQIDMonitor.Instance.SetCurrentId(0);
            Debug.Log("No more quests");
        }
        
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

                if (GetCurrentQuest().IsCompleted == false) // make sure he doesn't have it already;
                { // quest is assigned but not done.
                  // i have to check if it's a talking script you know? cuz its flimsy?
                  generalDialogCounter = 0;
                    if (GetCurrentQuest() is TalkToNPCQuest talkToNPC)
                    {
                        if (talkToNPC.npc == interactableNPC && GetCurrentQuest().data.id == talkToNPC.GetQuestID())
                        {
                            SetQuestCompleted(GetCurrentQuest());
                            TryToGiveQuest(interactableNPC, dialogManager); // recall if so;
                            Debug.Log("You, sir have won the internet today! (Completed.)");
                        }
                    }
                    else // if not this exception... not complete.
                    {
                        
                        Debug.Log("Not complete.");
                        
                        if (GetCurrentQuest().todo[0].dialogsForQuestSections.Count > 0) {
                            SetCurrentlyInDialog(true);
                            interactableNPC.inDialog = true;
                            dialogManager.SetCharName(GetCurrentQuest().todo[generalDialogCounter].GetCharName(generalDialogCounter));
                            dialogManager.SetDialText(GetCurrentQuest().todo[generalDialogCounter].GetCharText(generalDialogCounter));
                        }
                        
                    }
                }
                // completes GetCurrentQuest if it has most recent quest behind it and if it is ready to go.
                // at least thats what i hope it does. :grimace:

                // system:

                // assign a talk to quest with id 5 (example id)
                // assign quest that is given by npc to talk to with same id (eg. 5)
                // further it along when completed with that


                if (GetCurrentQuest().GetQuestID() == questAssigned.GetQuestID())
                {
                    if (GetCurrentQuest().IsCompleted &&
                    questAssigned.IsCompleted)
                    { // quest is assigned and done.
                        if (GetCurrentQuest().relatedQuests.Count > 0)
                        {
                            foreach (QuestInstance relatedQuest in GetCurrentQuest().relatedQuests)
                            {
                                Debug.Log(relatedQuest.IsCompleted + ": completion of related quests");
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
                            ChangeFunction();
                        }
                        // redundancy, might want to fix later.

                        SetQuestCompleted(GetCurrentQuest());
                        npcQuestHandler.questsInStock.RemoveAt(0);
                        // throw into dialog gui here.
                        generalDialogCounter = 0;
                    }
                }

                else
                {
                    Debug.Log("Not the same quest.");
                }

            }
            Debug.Log(dialogManager.GetPrint());
            if (questsAssigned.Count == 0 && npcQuestHandler.questsInStock.Count > 0)
            {
                if (questsCompleted.Contains(GetCurrentQuest()))  // This will also fail since GetCurrentQuest is null
                {
                    Debug.Log("You already did this! I'm Ignoring You. BLOCKED!");
                }
                else
                {
                    questAssigned = npcQuestHandler.GetMostRecentQuest();
                    
                    if (questAssigned != null)
                    {
                        Debug.Log($"Adding quest {questAssigned.data.questName} with id {questAssigned.data.id}");
                        Debug.Log($"Conditions are {questAssigned.CheckConditions()}.");
                        
                        if (questAssigned.CheckConditions())
                        {
                            Debug.Log("Ignoring conditions |>");
                            
                            // FIX: Don't try to complete GetCurrentQuest() when questsAssigned is empty!
                            // Just mark this quest as complete and move on
                            questAssigned.IsCompleted = true;
                            questsCompleted.Add(questAssigned);
                            npcQuestHandler.questsInStock.RemoveAt(0);
                            
                            Debug.Log("Quest auto-completed, trying next quest");
                            TryToGiveQuest(interactableNPC, dialogManager);
                        }
                        else
                        {
                            // Normal flow - add the quest
                            AddQuestToList(questAssigned);
                            
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
                }
            }
            else if (questsAssigned.Count > 0 && npcQuestHandler.questsInStock.Count > 0)
            {
                if (!questsCompleted.Contains(GetCurrentQuest())) {
                    if (questAssigned.data.id == GetCurrentQuest().data.id)
                    {
                        Debug.Log("IDs match and its ready to go.");
                    }
                    else
                    {
                        // Check if not same npc.
                        Debug.Log("Quest Assigned in NPC: " + questAssigned.data.id + " Actual: " + GetCurrentQuest().data.id);
                    }
                    Debug.Log("Can't assign Quest, One in progress already.");
                    // hint sys goes here
                    // }
                }
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

                        var a = gameObject.GetComponent<Player>().GetHeldItem();
                        var b = gameObject.GetComponent<HoldingItemScript>();

                        if (a.item != null)
                        {
                            if (a.item.sprite == b.spriteTopLeftImage.sprite && a.item.id == item.item.id)
                            {
                                gameObject.GetComponent<Player>().isHolding = false;
                                gameObject.GetComponent<HoldingItemScript>().Activate(false);
                            }
                        }

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
        } else {
            // TALKTONPCQUEST
            questAssigned.QuestEventTriggered();
        }
        
    }
}