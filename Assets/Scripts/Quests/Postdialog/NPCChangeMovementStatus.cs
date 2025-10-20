using UnityEngine;

public class NPCChangeMovementStatus : AfterQuestDialog
{
    public NPCMovement npc;
    public string desiredStatus;
    public override void SetChange()
    {
        Debug.Log($"[NPCChangeMovementStatus] Setting status to {desiredStatus} on {npc?.name}");
        if (npc != null)
        {
            npc.status = desiredStatus;
        }

    }
}