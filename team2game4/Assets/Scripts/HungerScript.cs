using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HungerScript : MonoBehaviour
{
    GameManager gm;
    TMP_Text percent;

    public float depleteRate = 2.0f;
    public int depleteBy = 1;
    public int increaseAmount = 5;

    public static HungerScript instance;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        gm.hungerScript = this;
        instance = this;
        percent = gameObject.GetComponent<TMP_Text>();
        InvokeRepeating("Deplete", 3.0f, depleteRate);
    }

    // Update is called once per frame
    void Update()
    {
        percent.text = gm.stomachMeter.ToString() + "%";
    }

    public void Deplete()
    {
        percent.color = Color.white;
        if (gm.stomachMeter > 0)
        {
            gm.stomachMeter = gm.stomachMeter - depleteBy;
        }
        else if(gm.stomachMeter >= 100)
        {
            gm.stomachMeter = 100;
        }
        else
        {
            gm.stomachMeter = 0;
        }
        
    }

    public void Increase()
    {
        if(gm.stomachMeter < 100 && gm.stomachMeter <= 100 - increaseAmount)
        {
            gm.stomachMeter = gm.stomachMeter + increaseAmount;
            percent.color = Color.green;
        }
        else if (gm.stomachMeter < 100 && gm.stomachMeter > 100 - increaseAmount)
        {
            gm.stomachMeter = 100;
            percent.color = Color.green;
        }
    }
}
