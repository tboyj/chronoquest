using UnityEngine;

public class MoveNode : AfterQuestDialog
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private Vector3 goTo;
    public override void SetChange()
    {
        obj.transform.position = goTo;
    }
}