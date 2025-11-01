
using UnityEngine;

public class NPCChangeMovementStatus : AfterQuestDialog
{
    public NPCMovement npc;
    public string desiredStatus;
    public Vector3 moveLocation;
    public override void SetChange()
    {
        Debug.Log($"[NPCChangeMovementStatus] Setting status to {desiredStatus} on {npc?.name}");
        if (npc != null)
        {
            npc.status = desiredStatus;
            npc.endNode.position = moveLocation;
        }

    }
}