using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzlePanel : MonoBehaviour
{
    protected RectTransform gui;
    protected Button button;
    protected bool active = false;
    protected virtual void Start() {
        myButton.onClick.AddListener(OnButtonClick);
    }
    protected virtual void Update() {
        
    }
    protected PuzzlePanel(RectTransform guiPanel, Button submitButton) {
        gui = guiPanel;
        button = submitButton;
        active = true;
    }
}