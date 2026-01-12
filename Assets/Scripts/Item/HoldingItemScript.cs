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

    public void Awake()
    {

    }
/// <summary>
/// Handles the enabling/disabling of the sprite images based on whether they are null or not.
/// </summary>
    public void Start()
    {
        if (spriteHolderImage.sprite != null)
        {
            SetSprite(spriteHolderImage.sprite);
            
        }
        
    }
    public void Update()
    {
        if (spriteTopLeftImage == null)
        {
            spriteHolderImage.gameObject.SetActive(false);
            
        }
        if (spriteHolderImage == null)
        {
            spriteTopLeftImage.gameObject.SetActive(false);
        }
        if (spriteHolderImage != null && spriteTopLeftImage != null)
        {
            spriteHolderImage.gameObject.SetActive(true);
            spriteTopLeftImage.gameObject.SetActive(true);
        }
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