using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;      // Base movement speed
    public float runMultiplier = 1.15f; // Sprint multiplier
    public float gravity = -0.075f;

    [Header("References")]
    
    public Transform rayholder; // Optional for stairs or slope checks

    protected Vector3 velocity;
    protected bool isGrounded;
    public bool flip;

    public float acceleration;   // How fast you gain speed
    public float deceleration; // How fast you slow down

    protected virtual void Awake()
    {
        
        rayholder = transform.Find("Rayholder");
        if (rayholder == null)
            Debug.LogWarning("Rayholder not found, slope detection will be limited.");
    }

    public virtual void MoveWithForce() { }
}


