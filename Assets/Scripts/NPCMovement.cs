using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform endNode;
    public string status;
    [SerializeField]
    private NavMeshAgent agent;

    public void Awake()
    {
        
        
        
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing on NPC.");
            return;
        }

        // Sync moveSpeed with base class
        agent.speed = 2;
        agent.angularSpeed = 120f;
        agent.acceleration = 2;
        agent.stoppingDistance = 0.5f;
        agent.updateRotation = false; // We'll handle flipping manually
    }

    private void Update()
    {

        // Flip NPC based on movement direction
        
        


<<<<<<< HEAD
        // Debug.Log("Remaining: " + agent.remainingDistance);
=======
        Debug.Log("Remaining: " + agent.remainingDistance);
>>>>>>> a3506ef9b943eb0ffe2936a110a11d435742425c
        // Move agent along path
        MoveToTarget(endNode);
        // if (agent.remainingDistance > agent.stoppingDistance)
        // {
            
        // }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            status = "IDLE";
        }

    }


    public void MoveToTarget(Transform target)
    {

        endNode = target;
        agent.SetDestination(endNode.position);
        status = "MOVING";
    }

    public void StopMovement()
    {
        if (agent == null) return;
        agent.ResetPath();
        status = "IDLE";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            StopMovement();
        }
    }
}
