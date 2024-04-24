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

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        gm.hungerScript = this;
        percent = gameObject.GetComponent<TMP_Text>();
        InvokeRepeating("Deplete", 3.0f, depleteRate);
    }

    // Update is called once per frame
    void Update()
    {
        percent.text = gm.stomachMeter.ToString() + "%";
    }

    void Deplete()
    {
        if(gm.stomachMeter > 0)
        {
            gm.stomachMeter--;
        }
        else
        {
            gm.stomachMeter = 0;
        }
        
    }

    public void Increase()
    {
        Debug.Log("Food!");
        if(gm.stomachMeter < 100 && gm.stomachMeter <= 95)
        {
            gm.stomachMeter = gm.stomachMeter + 5;
        }
        else if (gm.stomachMeter < 100 && gm.stomachMeter > 95)
        {
            gm.stomachMeter = 100;
        }
    }
}
