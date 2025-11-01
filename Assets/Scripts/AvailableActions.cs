using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableActions : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Player playerInRange;
    public bool isInRange;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    // yes i know i reuse this a lot and yes i know it could probably be refactored
    // i'll do that in other games
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("[TRIGGER EVENT / NPC] Trigger entered by: " + other.name + " Tag: " + other.tag);
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            playerInRange = other.GetComponent<Player>();
            isInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            playerInRange = other.GetComponent<Player>();
            isInRange = true;
        }
    }
}
