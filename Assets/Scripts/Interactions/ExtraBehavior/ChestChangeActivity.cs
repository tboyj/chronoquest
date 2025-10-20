using UnityEngine;

public class ChestChangeActivity : ExtraBase
{
    public GameObject topChestObject;
    public bool active = true;
    void Start()
    {
        topChestObject.SetActive(active);
    }
    public override void Change()
    {
        active = !active;
        if (!active)
        {
            topChestObject.transform.localEulerAngles = new Vector3(0,0,45);
            topChestObject.transform.localPosition = new Vector3(0, 0.75f, 0);
        }
        else
        {
            topChestObject.transform.localEulerAngles = new Vector3(0,0,0);
            topChestObject.transform.localPosition = Vector3.zero;

        }
        
    }
}