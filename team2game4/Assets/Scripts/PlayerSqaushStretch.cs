using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSqaushStretch : MonoBehaviour
{
    public static PlayerSqaushStretch Instance;

    public Vector3 targetScale;
    public float tweenRate;

    //Start & Scale Reset
    public void ScaleReset(){ transform.localScale = Vector3.one; }
    public void TweenReset(float tweenRate){ targetScale = Vector3.one; this.tweenRate = tweenRate; }
    public void TweenTo(Vector3 scale, float tweenRate) {targetScale=scale; this.tweenRate = tweenRate; }
    private void Start(){ Instance = this; ScaleReset(); }

    // Tween!
    void Update()
    {
        transform.localScale = Tween.LazyTween(transform.localScale, targetScale, tweenRate);
    }

    //Food Collect Squash & Stretch Sequence
    public void FoodCollect()
    {
        //Initial growth tween
        TweenTo(new Vector3(1.2f, 1.4f, 1.2f), 0.1f);

        StartCoroutine(collectTweenSequence());
    }
    IEnumerator collectTweenSequence()
    {
        yield return new WaitForSeconds(0.3f);

        //Squish down slightly tween
        TweenTo(new Vector3(1.3f, .8f, 1.3f), 0.1f);

        yield return new WaitForSeconds(0.2f);

        //Reset to regular scale tween
        TweenReset(0.1f); //0.1f = tweenRate for tweening back to normal
    }

    //Pillar Hit
    public void PillarHit()
    {

    }
}
