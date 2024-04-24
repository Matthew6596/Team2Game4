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
        percent.color = Color.white;
        if (gm.stomachMeter > 0)
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
        if(gm.stomachMeter < 100 && gm.stomachMeter <= 98)
        {
            gm.stomachMeter = gm.stomachMeter + 2;
            percent.color = Color.green;
        }
        else if (gm.stomachMeter < 100 && gm.stomachMeter > 98)
        {
            gm.stomachMeter = 100;
            percent.color = Color.green;
        }
    }
}
