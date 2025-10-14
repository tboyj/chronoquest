using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public LayerMask collisionDetector;
    public float moveForce = 22.5f;       // movement default force strength
    public float runMultiplier = 1.25f;   // sprint multiplier
    public float maxSpeed = 1.5f;         // max horizontal speed
    // public float jumpForce = 5f;        // optional: jump

    public Rigidbody rb;
    public Transform rayholder;
    public bool flip = false;

    protected virtual void Initialize(float moveForce, float runMultiplier, float maxSpeed, Rigidbody rb)
    {
        this.moveForce = moveForce;
        this.runMultiplier = runMultiplier;
        this.maxSpeed = maxSpeed;
    }

    // Abstract methods that must be implemented by derived classes
    public virtual void MoveWithForce() { }

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
    public override void MoveWithForce()
    {
        if (Time.timeScale <= 0) return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Ray ray;

        if (flip)
        {
            rayholder.position = new Vector3(rb.transform.position.x - 0.075f, rayholder.position.y, rayholder.position.z);
            ray = new Ray(rayholder.position, new Vector3(-0.025f, 0, 0));
        }
        else
        {
            rayholder.position = new Vector3(rb.transform.position.x + 0.075f, rayholder.position.y, rayholder.position.z);
            ray = new Ray(rayholder.position, new Vector3(0.025f, 0, 0));
        }

        Debug.DrawRay(rayholder.position, ray.direction, Color.green);
        RaycastHit hit;

        Vector3 currentMovement = new Vector3(x, -0.5f, z).normalized;
        Debug.Log("Movement (x): " + currentMovement.x);

        if (Physics.Raycast(ray, out hit, 0.025f))
        {
            if (hit.collider.CompareTag("Stair") && x > 0.1f)
            {
                rb.isKinematic = false;
                Debug.Log("In the presence of stairs");
                float stairDecreaser = 0.0f;
                currentMovement = new Vector3(x - stairDecreaser, 0, z - stairDecreaser);
                if (x > 0.1f)
                {
                    currentMovement.y += 1.75f;
                }
            } else
            {
                
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }

        Ray rayZ = new Ray();
        if (currentMovement.z < 0)
        {
            rayholder.position = new Vector3(rayholder.position.x, rayholder.position.y, rb.transform.position.z - 0.075f);
            rayZ = new Ray(rayholder.position, new Vector3(0, 0, -0.025f));
        }
        else if (currentMovement.z > 0)
        {
            rayholder.position = new Vector3(rayholder.position.x, rayholder.position.y, rb.transform.position.z + 0.075f);
            rayZ = new Ray(rayholder.position, new Vector3(0, 0, 0.025f));
        }

        Debug.DrawRay(rayholder.position, rayZ.direction, Color.blue);

        if (Physics.Raycast(rayZ, out hit, 0.025f))
        {
            if (hit.collider.CompareTag("Stair") && z > 0.1f)
            {
                rb.isKinematic = false; // do this with a ground ray instead...looking good otherwise
                Debug.Log("In the presence of stairs");
                float stairDecreaser = 0.0f;
                currentMovement = new Vector3(x - stairDecreaser, 0, z - stairDecreaser);
                if (z > 0.1f)
                {
                    currentMovement.y += 1.75f;
                }
            } else
            {
                
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }

        float currentForce = moveForce;
        runMultiplier = Input.GetKey(KeyCode.LeftShift) ? 1.25f : 1f;
        currentForce *= runMultiplier;

        if (currentMovement.sqrMagnitude == 0) return;

        rb.AddForce(currentMovement * currentForce, ForceMode.Acceleration);

        // Clamp horizontal velocity (keep gravity on Y)
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
        }

        // Flip character
        if (x < 0) flip = true;
        else if (x > 0) flip = false;
    }

    void Start()
    {
        rb.freezeRotation = true; // stops rigidbody from tipping over
        rb.useGravity = true;     // enables gravity
    }
}

[System.Serializable]
public class NPCMovement : Movement
{
    public string status = "Idle";
    private PauseScript pauseScript;
    public GameObject pathPointContainer;
    public List<Transform> pathPoints = new List<Transform>();
    public Transform currentNode;

    private Transform minNode = null;
    [SerializeField] private float x = 0;
    [SerializeField] private float z = 0;
    [SerializeField] private float minDist = Mathf.Infinity;

    public void Start()
    {
        rb.freezeRotation = true;
        rb.useGravity = true;

        pathPointContainer = GameObject.Find("AINodeHolder");

        foreach (Transform child in pathPointContainer.transform)
            pathPoints.Add(child);

        currentNode = GetClosestNode();
    }

    public Transform GetClosestNode()
    {
        foreach (Transform node in pathPoints)
        {
            if (node != null && node.GetComponent<PathfinderNode>().wasVisited == false)
            {
                float dist = Vector3.Distance(transform.position, node.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    minNode = node;
                }
            }
        }
        return minNode;
    }

    public void FixedUpdate()
    {
        if (status != "Idle")
        {
            MoveWithForce();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public override void MoveWithForce()
    {
        if (Time.timeScale <= 0 || currentNode == null) return;

        Ray ray;
        if (flip)
        {
            rayholder.position = new Vector3(rb.transform.position.x - 0.075f, rayholder.position.y, rayholder.position.z);
            ray = new Ray(rayholder.position, new Vector3(-0.025f, 0, 0));
        }
        else
        {
            rayholder.position = new Vector3(rb.transform.position.x + 0.075f, rayholder.position.y, rayholder.position.z);
            ray = new Ray(rayholder.position, new Vector3(0.025f, 0, 0));
        }

        Debug.DrawRay(rayholder.position, ray.direction, Color.green);

        Vector3 currentMovement = new Vector3(x, -0.5f, z).normalized;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.025f))
        {
            if (hit.collider.CompareTag("Stair") && currentMovement.x > 0)
            {
                float stairDecreaser = 0.5f;
                if (transform.position.x > 0)
                    currentMovement.y += 0.1f;
                else if (transform.position.x < 0)
                    currentMovement.y += 0.1f;
                currentMovement = new Vector3(x - stairDecreaser, -0.25f, z - stairDecreaser);
            }
        }

        Vector3 direction = (currentNode.position - transform.position).normalized;
        x = direction.x;
        z = direction.z;

        float currentForce = moveForce;
        if (currentMovement.sqrMagnitude == 0) return;

        rb.AddForce(currentMovement * currentForce, ForceMode.Acceleration);

        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
        }

        if (x < 0) flip = true;
        else if (x > 0) flip = false;

        float dist = Vector3.Distance(transform.position, currentNode.position);
    }

    public Transform GetCurrentNode() => minNode;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            Debug.Log("End reached");
            pathPoints.Clear();
            foreach (Transform child in pathPointContainer.transform)
            {
                pathPoints.Add(child);
                child.GetComponent<PathfinderNode>().wasVisited = false;
            }
        }

        if (other.CompareTag("Middleman") && other.transform == currentNode)
        {
            currentNode.GetComponent<PathfinderNode>().wasVisited = true;
            minDist = Mathf.Infinity;
            currentNode = GetClosestNode();
        }

        if (other.CompareTag("Player"))
        {
            // Optional: interaction logic here
        }
    }
}
