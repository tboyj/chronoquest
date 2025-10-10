using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
public class BoneObjectCreator : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> boneSprites = new List<Sprite>();
    [SerializeField]
    private GameObject templateObject;
    private Image img;

    void Awake()
    {
        templateObject = transform.GetChild(0).gameObject;
        img = templateObject.transform.Find("Bone").gameObject.GetComponent<Image>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GenerateNewObject()
    {
        System.Random random = new System.Random();
        int range = random.Next(0, boneSprites.Count - 1);
        int x = random.Next(400, Screen.width - 400);
        int y = random.Next(400, Screen.height - 400);
        img.sprite = boneSprites[range];
        templateObject.transform.position = new Vector3(x, y, 0);
        Instantiate(templateObject);
    }
}
