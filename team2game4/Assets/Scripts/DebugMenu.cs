using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu instance;
    public bool mouseOverBtn;
    public bool open;
    public TMP_Dropdown presetDrop;

    public Slider PillarMinGap, PillarMaxGap, PillarSpacing, SafeSlowDown;

    private void Start()
    {
        instance = this;

        PillarSpawn pilSpawn = PillarSpawn.instance;
        //Event listeners
        PillarMinGap.onValueChanged.AddListener((float val) => //<< Parameter and "onValueChanged" different based on input type
        {
            if (val > pilSpawn.maxGapSize)
            {
                val = pilSpawn.maxGapSize;
                PillarMinGap.value = val;
            }
            pilSpawn.minGapSize = (int)val;
        });
        PillarMaxGap.onValueChanged.AddListener((float val) =>
        {
            if (val < pilSpawn.minGapSize)
            {
                val = pilSpawn.minGapSize;
                PillarMaxGap.value = val;
            }
            pilSpawn.maxGapSize = (int)val;
        });
        /*PillarSpacing.onValueChanged.AddListener((float val) =>
        {
            pilSpawn.horizontalSpacing = val;
        });*/
        /*SafeSlowDown.onValueChanged.AddListener((float val) =>
        {
            AimingScript.instance.safeZoneSlowDown = val;
        });*/
    }

    public void MouseOverBtn(bool perhaps)
    {
        mouseOverBtn = perhaps;
    }

    public void TogglePanel(GameObject debugPanel)
    {
        debugPanel.SetActive(!debugPanel.activeSelf);
        Time.timeScale = (debugPanel.activeSelf)?0:1;
        open = debugPanel.activeSelf;
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
