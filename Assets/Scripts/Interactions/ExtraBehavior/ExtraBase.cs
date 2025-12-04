using System;
using UnityEngine;

public abstract class ExtraBase : MonoBehaviour
{
    // Called when the script instance is being loaded
    public bool inRange = false;
    public Player player;
    public QuestManager qm;
    // Called before the first frame update
    public void Start()
    {
        
        GameObject rp = GameObject.Find("RealPlayer");
        if (rp != null)
        {
            player = rp.GetComponent<Player>();
            qm = rp.GetComponent<QuestManager>();
        }
        else
        {
            Debug.LogError("RealPlayer not found!");
        }
    }
    private void Update()
    {
        
    }
    // Example method for using the paper item
    public abstract void Change();
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            SetPlayer(other.gameObject.GetComponent<Player>());
            SetManager(other.gameObject.GetComponent<QuestManager>());
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
    public void SetManager(QuestManager qm)
    {
        this.qm = qm;
    }
    public QuestManager GetManager()
    {
        return qm;
    }
}
