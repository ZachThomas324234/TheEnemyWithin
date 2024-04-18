using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVisible : MonoBehaviour
{
    
    public bool HideOnStart;

    void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(HideOnStart == true) HideMouse();
    }
}
