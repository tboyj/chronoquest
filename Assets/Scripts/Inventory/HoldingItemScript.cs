using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;

public class HoldingItemScript : MonoBehaviour
{
    public SpriteRenderer spriteHolderImage;
    public Image spriteTopLeftImage; // UI Image

    public void Start()
    {
        spriteHolderImage.gameObject.SetActive(true);
        
    }
    // public void SelectedPlayerItem(int index, List<Item> inventory, bool isHolding)
    // {
    //     spriteHolderImage.sprite = inventory[index].item.sprite;
    //     spriteHolderImage.gameObject.SetActive(isHolding);
    // }




    // public void SelectedNPCItem(Item item)
    // {
    //     if (item.item != null)
    //     {
    //         spriteHolderImage.sprite = item.item.sprite;
    //         spriteHolderImage.gameObject.SetActive(true);

    //     }
    //     else
    //     {
    //         spriteHolderImage.gameObject.SetActive(false);
    //     }
    // }
}