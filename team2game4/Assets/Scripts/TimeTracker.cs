using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker instance;

    bool started = false;
    public float time { get; private set; }
    public float timeRounded { get{ return RoundTwoPlaces(time); } }
    public string timeString { get { return FloatToString(time); } }

    public TMP_Text timerTxt;
    public float startDelay;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        time = 0;

        TMP_Text _t = gameObject.GetComponent<TMP_Text>();
        if(_t!=null) timerTxt = _t;

        StartCoroutine(startCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            time += Time.deltaTime;

            if(timerTxt!=null) timerTxt.text = "Time: "+timeString;
        }
    }

    public static float RoundTwoPlaces(float num)
    {
        return ((int)(num * 100)) / 100f;
    }
    public static string AddZerosToDecimalString(float floatNum)
    {
        string floatString = floatNum.ToString();
        int dotIndex = floatString.IndexOf('.');

        if (dotIndex == -1) //Whole number (17 -> 17.00)
            return floatString + ".00";

        if (floatString.Length - dotIndex - 1 == 1) //Only one decimal place (17.1 -> 17.10)
            return floatString + "0";

        return floatString; //Two decimal places (17.27 -> 17.27)
    }

    IEnumerator startCountdown()
    {
        yield return new WaitForSeconds(startDelay);
        started = true;
    }

    public static string FloatToString(float num)
    {
        return AddZerosToDecimalString(RoundTwoPlaces(num));
    }
}
