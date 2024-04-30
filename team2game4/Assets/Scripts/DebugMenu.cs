using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu instance;
    public bool mouseOverBtn;
    public bool open;
    public TMP_Dropdown presetDrop;

    //Inputs
    public Slider PillarMinGap, PillarMaxGap, PillarSpacing, SafeSlowDown, AimLineThickness, AimLineLength;

    //Value Displays
    public TMP_Text minGapTxt, maxGapTxt, spacingTxt, safeSlowDownTxt, lineThicknessTxt, lineLengthTxt;

    GameManager gm;
    PillarSpawn pilSpawn;
    AimingScript aimScript;

    private void Start()
    {
        instance = this;
        gm = GameManager.gm;
        pilSpawn = PillarSpawn.instance;
        aimScript = AimingScript.instance;

        if (gm == null || pilSpawn == null || aimScript == null) { 
            StartCoroutine(lateStart());
            return;
        }

        //Set all values based on GameManager
        pilSpawn.maxGapSize = gm.maxPillarGap;
        pilSpawn.minGapSize = gm.minPillarGap;
        pilSpawn.horizontalSpacing = gm.pillarSpacing;
        aimScript.safeZoneSlowDown = gm.safeZoneSlowDown;
        aimScript.UpdateLine(gm.aimLineLength, gm.aimLineThickness);

        //Set initial input values
        PillarMaxGap.value = pilSpawn.maxGapSize;
        PillarMinGap.value = pilSpawn.minGapSize;
        PillarSpacing.value = pilSpawn.horizontalSpacing;
        SafeSlowDown.value = aimScript.safeZoneSlowDown;
        AimLineLength.value = aimScript.lineLength;
        AimLineThickness.value = aimScript.lineThickness;
        //Set initial label values
        minGapTxt.text = pilSpawn.minGapSize.ToString();
        maxGapTxt.text = pilSpawn.maxGapSize.ToString();
        spacingTxt.text = pilSpawn.horizontalSpacing.ToString();
        safeSlowDownTxt.text = aimScript.safeZoneSlowDown.ToString();
        lineLengthTxt.text = aimScript.lineLength.ToString();
        lineThicknessTxt.text = aimScript.lineThickness.ToString();

        //Add Input Event listeners
        PillarMinGap.onValueChanged.AddListener((float val) =>
        {
            if (val > pilSpawn.maxGapSize)
            {
                val = pilSpawn.maxGapSize;
                PillarMinGap.value = val;
            }
            pilSpawn.minGapSize = (int)val;
            minGapTxt.text = ((int)val).ToString();
        });
        PillarMaxGap.onValueChanged.AddListener((float val) =>
        {
            if (val < pilSpawn.minGapSize)
            {
                val = pilSpawn.minGapSize;
                PillarMaxGap.value = val;
            }
            pilSpawn.maxGapSize = (int)val;
            maxGapTxt.text = ((int)val).ToString();
        });
        PillarSpacing.onValueChanged.AddListener((float val) => { 
            pilSpawn.horizontalSpacing = val; 
            spacingTxt.text = val.ToString();
        });
        SafeSlowDown.onValueChanged.AddListener((float val) => { 
            aimScript.safeZoneSlowDown = val;
            safeSlowDownTxt.text = val.ToString();
        });
        AimLineLength.onValueChanged.AddListener((float val) => { 
            aimScript.UpdateLine(val, aimScript.lineThickness); 
            lineLengthTxt.text = val.ToString();
        });
        AimLineThickness.onValueChanged.AddListener((float val) => { 
            aimScript.UpdateLine(aimScript.lineLength, val); 
            lineThicknessTxt.text = val.ToString();
        });
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

        //Set GameManager Values
        gm.maxPillarGap = pilSpawn.maxGapSize;
        gm.minPillarGap = pilSpawn.minGapSize;
        gm.pillarSpacing = pilSpawn.horizontalSpacing;
        gm.safeZoneSlowDown = aimScript.safeZoneSlowDown;
        gm.aimLineLength = aimScript.lineLength;
        gm.aimLineThickness = aimScript.lineThickness;
        //

        //Reset the game scene to apply changes more safely
        if(!open)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    IEnumerator lateStart()
    {
        yield return null;
        Start();
    }
}
