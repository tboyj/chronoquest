using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedCamScript : MonoBehaviour
{
    private Vector3 offset = new(0, -1.25f, -2f);
    public float smoothTime = .075f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform target;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    
    [Header("Collision Settings")]
    public LayerMask collisionLayers = ~0;
    public float collisionRadius = 0.05f;
    public float minDistance = 0.5f;
    
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        if (!SceneManager.GetSceneByName("SpaceEndScene").isLoaded)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        
        Vector3 rotatedOffset = rotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;
        
        Vector3 targetHead = target.position + Vector3.up * 0.5f;
        Vector3 direction = desiredPosition - targetHead;

        float desiredDistance = direction.magnitude;
        direction.Normalize();
        
        float actualDistance = desiredDistance;

        if (Physics.SphereCast(targetHead, collisionRadius, direction, out RaycastHit hit, desiredDistance, collisionLayers))
        {
            actualDistance = Mathf.Max(hit.distance, minDistance);
        }

        Vector3 targetPosition = targetHead + direction * actualDistance;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        
        transform.LookAt(targetHead);
    }
}
