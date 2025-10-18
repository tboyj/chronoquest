using UnityEngine;

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
        else
            velocity.y += gravity;
        if (Time.timeScale > 0)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
                speed *= runMultiplier;

            controller.Move(move * speed * Time.deltaTime);

            // Smooth acceleration/deceleration
            Vector3 input = new Vector3(x, 0, z);
            input = transform.TransformDirection(input).normalized;

            if (input.magnitude > 0.01f)
            {
                velocity = Vector3.Lerp(velocity, input * moveSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            controller.Move(velocity * Time.deltaTime);

            // Flip character
            if (x < 0) flip = true;
            else if (x > 0) flip = false;

        }
            // Optional: HandleStairs(move);
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