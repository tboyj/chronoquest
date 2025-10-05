using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{

    public LayerMask collisionDetector;
    public float moveForce = 22.5f;       // movement default force strength
    public float runMultiplier = 1.25f; // sprint multiplier
    public float maxSpeed = 1.5f;         // max horizontal speed
    // public float jumpForce = 5f;        // optional: jump
    public Rigidbody rb;
    public Transform rayholder;

    public bool flip = false;



    protected virtual void Initialize(float moveForce, float runMultiplier,
    float maxSpeed, Rigidbody rb)
    {
        this.moveForce = moveForce;
        this.runMultiplier = runMultiplier;
        this.maxSpeed = maxSpeed;

    }

    // Abstract methods that must be implemented by derived classes
    public virtual void MoveWithForce()
    {

    }
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rayholder = transform.Find("Rayholder");

        if (rayholder == null)
            Debug.LogError("Rayholder not found on " + gameObject.name);
    }


}
public class PlayerMovement : Movement
{
    
    /// <summary>
    /// Applies a force to the player's Rigidbody in the direction of the player's input.
    /// The force is multiplied by the run multiplier if the left shift key is pressed.
    /// The horizontal velocity is clamped to the maximum speed.
    /// </summary>
    /// <param name="force">The force to apply.</param>
    public override void MoveWithForce()
    {

        if (Time.timeScale > 0)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Ray ray = new Ray();
            if (flip)
            {

                rayholder.position = new Vector3(rb.transform.position.x - 0.075f, rayholder.position.y, rayholder.position.z);
                ray = new Ray(rayholder.position, new Vector3(-0.025f, 0, 0));
            } else {

                rayholder.position = new Vector3(rb.transform.position.x + 0.075f, rayholder.position.y, rayholder.position.z);
                ray = new Ray(rayholder.position, new Vector3(0.025f, 0, 0));
            }

            Debug.DrawRay(rayholder.position, ray.direction, Color.green);   
            RaycastHit hit;
            Vector3 currentMovement = new Vector3(x, -.5f, z).normalized;
            if (Physics.Raycast(ray, out hit, 0.025f))
            {
                if (hit.collider.CompareTag("Stair") && currentMovement.x > 0)
                {
                    float stairDecreaser = .5f;
                    if (transform.position.x > 0)
                    {
                        transform.position = new Vector3(transform.position.x + .025f, transform.position.y + 0.2f, transform.position.z);
                    }
                    else if (transform.position.x < 0)
                    {
                        transform.position = new Vector3(transform.position.x - .025f, transform.position.y + 0.2f, transform.position.z);
                    }
                    currentMovement = new Vector3(x - stairDecreaser, -.25f, z - stairDecreaser);
                }
                // Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            }
            
            float currentForce = moveForce;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                runMultiplier = 1.25f;
            }
            else
            {
                runMultiplier = 1;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                runMultiplier = 1;
            }
            currentForce *= runMultiplier;
            if (currentMovement.sqrMagnitude == 0) return;

            rb.AddForce(currentMovement * currentForce, ForceMode.Acceleration);
            // Debug.Log("Force: "+rb.GetAccumulatedForce());
            // Clamp horizontal velocity only (keep gravity on Y)
            Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVel.magnitude > maxSpeed)
            {
                horizontalVel = horizontalVel.normalized * maxSpeed;
                rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
            }
            if (x < 0)
            {
                flip = true;
            }
            else if (x > 0)
            {
                flip = false;
            }


        }
    }


    void Start()
    {

        rb.freezeRotation = true; // stops rigidbody from tipping over
        rb.useGravity = true;     // enables gravity

    }
    // Example jump (optional)
    // if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    // {
    //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    // }


}
[System.Serializable]
public class NPCMovement : Movement
{
    public GameObject pathPointContainer;
    public List<Transform> pathPoints = new List<Transform>();
    public NavMeshAgent agent;
    public Transform currentNode;
    private Transform minNode = null;
    private int currentWaypointIndex = 0;
    /// <summary>
    /// Applies a force to the player's Rigidbody in the direction of the player's input.
    /// The force is multiplied by the run multiplier if the left shift key is pressed.
    /// The horizontal velocity is clamped to the maximum speed.
    /// </summary>
    /// <param name="force">The force to apply.</param>
    public void Start()
    {
        currentNode = null;
        rb.freezeRotation = true; // stops rigidbody from tipping over
        rb.useGravity = true;     // enables gravity

        pathPointContainer = GameObject.Find("AINodeHolder");

        foreach (Transform child in pathPointContainer.transform)
        {
            pathPoints.Add(child);
        }

        agent = GetComponent<NavMeshAgent>();
        if (pathPoints.Count > 0)
            agent.SetDestination(pathPoints[0].position);
    }
    public void FixedUpdate()
    {
        MoveWithForce();
    }
    public override void MoveWithForce()
    {
        if (pathPoints.Count == 0) return;

        // If agent is close to current waypoint, go to the next
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % pathPoints.Count; 
            agent.SetDestination(pathPoints[currentWaypointIndex].position);
        }
        // if (Time.timeScale > 0)
        // {

        //     Ray ray;
        //     if (flip) // fix this later in the morning
        //     {

        //         rayholder.position = new Vector3(rb.transform.position.x - 0.075f, rayholder.position.y, rayholder.position.z);
        //         ray = new Ray(rayholder.position, new Vector3(-0.025f, 0, 0));
        //     }
        //     else
        //     {

        //         rayholder.position = new Vector3(rb.transform.position.x + 0.075f, rayholder.position.y, rayholder.position.z);
        //         ray = new Ray(rayholder.position, new Vector3(0.025f, 0, 0));
        //     }

        //     Debug.DrawRay(rayholder.position, ray.direction, Color.green);
        //     RaycastHit hit;

        //     float x = 0;
        //     float z = 0;


        //     Vector3 currentMovement = new Vector3(x, -.5f, z).normalized;
        //     if (Physics.Raycast(ray, out hit, 0.025f))
        //     {
        //         if (hit.collider.CompareTag("Stair") && currentMovement.x > 0)
        //         {
        //             float stairDecreaser = .5f;
        //             if (transform.position.x > 0)
        //             {
        //                 transform.position = new Vector3(transform.position.x + .025f, transform.position.y + 0.2f, transform.position.z);
        //             }
        //             else if (transform.position.x < 0)
        //             {
        //                 transform.position = new Vector3(transform.position.x - .025f, transform.position.y + 0.2f, transform.position.z);
        //             }
        //             currentMovement = new Vector3(x - stairDecreaser, -.25f, z - stairDecreaser);
        //         }
        //         // Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
        //     }

        //     float currentForce = moveForce;
        //     if (Input.GetKey(KeyCode.LeftShift))
        //     {
        //         runMultiplier = 1.25f;
        //     }
        //     else
        //     {
        //         runMultiplier = 1;
        //     }
        //     if (Input.GetKeyUp(KeyCode.LeftShift))
        //     {
        //         runMultiplier = 1;
        //     }
        //     currentForce *= runMultiplier;
        //     if (currentMovement.sqrMagnitude == 0) return;

        //     rb.AddForce(currentMovement * currentForce, ForceMode.Acceleration);
        //     // Debug.Log("Force: "+rb.GetAccumulatedForce());
        //     // Clamp horizontal velocity only (keep gravity on Y)
        //     Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //     if (horizontalVel.magnitude > maxSpeed)
        //     {
        //         horizontalVel = horizontalVel.normalized * maxSpeed;
        //         rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
        //     }
        //     if (x < 0)
        //     {
        //         flip = true;
        //     }
        //     else if (x > 0)
        //     {
        //         flip = false;
        //     }


        // }
    }

    public Transform GetCurrentNode()
    {
        return minNode;
    }




    // Example jump (optional)
    // if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    // {
    //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    // }


}
