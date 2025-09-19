using UnityEngine.UIElements;
using UnityEngine;

public class HoldingItemScript : MonoBehaviour
{
    public SpriteRenderer spriteHolderImage;
    public Image spriteTopLeftImage;

    public void Start()
    {
        spriteHolderImage.gameObject.SetActive(true);
        spriteTopLeftImage.SetEnabled(true);
    }
    public void SetSelectedImage(Item item)
    {
        if (item.item != null)
        {
            spriteHolderImage.sprite = item.item.sprite;
            spriteTopLeftImage.sprite = item.item.sprite;
            spriteHolderImage.gameObject.SetActive(true);
            spriteTopLeftImage.SetEnabled(true);
        }
        else
        {
            spriteHolderImage.gameObject.SetActive(false);
            spriteTopLeftImage.SetEnabled(false);
        }
    }
}