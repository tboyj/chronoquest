using System;
using UnityEngine;

public class PlayerMovement : Movement
{
    public CharacterController controller;

    private void Update()
        {
            MoveWithForce();
    }

    public float jumpForce = 7.2f; // how strong the jump is
public float gravityStrength = -8.81f; // consistent gravity pull
private float verticalVelocity; // separate Y velocity tracking

    public override void MoveWithForce()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -1f; // small downward push to keep grounded

            // Jump input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Apply gravity while in air
            verticalVelocity += gravityStrength * Time.deltaTime;
        }

        if (Time.timeScale > 0)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 rawInput = new Vector3(x, 0, z);
            rawInput = Vector3.ClampMagnitude(rawInput, 1f);
            Vector3 input = transform.TransformDirection(rawInput);

            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
                speed *= runMultiplier;

            Vector3 horizontalVelocity;
            if (rawInput.magnitude > 0.01f)
                horizontalVelocity = Vector3.Lerp(new Vector3(velocity.x, 0, velocity.z), input * speed, acceleration * Time.deltaTime);
            else
                horizontalVelocity = Vector3.Lerp(new Vector3(velocity.x, 0, velocity.z), Vector3.zero, deceleration * Time.deltaTime);

            // Combine horizontal and vertical movement
            velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
                if (controller.enabled)
                {
                    controller.Move(velocity * Time.deltaTime);
                }

            // Flip character
            if (x < 0) flip = true;
            else if (x > 0) flip = false;
        }
    }

    /*
    private void HandleStairs(Vector3 move)
    {
        if (rayholder == null) return;

        Ray ray = new Ray(rayholder.position + move.normalized * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            if (hit.collider.CompareTag("Stair"))
            {
                Vector3 stairStep = new Vector3(0, 0.2f, 0);
                controller.Move(stairStep);
            }
        }
    }
    */
}