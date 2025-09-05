using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Serialization;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerCollider : MonoBehaviour
{
    public float speed;
    public float shiftUp;

    public Rigidbody rb;
    public SpriteRenderer sprite;
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

        Vector3 movement = new Vector3(x, 0, z);
        rb.velocity = movement * speed;
        if (Input.GetKey(KeyCode.LeftShift)) // Stamina & running
        {
            rb.velocity = movement * (speed + .5f);
        }





        if (x != 0)
        {
            if (x < 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }
    }
}
