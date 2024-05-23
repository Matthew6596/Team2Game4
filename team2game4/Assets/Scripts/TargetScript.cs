using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public static TargetScript instance;

    string prevCollide = "";
    GameManager gm;
    AimingScript aim;

    public bool aimSpeedSlower=false;

    Transform reticle;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gm = GameManager.gm;
        gm.target = gameObject;

        reticle = transform.GetChild(0);
        aim = AimingScript.instance;
    }

    private void Update()
    {
        reticle.rotation = Tween.Wobble(6, 70);
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

            //aim slow down
            if (prevCollide == "SafeZone")
            {
                aim.turnSpeed /= 1-aim.safeZoneSlowDown;
                aimSpeedSlower = true;
            }
        }
        else if (other.gameObject.CompareTag("SafeZone"))
        {
            gm.targetCollidingObj = other.gameObject;
            gm.nextOpening = other.gameObject;

            //aim slow down
            if (prevCollide == "Pillar")
            {
                aim.turnSpeed *= 1-aim.safeZoneSlowDown;
                aimSpeedSlower = false;
            }
        }

        prevCollide = other.gameObject.tag;
    }

    public void IncreaseTurnSpeed(float turnSpeedIncrease)
    {
        if (aimSpeedSlower) turnSpeedIncrease *= 1 - aim.safeZoneSlowDown;
        turnSpeedIncrease *= (aim.turnSpeed > 0) ? 1 : -1;
        aim.turnSpeed += turnSpeedIncrease;
    }
}
