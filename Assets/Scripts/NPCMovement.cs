
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMovement : Movement
{
    public string status;
    public GameObject pathPointContainer;
    public List<Transform> pathPoints = new List<Transform>();
    public Transform currentNode;

    private float minDist = Mathf.Infinity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("CharacterController missing on NPC.");

        pathPointContainer = GameObject.Find("AINodeHolder");

        foreach (Transform child in pathPointContainer.transform)
            pathPoints.Add(child);

        currentNode = GetClosestNode();
    }

    private void Update()
    {
        if (status != "Idle")
            MoveWithForce();
    }

    public override void MoveWithForce()
    {
        if (Time.timeScale > 0)
        {
            if (currentNode == null) return;

            Vector3 dir = (currentNode.position - transform.position).normalized;
            Vector3 move = new Vector3(dir.x, 0, dir.z) * moveSpeed * Time.deltaTime;

            controller.Move(move);

            // Apply gravity
            if (!controller.isGrounded)
                velocity.y += gravity * Time.deltaTime;
            else
                velocity.y = -2f;

            // Smooth acceleration
            Vector3 directionOfNPC = transform.TransformDirection(dir).normalized;

            if (directionOfNPC.magnitude > 0.1f)
            {
                velocity = Vector3.Lerp(velocity, directionOfNPC * moveSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            controller.Move(velocity * Time.deltaTime);

            // Flip NPC
            if (dir.x < 0) flip = true;
            else if (dir.x > 0) flip = false;

            float dist = Vector3.Distance(transform.position, currentNode.position);
            if (dist < 0.5f)
            {
                currentNode.GetComponent<PathfinderNode>().wasVisited = true;
                currentNode = GetClosestNode();
                minDist = Mathf.Infinity;
            }
        }
    }

    public Transform GetClosestNode()
    {
        Transform minNode = null;
        float minDistLocal = Mathf.Infinity;

        foreach (Transform node in pathPoints)
        {
            if (node != null && !node.GetComponent<PathfinderNode>().wasVisited)
            {
                float dist = Vector3.Distance(transform.position, node.position);
                if (dist < minDistLocal)
                {
                    minDistLocal = dist;
                    minNode = node;
                }
            }
        }

        return minNode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            foreach (Transform child in pathPointContainer.transform)
                child.GetComponent<PathfinderNode>().wasVisited = false;

            currentNode = GetClosestNode();
        }
    }
}
