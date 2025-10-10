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
    {
        spriteHolderImage.gameObject.SetActive(true);
        spriteTopLeftImage.gameObject.SetActive(true);
    }

    internal void Activate(bool v)
    {
        spriteHolderImage.enabled = v;
        spriteTopLeftImage.enabled = v;
    }

    internal void EnableWithSprite(Sprite sprite)
    {
        spriteHolderImage.enabled = true;
        spriteHolderImage.sprite = sprite;
        spriteTopLeftImage.enabled = true;
        spriteTopLeftImage.sprite = sprite;
    }

    internal void SetSprite(Sprite sprite)
    {
        spriteHolderImage.sprite = sprite;
        spriteTopLeftImage.sprite = sprite;
    }
    public bool GetActiveness()
    {
        if (spriteHolderImage.enabled && spriteTopLeftImage.enabled)
        {
            return true;
        }
        else if (!spriteHolderImage.enabled && !spriteTopLeftImage.enabled)
        {
            return false;
        }
        else
        {
            Debug.Log("what do you have...?");
            return false;
        }
    }
}