using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnpoint : MonoBehaviour
{
    public GameObject respawnLocation;
    public Vector3 respawn;
    [SerializeField]
    private AudioSource respawnSFX;

/// <summary>
/// Finds the respawn location and sets the respawn point to its position.
/// </summary>
    public void Start()
    {
        respawnSFX = transform.Find("AudioSources/Respawn").GetComponent<AudioSource>();
        if (respawnLocation != null)
        {
            respawn = respawnLocation.transform.position;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} hit respawn point");
        if (other.CompareTag("Player"))
        {
            respawnSFX.Play();
            Teleport(other.gameObject);
        }
        else if (other.CompareTag("NPC"))
        {
            Teleport(other.gameObject);
        }
    }
        public void Teleport(GameObject target)
        {
            PlayerMovement movement = target.GetComponent<PlayerMovement>(); // or whatever your movement script is called
            if (movement != null && movement.controller != null)
            {
                movement.controller.enabled = false;
                target.transform.position = respawn;
                movement.controller.enabled = true;
            }
            else
            {
                Debug.LogWarning("Teleport failed: movement or controller missing.");
            }
        }
}