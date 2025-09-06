using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DelayedCamScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 offset = new Vector3(0, .75f, -1.75f);
    public float smoothTime = .075f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tp = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, tp, ref velocity, smoothTime);
    }
}
