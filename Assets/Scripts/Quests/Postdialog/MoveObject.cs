using UnityEngine;

public class MoveObject : AfterQuestDialog
{
    public GameObject obj;
    public Vector3 startPosition;
    public void Start()
    {
        startPosition = obj.transform.position;
    }
    public override void SetChange()
    {
        if (obj.transform.position == startPosition)
        {
            obj.transform.position = new Vector3(0, -100, 0);
        }
    }
}