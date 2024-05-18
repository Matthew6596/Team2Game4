using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HungerScript : MonoBehaviour
{
    GameManager gm;
    TMP_Text percent;

    public float depleteRate = 2.0f;
    public int depleteBy = 1;
    public int increaseAmount = 5;

    public static HungerScript instance;

    Transform stomachIcon;
    public Image hungerBar;

    public bool depleteActive=true;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        gm.hungerScript = this;
        instance = this;
        percent = gameObject.GetComponent<TMP_Text>();
        InvokeRepeating("Deplete", 3.0f, depleteRate);
        stomachIcon = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        percent.text = gm.stomachMeter.ToString() + "%";
        float fillPercent = gm.stomachMeter / 100f;
        stomachIcon.localScale = Tween.LazyTween(stomachIcon.localScale, Vector3.one, 0.04f);
        hungerBar.fillAmount = Tween.LazyTween(hungerBar.fillAmount, fillPercent, 0.04f);
        //I'd like to thank desmos for the seemingly random numbers and math going on here
        stomachIcon.rotation = Tween.Wobble(10, ((float)Math.Pow(1-fillPercent-.3,7))*45);
    }

    public void Deplete()
    {
        if (depleteActive)
        {
            percent.color = Color.white;
            if (gm.stomachMeter > 0)
            {
                gm.stomachMeter = gm.stomachMeter - depleteBy;
            }
            else if (gm.stomachMeter >= 100)
            {
                gm.stomachMeter = 100;
            }
            else
            {
                gm.stomachMeter = 0;
            }
        }

        //stomachIcon.localScale = Vector3.one * 0.14f;
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

        stomachIcon.localScale = Vector3.one * 1.25f;
    }
}
