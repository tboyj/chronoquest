using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderNode : MonoBehaviour
{
    public bool wasVisited = false;

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
        if (other.CompareTag("NPC"))
        {
            wasVisited = true;
        }
    }
}
