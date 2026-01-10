using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer sprite;
    [Tooltip("E1: X axis, E2: Y axis, E3: Z axis")]
    public bool[] lockAxis = new bool[3]; // X, Y, Z
    private float spX;
    private float spY;
    private float spZ;

    void Start()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();
        spX = sprite.transform.rotation.eulerAngles.x;
        spY = sprite.transform.rotation.eulerAngles.y;
        spZ = sprite.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null) return;

        sprite.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - sprite.transform.position);
        if (lockAxis[0])
        {
            spY = sprite.transform.rotation.eulerAngles.y;
            spZ = sprite.transform.rotation.eulerAngles.z;
            sprite.transform.rotation = Quaternion.Euler(spX, sprite.transform.rotation.eulerAngles.y, sprite.transform.rotation.eulerAngles.z);
        }
        if (lockAxis[1])
        {
            spX = sprite.transform.rotation.eulerAngles.x;
            spZ = sprite.transform.rotation.eulerAngles.z;
            sprite.transform.rotation = Quaternion.Euler(sprite.transform.rotation.eulerAngles.x, spY, sprite.transform.rotation.eulerAngles.z);
        }
        if (lockAxis[2])
        {
            spX = sprite.transform.rotation.eulerAngles.x;
            spY = sprite.transform.rotation.eulerAngles.y;
            sprite.transform.rotation = Quaternion.Euler(sprite.transform.rotation.eulerAngles.x, sprite.transform.rotation.eulerAngles.y, spZ);
        }
    }
}
