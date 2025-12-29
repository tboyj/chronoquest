using System;
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
    [SerializeField]
    private AudioSource walkSFX;

    private float footstepTimer;
    private float footstepInterval = 0.4f;
    public void Awake()
    {
        
        walkSFX = transform.Find("AudioSources/Walk").GetComponent<AudioSource>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing on NPC.");
            return;
        }

        // Sync moveSpeed with base class
        agent.speed = 2;
        agent.angularSpeed = 120f;
        agent.acceleration = 2;
        agent.stoppingDistance = 0.1f;
        agent.updateRotation = false; // We'll handle flipping manually
    }
    
    private void Update()
    {

        // Flip NPC based on movement direction

        if (!agent.isStopped)
        {
            walkSFX.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            walkSFX.volume = agent.velocity.magnitude * 0.5f; 
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval && agent.velocity.magnitude > 0f) {
                walkSFX.Play();
                footstepTimer = 0f; // Reset timer
            }
        }
        else
        {
            
        }


        // Debug.Log("Remaining: " + agent.remainingDistance);
        // Move agent along path
        if (status == "QUEST" || status == "MOVING")
        {
            MoveToDestination(endNode);
        }
        else if (status == "WANDER")
        {
            WanderToRandom();
        }
        // if (agent.remainingDistance > agent.stoppingDistance)
        // {
            
        // }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            status = "IDLE";
        } else
        {
             status = "MOVING";
        }

    }

    private void WanderToRandom()
    {
        throw new NotImplementedException(); // not done

    }

    public void MoveToDestination(Transform target)
    {

        endNode = target;
        agent.SetDestination(endNode.position);
        status = "QUEST";
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
    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}
