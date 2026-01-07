using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Paper : BaseUse
{
    
    public GameObject container;
    [TextArea]
    public string paperText;
    // public ItemStorable itemPreferred; // will likely change up in future but i need this working. SPEED I NEED THIS MY UI'S KIND OF HOMELESS...
    public bool isActive;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GetPlayer() != null)
        {
            if (GetPlayer().GetHeldItem().item != null)
            {
                if (GetPlayer().GetHeldItem().item.id == 0)
                {
                    Debug.Log(isActive);
                    Debug.Log("Id == 0");
                    // this is the new system, should be migrated to a new class for scalability
                    // item.id == 0 // this is magnifying glass // could change into switch case
                    // ___
                    // Still not working right now please fix!!!
                    ChangeTheUI("[E] Inspect Paper");
                } else
                {
                    ChangeTheUI("");
                }
            } else
            {
                ChangeTheUI("");
            }
             // only if holding the right thign.
            if (isActive)
            {
                // bug can occur where character can go outside the trigger zone, disabling.
                GetPlayer().movement.moveSpeed = 0;
                GetPlayer().SetInventoryPermission(false);
                GetPlayer().hotbarHolder.Find("OpenInv").GetComponent<Button>().interactable = false;
                ChangeTheUI("");
            }
            else
            {
                GetPlayer().movement.moveSpeed = 2f; // <-- This is the movement speed of the player. Find another way to do this lol.
                GetPlayer().hotbarHolder.Find("OpenInv").GetComponent<Button>().interactable = true;
                GetPlayer().SetInventoryPermission(true);
            }
        }
    }
    // Example method for using the paper item
    public override void Use()
    {
        container.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = paperText;
        isActive = !isActive;
        Debug.Log("Paper used!");
        musicSource.Play();
        container.SetActive(isActive);
        if (GetPlayer() != null)
        {
            GetPlayer().isUsingItem = isActive;
        }
    }
}
