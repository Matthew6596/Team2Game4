using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTweenOn : MonoBehaviour
{
    public float startDelay;
    public float tweenRate;

    public Transform startTransform;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        //Temp transform to swap
        GameObject g = new();
        Transform t = g.transform;
        t.SetPositionAndRotation(transform.position, transform.rotation);
        t.localScale = transform.localScale;

        //Swap object transform with startTransform
        transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
        transform.localScale = startTransform.localScale;

        startTransform.SetPositionAndRotation(t.position,t.rotation);
        startTransform.localScale = t.localScale;

        //Destroy temp transform
        Destroy(g);

        StartCoroutine(delayAtStart());   
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            //Start transform swapped positions with the original transform in Start
            Tween.LazyTween(transform, startTransform, tweenRate);
        }
    }

    IEnumerator delayAtStart()
    {
        yield return new WaitForSeconds(startDelay);
        started = true;
    }
}
