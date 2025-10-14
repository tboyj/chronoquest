using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = .85f;        // Base movement speed
    public float runMultiplier = 1.25f; // Sprint multiplier
    public float gravity = -.01f;

    [Header("References")]
    public CharacterController controller;
    public Transform rayholder;         // Optional for stairs or slope checks

    protected Vector3 velocity;
    protected bool isGrounded;
    public bool flip;
    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("CharacterController not found on " + gameObject.name);

        rayholder = transform.Find("Rayholder");
        if (rayholder == null)
            Debug.LogWarning("Rayholder not found, slope detection will be limited.");
    }

    public virtual void MoveWithForce() { }


}
public class PlayerMovement : Movement
{
    
    

    private void Update()
    {
        MoveWithForce();
    }

    public override void MoveWithForce()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
            velocity.y = -1f; // small downward force to keep grounded

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= runMultiplier;

        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity;
        controller.Move(velocity * Time.deltaTime);

        // Flip character based on movement direction
        if (x < 0) flip = true;
        else if (x > 0) flip = false;

        // Optional stair adjustment
        // HandleStairs(move);
    }

    // private void HandleStairs(Vector3 move)
    // {
    //     if (rayholder == null) return;

    //     Ray ray = new Ray(rayholder.position + move.normalized * 0.1f, Vector3.down);
    //     if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
    //     {
    //         if (hit.collider.CompareTag("Stair"))
    //         {
    //             Vector3 stairStep = new Vector3(0, 0.2f, 0);
    //             controller.Move(stairStep);
    //         }
    //     }
    // }
}
[System.Serializable]
public class NPCMovement : Movement
{
    public string status = "Idle";
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
        if (currentNode == null) return;

        Vector3 direction = (currentNode.position - transform.position).normalized;

        Vector3 move = new Vector3(direction.x, 0, direction.z) * moveSpeed * Time.deltaTime;
        controller.Move(move);

        // Apply gravity
        if (!controller.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = -2f;

        controller.Move(velocity * Time.deltaTime);

        // Flip NPC based on movement
        if (direction.x < 0) flip = true;
        else if (direction.x > 0) flip = false;

        float dist = Vector3.Distance(transform.position, currentNode.position);
        if (dist < 0.5f) // Node reached
        {
            currentNode.GetComponent<PathfinderNode>().wasVisited = true;
            currentNode = GetClosestNode();
            minDist = Mathf.Infinity;
        }
    }

    public Transform GetClosestNode()
    {
        Transform closest = null;
        foreach (Transform node in pathPoints)
        {
            if (!node.GetComponent<PathfinderNode>().wasVisited)
            {
                float dist = Vector3.Distance(transform.position, node.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = node;
                }
            }
        }
        return closest;
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
