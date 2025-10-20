using System;
using UnityEngine;

public abstract class ExtraBase : MonoBehaviour
{
    // Called when the script instance is being loaded
    public bool inRange = false;
    public Player player;
    public QuestManager qm;
    // Called before the first frame update
    private void Start()
    {
        // Setup code here
        qm = player.gameObject.GetComponent<QuestManager>();
    }
    private void Update()
    {
        // Initialization code here
    }
    // Example method for using the paper item
    public abstract void Change();
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
            SetPlayer(null);
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
