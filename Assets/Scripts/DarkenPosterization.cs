using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DarkenPosterization : MonoBehaviour
{
    // Start is called before the first frame update
    public float ogLevel;
    public float darkenLevel;
    public PixelateEffect posterizationController;
    public GameObject removal;
    public int divideVy;
    private Material mat;
    void Start()
    {
        if (removal != null)
        { 
            Renderer rend = removal.GetComponent<Renderer>();
            if (rend != null)
                mat = rend.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            posterizationController = other.gameObject.transform.Find("MainCamera").GetComponent<PixelateEffect>();
            if (mat != null) {
                float a1 = (float) divideVy/6;
                Debug.Log(a1*255);
                if (divideVy == 1)
                    a1 = 0;
                mat.color = new Color(0,0,0,a1 - 1/6);
            }
            posterizationController.colorLevels = darkenLevel;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            posterizationController = other.gameObject.transform.Find("MainCamera").GetComponent<PixelateEffect>();
            if (mat != null) {
                float a1 = (float) divideVy/6;
                Debug.Log(a1*255);
                if (divideVy == 1)
                    a1 = 0;
                mat.color = new Color(0,0,0,a1 - 1/6);
            }
            posterizationController.colorLevels = darkenLevel;
        }
    }
        void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            posterizationController.colorLevels = ogLevel;
            if (mat != null)
                mat.color = new Color(0,0,0,200);
            posterizationController = null;

        }
    }
}
