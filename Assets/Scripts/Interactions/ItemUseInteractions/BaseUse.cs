using System;
using ChronoQuest.UIForInteractions;
using UnityEngine;

public abstract class BaseUse : MonoBehaviour, IAvailableActions
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
            ChangeTheUI("");
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
    public void ChangeTheUI(string str)
    {
        if (GetPlayer() != null)
            GetPlayer().interactionPanel.text = str;
    }

    public void ChangeTheUI(Item item)
    {
        throw new NotImplementedException();
    }

}
