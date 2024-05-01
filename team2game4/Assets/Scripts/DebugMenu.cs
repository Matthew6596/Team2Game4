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
    public Slider PillarMinGap, PillarMaxGap, PillarSpacing, SafeSlowDown, AimLineThickness, AimLineLength, AimLineSpeed, AimLineSpeedIncrease, HungerDepleteAmount, FoodIncreaseAmount;

    public Toggle reticleToggle;

    //Value Displays
    public TMP_Text minGapTxt,   maxGapTxt,  spacingTxt,   safeSlowDownTxt, lineThicknessTxt, lineLengthTxt, lineSpeedTxt, lineSpeedIncreaseTxt, hungerSpeedTxt, foodIncreaseTxt;

    GameManager gm;
    PillarSpawn pilSpawn;
    AimingScript aimScript;
    HungerScript hungyScript; //new

    private void Start()
    {
        instance = this;
        gm = GameManager.gm;
        pilSpawn = PillarSpawn.instance;
        aimScript = AimingScript.instance;
        hungyScript = HungerScript.instance; //new

        if (gm == null || pilSpawn == null || aimScript == null || hungyScript == null) {  //new
            StartCoroutine(lateStart());
            return;
        }

        //Set all values based on GameManager
        SetAllValues(gm.maxPillarGap, gm.minPillarGap, gm.pillarSpacing, gm.safeZoneSlowDown, gm.aimLineLength, gm.aimLineThickness, gm.aimLineSpeed, gm.aimLineSpeedIncrease, gm.hungerDepleteAmount, gm.foodIncreaseAmount,gm.reticleOn);

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
        //Add New Input Event listeners
        AimLineSpeed.onValueChanged.AddListener((float val) => {
            aimScript.turnSpeed = val;
            lineSpeedTxt.text = val.ToString();
        });
        AimLineSpeedIncrease.onValueChanged.AddListener((float val) => {
            aimScript.turnSpeedIncrease = val;
            lineSpeedIncreaseTxt.text = val.ToString();
        });
        HungerDepleteAmount.onValueChanged.AddListener((float val) => {
            hungyScript.depleteBy = (int)val;
            hungerSpeedTxt.text = ((int)val).ToString();
        });
        FoodIncreaseAmount.onValueChanged.AddListener((float val) => {
            hungyScript.increaseAmount = (int)val;
            foodIncreaseTxt.text = ((int)val).ToString();
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
        gm.aimLineSpeed = aimScript.turnSpeed;
        gm.aimLineSpeedIncrease = aimScript.turnSpeedIncrease;
        gm.hungerDepleteAmount = hungyScript.depleteBy;
        gm.foodIncreaseAmount = hungyScript.increaseAmount;
        gm.reticleOn = reticleToggle.isOn;
        //

        //Reset the game scene to apply changes more safely
        if(!open)
        {
            gm.stomachMeter = 50;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
    }
    public void SelectPreset()
    {
        switch (presetDrop.value)
        {
            case (1): //set EVIl values
                SetAllValues(1, 2, 7, 0, 1.5f, 0.1f,60, 10,5,2,false);
                break;
            case (2): //set EZ peazy values
                SetAllValues(5, 5, 4.5f, 0.4f, 3.5f, 0.15f,40, 0,1,10,true);
                break;
            default: //set Standard values
                SetAllValues(3, 3, 4, 0.1f, 2.5f, 0.1f,40,0, 2,5,false);
                break;
        }
    }

    //Where copied SetAllValues code is: replace with SetAllValues(gm.whatever,..);

    public void SetAllValues(int maxGap, int minGap, float hozPilSpace, float slowDown, float lineLen, float lineThick, float lineSpd, float lineSpdInc, int hungerDeplete, int foodInc, bool reticleOn)
    {
        //Set all values based on GameManager
        pilSpawn.maxGapSize = maxGap;
        pilSpawn.minGapSize = minGap;
        pilSpawn.horizontalSpacing = hozPilSpace;
        aimScript.safeZoneSlowDown = slowDown;
        aimScript.UpdateLine(lineLen, lineThick);
        aimScript.turnSpeed = lineSpd;
        aimScript.turnSpeedIncrease = lineSpdInc;
        hungyScript.depleteBy = hungerDeplete;
        hungyScript.increaseAmount = foodInc;
        TargetScript.instance.transform.GetChild(0).gameObject.SetActive(reticleOn);

        //Set initial input values
        PillarMaxGap.value = pilSpawn.maxGapSize;
        PillarMinGap.value = pilSpawn.minGapSize;
        PillarSpacing.value = pilSpawn.horizontalSpacing;
        SafeSlowDown.value = aimScript.safeZoneSlowDown;
        AimLineLength.value = aimScript.lineLength;
        AimLineThickness.value = aimScript.lineThickness;
        //Set new initial input values
        AimLineSpeed.value = aimScript.turnSpeed;
        AimLineSpeedIncrease.value = aimScript.turnSpeedIncrease;
        HungerDepleteAmount.value = hungyScript.depleteBy;
        FoodIncreaseAmount.value = hungyScript.increaseAmount;
        reticleToggle.isOn = reticleOn;

        //Set initial label values
        minGapTxt.text = pilSpawn.minGapSize.ToString();
        maxGapTxt.text = pilSpawn.maxGapSize.ToString();
        spacingTxt.text = pilSpawn.horizontalSpacing.ToString();
        safeSlowDownTxt.text = aimScript.safeZoneSlowDown.ToString();
        lineLengthTxt.text = aimScript.lineLength.ToString();
        lineThicknessTxt.text = aimScript.lineThickness.ToString();
        //Set new initial label values
        lineSpeedTxt.text = aimScript.turnSpeed.ToString();
        lineSpeedIncreaseTxt.text = aimScript.turnSpeedIncrease.ToString();
        hungerSpeedTxt.text = hungyScript.depleteBy.ToString();
        foodIncreaseTxt.text = hungyScript.increaseAmount.ToString();
    }
    IEnumerator lateStart()
    {
        yield return null;
        Start();
    }
}
