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

    Transform hungerParent;
    public Image hungerBar;
    public Transform hungerIcon;

    public bool depleteActive=true;

    AudioSource src;
    public AudioClip bompSfx;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        gm.hungerScript = this;
        instance = this;
        src = GetComponent<AudioSource>();
        percent = gameObject.GetComponent<TMP_Text>();
        InvokeRepeating("Deplete", 3.0f, depleteRate);
        hungerParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //Percent text
        percent.text = gm.stomachMeter.ToString() + "%";

        //Make UI naturally go to normal scale
        hungerParent.localScale = Tween.LazyTween(hungerParent.localScale, Vector3.one, 0.04f);
        hungerIcon.localScale = Tween.LazyTween(hungerIcon.localScale, Vector3.one* 0.175f, 0.04f);
        transform.localScale = Tween.LazyTween(transform.localScale, Vector3.one, 0.04f);

        //Bar fill tweens
        hungerBar.fillAmount = Tween.LazyTween(hungerBar.fillAmount, gm.stomachMeter / 100f, 0.04f);
        
        //Wobble when low
        hungerParent.rotation = Tween.Wobble(10, HungerSeverityPercent()*45);
    }

    public float HungerSeverityPercent()
    {
        //I'd like to thank desmos for the seemingly random numbers and math going on here
        return ((float)Math.Pow(1 - (gm.stomachMeter / 100f) - .3, 7));
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

            if (HungerSeverityPercent() * 120 > 1)
            {
                hungerIcon.localScale = Vector3.one * .22f;
                transform.localScale = Vector3.one * 1.26f;
                //Play the "Bomp, bomp, bomp" sfx
                src.PlayOneShot(bompSfx,3f);
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

        hungerParent.localScale = Vector3.one * 1.25f;
    }
}
