using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition instance;

    public float tweenRate;

    public Transform curtain1, curtain2, curtain3, curtain4, curtainsOffscreenPos, curtainsOnscreenPos;
    bool tween1,tween2,tween3, tween4;

    float offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = 25*((float)Screen.width/800);
        Debug.Log(offset+", "+Screen.width);

        instance = this;
        tween1 = tween2 = tween3 = tween4 = false;
        curtain1.position = curtainsOffscreenPos.position;
        curtain2.position = curtainsOffscreenPos.position + Vector3.right * offset;
        curtain3.position = curtainsOffscreenPos.position + Vector3.down * offset;
        curtain4.position = curtainsOffscreenPos.position + new Vector3(offset,-offset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        curtain1.position = Tween.LazyTween(curtain1.position, (tween1) ? curtainsOnscreenPos.position : curtainsOffscreenPos.position, tweenRate);
        curtain2.position = Tween.LazyTween(curtain2.position, ((tween2) ? curtainsOnscreenPos.position : curtainsOffscreenPos.position)+Vector3.right*offset, tweenRate);
        curtain3.position = Tween.LazyTween(curtain3.position, ((tween3) ? curtainsOnscreenPos.position : curtainsOffscreenPos.position) + Vector3.down * offset, tweenRate);
        curtain4.position = Tween.LazyTween(curtain4.position, ((tween4) ? curtainsOnscreenPos.position : curtainsOffscreenPos.position)+new Vector3(offset, -offset, 0), tweenRate);
    }

    public void ChangeScene(string sceneName, float offsetTime, float endTime)
    {
        StartCoroutine(changeScene(sceneName, offsetTime, endTime));
    }
    IEnumerator changeScene(string sceneName, float t, float tt)
    {
        tween1 = true;
        yield return new WaitForSeconds(t);
        tween2 = true;
        yield return new WaitForSeconds(t);
        tween3 = true;
        yield return new WaitForSeconds(t);
        tween4 = true;
        yield return new WaitForSeconds(tt);
        
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(t);

        tween1 = false;
        yield return new WaitForSeconds(t);
        tween2 = false;
        yield return new WaitForSeconds(t);
        tween3 = false;
        yield return new WaitForSeconds(t);
        tween4 = false;
    }
}
