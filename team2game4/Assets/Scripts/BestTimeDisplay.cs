using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestTimeDisplay : MonoBehaviour
{
    public TMP_Text bestTimeTxt, prevTimeTxt;

    // Start is called before the first frame update
    void Start()
    {
        if (bestTimeTxt != null) bestTimeTxt.text += TimeTracker.FloatToString(GameManager.gm.bestSessionTime);
        if (prevTimeTxt != null) prevTimeTxt.text += TimeTracker.FloatToString(GameManager.gm.prevTime);
    }
}
