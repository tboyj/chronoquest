using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoneObject : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public int dirtStrength;
    public bool fullyCleaned = false;
    public Image dirt;
    public Image bone;
    public BoneObjectCreator master;
    void Awake()
    {
        

    }
    void Start()
    {
        System.Random random = new System.Random();
        dirtStrength = random.Next(3, 6);
        dirt = transform.Find("Dust").GetComponent<Image>();
        bone = transform.Find("Bone").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dirtStrength <= 0)
        {
            dirt.color = new Color(dirt.color.r, dirt.color.g, dirt.color.b, 0);
        }
        else
        {
            dirt.color = SetNewAlpha(dirtStrength, 6, dirt);

        }

    }

    private Color SetNewAlpha(int current, int original, Image dirt)
    {
        float alpha = (float)current / original;
        return new Color(dirt.color.r, dirt.color.g, dirt.color.b, alpha);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dirtStrength == 0)
        {
            BreakThisBone();
            dirtStrength--;
        }
        if (dirtStrength == 1)
        {

            dirtStrength--;
            fullyCleaned = true;
            master.AreAllBonesDusted();
        }
        else if (dirtStrength > 1)
        {
            dirtStrength--;
        }
    }

    private void BreakThisBone()
    {
        Debug.Log("Broken / cracked bone");
        bone.sprite = master.GetEvilBoneVersion(master.GetBoneSpritesList().IndexOf(bone.sprite));
    }
}
