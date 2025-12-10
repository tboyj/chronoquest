using UnityEngine;
using ChronoQuest.Quests;
using System.Collections.Generic;
using System;

[System.Serializable]
public class TalkToNPCQuest : QuestInstance, IQuestAction
{
    public NPC npc;
    public NPC questAssignerNPC;
    
    public override void Start()
    {
        base.Start();

        // Try to assign NPC if null
        if (npc == null)
        {
            npc = gameObject.GetComponent<NPC>();
            if (npc == null)
            {
                Debug.LogWarning($"NPC reference is null on {gameObject.name}. Assign questAssignerNPC in inspector!");
            }
            else
            {
                Debug.Log($"NPC automatically assigned from {gameObject.name}");
            }
        }
    }

    public override void QuestEventTriggered()
    {
        // This should be called when dialog is finished or conditions are met
        Debug.Log($"QuestEventTriggered for {data.questName}");
        
        if (CheckConditions())
        {
            IsCompleted = true;
            Debug.Log($"Quest {data.questName} completed!");
        }
        else
        {
            ShowDialog(false);
            if (npc != null)
            {
                npc.inDialog = false;
            }
            if (questManager != null)
            {
                questManager.gameObject.GetComponent<Player>().inDialog = false;
            }
        }
    }

    public override bool CheckConditions()
    {
        // ONLY check conditions, don't modify quest state
        Debug.Log($"Checking conditions for {data.questName}");
        
        if (questManager == null)
        {
            Debug.LogWarning("QuestManager is null");
            return false;
        }
        
        QuestInstance currentQuest = questManager.GetCurrentQuest();
        if (currentQuest == null)
        {
            Debug.Log("No current quest assigned");
            return false;
        }
        
        if (questAssignerNPC == null || questAssignerNPC.questHandler == null)
        {
            Debug.LogWarning("Quest assigner NPC or quest handler is null");
            return false;
        }
        
        QuestInstance npcQuest = questAssignerNPC.questHandler.GetMostRecentQuest();
        if (npcQuest == null)
        {
            Debug.Log("NPC has no quest in stock");
            return false;
        }
        
        // Check if this is the active quest
        if (currentQuest.data.id != data.id)
        {
            Debug.Log($"Quest ID mismatch: Current={currentQuest.data.id}, This={data.id}");
            return false;
        }
        
        // Check if BOTH NPCs are in range with the player
        bool npcInRange = npc != null && npc.GetInRange();
        bool questAssignerInRange = questAssignerNPC != null && questAssignerNPC.GetInRange();
        
        if (npcInRange && questAssignerInRange)
        {
            Debug.Log($"Quest {data.questName} conditions met - Both NPCs in range");
            return true;
        }
        else
        {
            Debug.Log($"NPCs not in range - Target NPC: {npcInRange}, Quest Assigner: {questAssignerInRange}");
            return false;
        }
    }
}