using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoverJuice : MonoBehaviour
{
    public Vector3 growScale;
    public Vector3 btnDownScale;
    Vector3 defaultScale;
    bool hovered = false;
    bool btnDown = false;

    public float tweenRate;

    public float wobbleSpeed;
    public float wobbleAmount;

    public bool isMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale tweening
        if(!btnDown)
            transform.localScale = Tween.LazyTween(transform.localScale, (hovered) ? growScale : defaultScale, tweenRate);
        else
            transform.localScale = Tween.LazyTween(transform.localScale, btnDownScale, tweenRate);

        //Wobble
        if (hovered)
        {
            transform.localRotation = Tween.Wobble(wobbleSpeed, wobbleAmount);
        }
        
    }
    
    public void ButtonHover()
    {
        //Grow btn
        hovered = true;
    }
    public void ButtonNotHover()
    {
        //Shrink button to normal size
        hovered = false;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
    public void ButtonDown()
    {
        btnDown = true;
    }
    public void ButtonUp()
    {
        btnDown = false;
    }
}

public class Tween
{
    public static float tweenRateMultiplier = 80;
    public static Vector3 LazyTween(Vector3 currentPos, Vector3 targetPos, float rate)
    {
        return Vector3.MoveTowards(currentPos, targetPos, Vector3.Distance(currentPos, targetPos) * rate* Time.unscaledDeltaTime * tweenRateMultiplier);
    }
    public static float LazyTween(float currentNum, float targetNum, float rate)
    {
        if (currentNum < targetNum) return currentNum + (targetNum - currentNum) * rate* Time.unscaledDeltaTime * tweenRateMultiplier;
        else return currentNum + (currentNum - targetNum) * -rate* Time.unscaledDeltaTime * tweenRateMultiplier;
    }
    public static void LazyTween(Transform currentTransform, Transform targetTransform, float rate)
    {
        currentTransform.SetLocalPositionAndRotation(LazyTween(currentTransform.localPosition, targetTransform.localPosition, rate* Time.unscaledDeltaTime * tweenRateMultiplier), Quaternion.Euler(LazyTween(currentTransform.localRotation.eulerAngles, targetTransform.localRotation.eulerAngles, rate* Time.unscaledDeltaTime * tweenRateMultiplier)));
        currentTransform.localScale = LazyTween(currentTransform.localScale, targetTransform.localScale, rate* Time.unscaledDeltaTime * tweenRateMultiplier);
    }
    public static Quaternion Wobble(float speed, float amount,float time){return Quaternion.Euler(0, 0, Mathf.Sin(speed * time) * amount);}
    public static Quaternion Wobble(float speed, float amount) { return Quaternion.Euler(0, 0, Mathf.Sin(speed * Time.unscaledTime) * amount); }

}