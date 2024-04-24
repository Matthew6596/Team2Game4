using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    public GameObject player;

    TMP_Text countdownTxt;
    int cnt = 3;

    // Start is called before the first frame update
    void Start()
    {
        countdownTxt = GetComponent<TMP_Text>();
        StartCoroutine(countDown());
    }

    IEnumerator countDown()
    {
        yield return new WaitForSeconds(0.75f);
        cnt--;
        countdownTxt.text = (cnt==0)?("GO!"):cnt.ToString();

        if(cnt>=0) StartCoroutine(countDown());
        else EndCountdown();
    }

    void EndCountdown()
    {
        //enable player input
        //start hunger

        //hide countdown text
        gameObject.SetActive(false);
    }
}
