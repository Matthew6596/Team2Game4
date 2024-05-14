using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class StartCountdown : MonoBehaviour
{
    PlayerInput playerControl;

    TMP_Text countdownTxt;
    int cnt = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = PillarSpawn.instance.playerControl;
        playerControl.enabled = false;
        countdownTxt = GetComponent<TMP_Text>();
        StartCoroutine(countDown());
        transform.localScale = Vector3.one*2f;
    }

    private void Update()
    {
        transform.localScale = Tween.LazyTween(transform.localScale, Vector3.one, 0.1f);
    }
    IEnumerator countDown()
    {
        yield return new WaitForSeconds(0.75f);
        cnt--;
        countdownTxt.text = (cnt==0)?("GO!"):cnt.ToString();
        transform.localScale = Vector3.one * 2f;

        if (cnt>=0) StartCoroutine(countDown());
        else EndCountdown();
    }

    void EndCountdown()
    {
        //enable player input
        playerControl.enabled = true;
        //start hunger

        //hide countdown text
        gameObject.SetActive(false);
    }
}
