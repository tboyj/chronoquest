using UnityEngine;

public class Door : ItemHandler
{
    public ItemStorable itemWanted;
    public SpriteRenderer door;
    public BoxCollider doorPassageWay;
    public int quantityNeeded;
    public bool unlocked = false;
    public bool opened = false;

    void Start()
    {
        door = transform.Find("Object").GetComponent<SpriteRenderer>();
        doorPassageWay = transform.Find("Object").GetComponent<BoxCollider>();
    }

    protected override bool IsValidItem(Item item, int index)
    {
        return base.IsValidItem(item, index) &&
               !unlocked &&
               item.item == itemWanted &&
               quantityNeeded <= item.quantity;
    }

    protected override void OnItemReceived(Item item, int index)
    {
        if (quantityNeeded > 0)
        {
            quantityNeeded--;
            items.Add(item);
            InventoryScript.instance.RemoveItem(index);
        }

        if (quantityNeeded <= 0 && !unlocked)
        {
            unlocked = true;
            openDoor();
            Debug.Log("Door unlocked.");
        }
    }

    void Update()
    {
        base.HandleInteraction();

        if (unlocked && playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (opened)
                closeDoor();
            else
                openDoor();
        }
    }

    void openDoor()
    {
        door.color = Color.black;
        opened = true;
        doorPassageWay.enabled = false;
    }

    void closeDoor()
    {
        door.color = Color.white;
        opened = false;
        doorPassageWay.enabled = true;
    }
}