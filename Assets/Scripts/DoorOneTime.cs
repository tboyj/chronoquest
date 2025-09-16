using UnityEngine;

public class DoorOneTime : ItemHandler
{
    public ItemStorable itemWanted;
    public SpriteRenderer door;
    public BoxCollider doorPassageWay;
    public int quantityNeeded;
    public bool opened = false;

    void Start()
    {
        door = transform.Find("Object").GetComponent<SpriteRenderer>();
        doorPassageWay = transform.Find("Object").GetComponent<BoxCollider>();
    }

    protected override bool IsValidItem(Item item, int index)
    {
        return base.IsValidItem(item, index) &&
               item.item == itemWanted &&
               quantityNeeded <= item.quantity;
    }

    protected override void OnItemReceived(Item item, int index)
    {
        if (quantityNeeded > 0)
        {
            quantityNeeded--;
            item.quantity--; // Directly decrement quantity
            items.Add(item);
        }

        if (quantityNeeded <= 0 && !opened)
        {
            openDoor();
        }
    }

    void Update()
    {
        base.HandleInteraction();
    }

    void openDoor()
    {
        door.color = Color.black;
        opened = true;
        doorPassageWay.enabled = false;
    }
}