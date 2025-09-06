using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public LayerMask collisionDetector;
    public float moveForce = 3.95f;       // movement force strength
    public float shiftMultiplier = 1.25f; // sprint multiplier
    public float maxSpeed = 2f;         // max horizontal speed
    // public float jumpForce = 5f;        // optional: jump
    public Rigidbody rb;
    public SpriteRenderer sprite;

    public Transform raycastPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // stops rigidbody from tipping over
        rb.useGravity = true;     // enable gravity
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(x, 0, z).normalized;

        float currentForce = moveForce;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftMultiplier = 1.25f;
        }
        else
        {
            shiftMultiplier = 1;
        }
        currentForce *= shiftMultiplier;
        // Apply movement force
        MoveWithForce(movement, currentForce);

        // Flip sprite
        if (x > 0)
        {
            sprite.flipX = false;
        }
        else if (x < 0)
        {
            sprite.flipX = true;
        }

        // Debug: ground ray
        Debug.DrawRay(raycastPos.position, -transform.up * 0.1f, Color.red);

        // Example jump (optional)
        // if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        // {
        //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        // }
    }

    void MoveWithForce(Vector3 direction, float force)
    {
        if (direction.sqrMagnitude == 0) return;

        rb.AddForce(direction * force, ForceMode.Acceleration);

        // Clamp horizontal velocity only (keep gravity on Y)
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
        }
    }

    // bool IsGrounded()
    // {
    //     return Physics.Raycast(raycastPos.position, Vector3.down, 0.2f, collisionDetector);
    // }
}
