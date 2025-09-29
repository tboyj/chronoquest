using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using System;

public class HoldingItemScript : MonoBehaviour
{
    public SpriteRenderer spriteHolderImage;
    public Image spriteTopLeftImage; // UI Image

    public void Start()
    { //
        spriteHolderImage.gameObject.SetActive(true);
        spriteTopLeftImage.gameObject.SetActive(true);
    }

    public void Activate(bool v)
    {
        spriteHolderImage.enabled = v;
        spriteTopLeftImage.enabled = v;
    }

    public void EnableWithSprite(Sprite sprite)
    {
        spriteHolderImage.enabled = true;
        spriteHolderImage.sprite = sprite;
        spriteTopLeftImage.enabled = true;
        spriteTopLeftImage.sprite = sprite;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteHolderImage.sprite = sprite;
        spriteTopLeftImage.sprite = sprite;
    }
}