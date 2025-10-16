using UnityEngine;

public abstract class BaseUse : MonoBehaviour
{
    // Called when the script instance is being loaded
    public bool inRange = false;
    // Called before the first frame update
    private void Start()
    {
        // Setup code here
    }
    private void Update()
    {
        // Initialization code here
    }
    // Example method for using the paper item
    public abstract void Use();
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
