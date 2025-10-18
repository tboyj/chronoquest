public class NPCChangeMovementStatus : AfterQuestDialog
{
    public NPCMovement npcmov;
    public string desiredStatus;
    public override void SetChange()
    {
        npcmov.status = desiredStatus;
    }
}