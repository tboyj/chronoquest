using Unity.VisualScripting;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public int id;
    public string questName;
    public string description;
    public bool isCompleted = false;

    public abstract void Initialize();

    public abstract void CheckProgress();
    public virtual void Update()
    {
        
    }
}