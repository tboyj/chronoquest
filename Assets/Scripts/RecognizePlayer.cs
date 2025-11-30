using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private bool flawedBlock;
    private bool sendEmDown = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sendEmDown)
        {
            rb.isKinematic = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && flawedBlock)
        {
            sendEmDown = true;
        }
    }
}
