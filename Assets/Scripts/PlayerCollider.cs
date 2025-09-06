using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerCollider : MonoBehaviour
{
    public LayerMask collisionDetector;
    public float speed;
    public float shiftUp;
    public Vector3 maxSpeed;
    public Rigidbody rb;
    public SpriteRenderer sprite;

    public Transform raycastPos;
    public Transform raycastPos2;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Debug.Log("X axis (raw) --> "+x);
        // Debug.Log("Z axis (raw) --> "+z);
        
        // Ray rayForward = new Ray(raycastPos.position, transform.forward * rayDistance);
        // Debug.DrawRay(raycastPos.position, transform.forward * rayDistance, Color.red);
        // Ray rayBackward = new Ray(raycastPos.position, -transform.forward * rayDistance);
        // Debug.DrawRay(raycastPos.position, -transform.forward * rayDistance, Color.blue);

        // Ray rayForward2 = new Ray(raycastPos2.position, transform.forward * rayDistance);
        // Debug.DrawRay(raycastPos2.position, transform.forward * rayDistance, Color.red);
        // Ray rayBackward2 = new Ray(raycastPos2.position, -transform.forward * rayDistance);
        // Debug.DrawRay(raycastPos2.position, -transform.forward * rayDistance, Color.blue);

        RaycastHit hit;
        Vector3 movement = new Vector3(x, 0, z).normalized;
        // bool edgeForward = false;
        //  if (Physics.Raycast(rayForward, out hit, rayDistance, collisionDetector) || Physics.Raycast(rayForward2, out hit, rayDistance, collisionDetector))
        //     edgeForward = true;   
        // bool edgeBackward = false;
        //  if (Physics.Raycast(rayBackward, out hit, rayDistance, collisionDetector) || Physics.Raycast(rayBackward2, out hit, rayDistance, collisionDetector))
        //     edgeBackward = true;

        // if (edgeForward && z < 0) {
        //     movement = new Vector3(x, 0, 0);
        // } else if (edgeBackward && z > 0) {
        //     movement = new Vector3(x, 0, 0);
        // }

        if (rb.velocity.magnitude < maxSpeed.magnitude)
            rb.velocity = movement * speed;
        if (Input.GetKey(KeyCode.LeftShift)) // Stamina & running
        {
            rb.velocity = movement * (speed + shiftUp);
        }


        if (x != 0)
        {
            if (x < 0)
            {
                transform.localScale = new Vector3(-.32f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(.32f, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
