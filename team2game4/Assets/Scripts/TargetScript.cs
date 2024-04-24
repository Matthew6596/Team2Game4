using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        gm.target = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pillar"))
        {
            gm.targetCollidingObj = other.gameObject;
            if (gm.nextPillar != other.gameObject)
            {
                gm.currPillar = gm.nextPillar;
                gm.nextPillar = other.gameObject;
            }
            Debug.Log("pillar");
        }
        else if (other.gameObject.CompareTag("SafeZone"))
        {
            gm.targetCollidingObj = other.gameObject;
            gm.nextOpening = other.gameObject;
            Debug.Log("safe");
        }
    }
}
