using System;
using UnityEngine;

public abstract class BaseUse : MonoBehaviour
{
    // Called when the script instance is being loaded
    public bool inRange = false;
    private Player player;
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
            SetPlayer(other.gameObject.GetComponent<Player>());
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
    
    public void SetPlayer(Player player)
    {
        this.player = player;
    }
    public Player GetPlayer()
    {
        return player;
    }
}
