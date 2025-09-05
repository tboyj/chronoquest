using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rbGr;
    public float maxforce;
    public float force;
    public LayerMask ground;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 groundQ = transform.TransformDirection(Vector3.down);
        // if (Physics.Raycast(transform.position, groundQ, .1f, ground))
        // {
        //     force = 0;
        // }
        // else
        // {
        //     if (force == 0)
        //     {
        //         force = -.01f;
        //     }
        //     if (force > maxforce)
        //     {
        //         force -= Math.Abs(force);
        //     }
        //     else
        //     {
        //         force = maxforce;
        //     }

        // }

    }
    void FixedUpdate()
    {
        rbGr.AddForce(Physics.gravity, ForceMode.Acceleration);
    }
    
}
