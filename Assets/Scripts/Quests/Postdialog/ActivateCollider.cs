using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollider : AfterQuestDialog
{
    // Start is called before the first frame update
    public Collider collider;
    void Start()
    {
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void SetChange()
    {
        collider.enabled = true;
    }
}