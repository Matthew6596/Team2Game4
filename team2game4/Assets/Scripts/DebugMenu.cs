using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu instance;
    public bool mouseOverBtn;
    public TMP_Dropdown presetDrop;

    private void Start()
    {
        instance = this;
    }

    public void MouseOverBtn(bool perhaps)
    {
        mouseOverBtn = perhaps;
    }

    public void TogglePanel(GameObject debugPanel)
    {
        debugPanel.SetActive(!debugPanel.activeSelf);
        Time.timeScale = (debugPanel.activeSelf)?0:1;
    }
    public void SelectPreset()
    {
        switch (presetDrop.value)
        {
            case (1): //set EVIl values
                
                break;
            case (2): //set EZ peazy values
                
                break;
            default: //set Standard values
                
                break;
        }
    }
}
