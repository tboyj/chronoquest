using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogGUIManager : MonoBehaviour
{
    public TextMeshProUGUI charName;
    public TextMeshProUGUI dialText;
    public void SetCharName(string cname)
    {
        charName.text = cname;
    }

    public void SetDialText(string dtext)
    {
        dialText.text = dtext;
    }
    public string GetPrint()
    {
        return charName.text + ": " + dialText.text;
    }


    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
