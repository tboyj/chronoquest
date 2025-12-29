using System;
using UnityEngine;

public class PlayerMovement : Movement
{
    public CharacterController controller;
    public Vector3 rawInput;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private AudioSource jumpSFX;
    private bool jumpRequested = false; // Buffer for jump input
    
    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (GameObject.Find("AudioSources/Jump") != null)
            jumpSFX = GameObject.Find("AudioSources/Jump").GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        // Only handle input detection in Update to catch it every frame
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
            jumpSFX.Play();
            
        }
    }

    public float jumpForce = 7.2f;
    public float gravityStrength = -8.81f;
    private float verticalVelocity;

    public override void MoveWithForce()
    {
        if (this == null || transform == null || controller == null) return; // Safety check
        isGrounded = controller.isGrounded;
        
        if (isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -1f;

            if (jumpRequested) // Check the buffered input
            {
                verticalVelocity = jumpForce;
                jumpRequested = false; // Clear the buffer
            }
        }
        else
        {
            verticalVelocity += gravityStrength * Time.deltaTime;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        rawInput = new Vector3(x, 0, z);
        rawInput = Vector3.ClampMagnitude(rawInput, 1f);
        
        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;
            
        if (cameraTransform == null) return; // Can't move without camera reference
        
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 moveDirection = (camForward * z + camRight * x).normalized * rawInput.magnitude;

        if (!controller.gameObject.GetComponent<PauseScript>().isPaused && 
            !controller.gameObject.GetComponent<PauseScript>().isInventory && 
            !controller.gameObject.GetComponent<Player>().inDialog)
        {
            if (moveDirection.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                Transform metarig = controller.transform.Find("Jimsprite").Find("metarig");

                metarig.rotation = targetRotation;
                metarig.rotation = Quaternion.Euler(-90f, metarig.rotation.eulerAngles.y, metarig.rotation.eulerAngles.z);
            }
        }

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= runMultiplier;

        Vector3 horizontalVelocity;
        if (rawInput.magnitude > 0.01f)
            horizontalVelocity = Vector3.Lerp(new Vector3(velocity.x, 0, velocity.z), moveDirection * speed, acceleration * Time.deltaTime);
        else
            horizontalVelocity = Vector3.Lerp(new Vector3(velocity.x, 0, velocity.z), Vector3.zero, deceleration * Time.deltaTime);

        velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

        if (controller.enabled)
            controller.Move(velocity * Time.deltaTime);

        // flip = moveDirection.x < -0.1f || (moveDirection.x <= 0.1f && flip);
    }
}