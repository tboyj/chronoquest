using UnityEngine;

public class TodoObject : MonoBehaviour
{
    public Transform accessPoint;
    public string todoText;

    public void Start()
    {
        accessPoint = gameObject.transform;
    }
    public void Update()
    {
        accessPoint = gameObject.transform;
    }
    public Transform GetTodoAp() // gets access point (pointer for todo)
    {
        return accessPoint;
    }
}