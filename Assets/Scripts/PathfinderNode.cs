using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && other.GetComponent<NPCMovement>().GetCurrentNode() == this.transform)
        {
            Debug.Log("NPC entered node");
            gameObject.SetActive(false);
        }
    }
}
