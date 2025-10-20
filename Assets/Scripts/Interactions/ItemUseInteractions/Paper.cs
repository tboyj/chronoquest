using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Paper : BaseUse
{

    public GameObject container;
    [TextArea]
    public string paperText;
    public bool isActive = false;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (GetPlayer() != null)
        {
            if (isActive)
            {
                // GetPlayer().movement.enabled = false; // bug can occur where character can go outside the trigger zone, disabling.
                GetPlayer().movement.moveSpeed = 0;
            }
            else
            {
                // GetPlayer().movement.enabled = true;
                GetPlayer().movement.moveSpeed = 0.45f;
            }
        }
    }
    // Example method for using the paper item
    public override void Use()
    {
        container.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = paperText;
        isActive = !isActive;
        container.SetActive(isActive);
        if (GetPlayer() != null)
        {
            GetPlayer().isUsingItem = isActive;
        }
    }
}
