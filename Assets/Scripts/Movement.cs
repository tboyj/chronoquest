using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.45f;      // Base movement speed
    public float runMultiplier = 1.15f; // Sprint multiplier
    public float gravity = -0.075f;

    [Header("References")]
    public CharacterController controller;
    public Transform rayholder; // Optional for stairs or slope checks

    protected Vector3 velocity;
    protected bool isGrounded;
    public bool flip;

    public float acceleration = 5f;   // How fast you gain speed
    public float deceleration = 7.6f; // How fast you slow down

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


