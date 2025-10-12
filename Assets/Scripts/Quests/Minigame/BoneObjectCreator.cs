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
    private List<Sprite> evilBoneSprites = new List<Sprite>();
    [SerializeField]
    private GameObject templateObject;
    private Image img;
    private Canvas canvas;
    public bool allBonesDusted;
    [SerializeField]
    private List<BoneObject> bonesList = new List<BoneObject>();
    public ItemStorable boneNotCracked;
    public ItemStorable boneCracked;
    void Awake()
    {
        img = templateObject.transform.Find("Bone").gameObject.GetComponent<Image>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }
    void Start()
    {

    }
    public void AreAllBonesDusted()
    {
        foreach (BoneObject bone in bonesList)
        {
            if (bone.bone.color.a > 0f)
                allBonesDusted = false;
        }
        allBonesDusted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (allBonesDusted == true)
        {
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inventory;
            foreach (BoneObject bone in bonesList)
            {
                
                if (bone.dirtStrength < 0)
                {
                    inventory.AddItem(new Item(boneCracked, 1));
                }
                else
                {
                    inventory.AddItem(new Item(boneNotCracked, 1));
                }
                Destroy(bone.gameObject);
            }
        }
        bonesList.Clear();
        allBonesDusted = false;
    }

    public void GenerateNewObject()
    {
        System.Random random = new System.Random();
        Debug.Log("Max;"+ (boneSprites.Count - 1));
        int range = random.Next(0, boneSprites.Count - 1);
        Vector2 screenPos = new Vector2(
            random.Next(400, Screen.width - 400),
            random.Next(400, Screen.height - 400)
        );

        templateObject.SetActive(true);
        RectTransform canvasRect = gameObject.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPos);
        templateObject.GetComponent<RectTransform>().localPosition = new Vector3(localPos.x, localPos.y, 0);
        GameObject newObj = Instantiate(templateObject, gameObject.transform.Find("Bones")); // apply position
        newObj.GetComponent<BoneObject>().bone.sprite = boneSprites[range];
        bonesList.Add(newObj.GetComponent<BoneObject>());
        templateObject.SetActive(false);
    }

    public List<Sprite> GetBoneSpritesList()
    {
        return boneSprites;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public Sprite GetEvilBoneVersion(int v)
    {
        return evilBoneSprites[v];
    }
}
