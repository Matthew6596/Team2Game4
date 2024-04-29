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

    public Slider PillarMinGap, PillarMaxGap, PillarSpacing, SafeSlowDown;

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

        //Set initial input values
        PillarMaxGap.value = pilSpawn.maxGapSize;
        PillarMinGap.value = pilSpawn.minGapSize;
        PillarSpacing.value = pilSpawn.horizontalSpacing;
        SafeSlowDown.value = aimScript.safeZoneSlowDown;

        //Add Input Event listeners
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
        PillarSpacing.onValueChanged.AddListener((float val) =>
        {
            pilSpawn.horizontalSpacing = val;
        });
        SafeSlowDown.onValueChanged.AddListener((float val) =>
        {
            aimScript.safeZoneSlowDown = val;
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
