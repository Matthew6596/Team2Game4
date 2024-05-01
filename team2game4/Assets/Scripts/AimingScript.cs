using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    public static AimingScript instance;

    public float lineLength;
    public float lineThickness;
    public float turnSpeed;
    public float safeZoneSlowDown;
    public float turnSpeedIncrease;

    public Transform player;

    public float maxPillarHeight;
    public float playerDistFromPillar;

    public Transform target;

    float maxAng, minAng;
    bool swapMinMax = false;

    Transform lineSpriteObj;

    bool secPass = false;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gm = GameManager.gm;

        lineSpriteObj = transform.GetChild(0);
        lineSpriteObj.localScale = new Vector3(lineLength, lineThickness, 1);
        lineSpriteObj.localPosition = new Vector3(lineLength / 2, 0, 0);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        //temp
        minAng = 0;
        maxAng = 270;

        UpdateMinMaxAngles();
        StartCoroutine(SecTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if(turnSpeedIncrease > 0 && secPass)
        {
            Debug.Log("hi");
            if (turnSpeedIncrease <= 99)
            {
                IncreaseSpeed();
                if(turnSpeed > 0)
                {
                    turnSpeed += turnSpeedIncrease;
                }
                else
                {
                    turnSpeed -= turnSpeedIncrease;
                }
            }
            else
                turnSpeed = 100;
        }
        //Rotate
        transform.Rotate(turnSpeed * Time.deltaTime*Vector3.forward);

        //Check for turning around
        float zAng = transform.rotation.eulerAngles.z;
        if (swapMinMax)
        {
            if (zAng <= maxAng && zAng >= minAng)
            {
                TurnAround();
            }
        }
        else
        {
            if (zAng >= maxAng || zAng <= minAng)
            {
                TurnAround();
            }
        }

        //Move target
        float _spacing = PillarSpawn.instance.horizontalSpacing;
        target.position = new Vector3(player.position.x+_spacing, Mathf.Tan(transform.rotation.eulerAngles.z*Mathf.Deg2Rad) * _spacing + player.position.y,0);
    }

    void TurnAround()
    {
        turnSpeed *= -1;
        //turn extra to avoid getting stuck
        transform.Rotate(turnSpeed * Time.deltaTime * Vector3.forward);
    }

    public void UpdateMinMaxAngles()
    {
        //Get min/max angles
        maxAng = Mathf.Rad2Deg * Mathf.Atan2(maxPillarHeight - player.position.y, playerDistFromPillar);
        minAng = Mathf.Rad2Deg * Mathf.Atan2(-player.position.y, playerDistFromPillar);

        //Shenanigans
        swapMinMax = false;
        if (minAng < 0)
        {
            minAng += 360;
            float tmp = minAng;
            minAng = maxAng;
            maxAng = tmp;
            swapMinMax = true;
        }
    }

    public void SetRotation(float ang)
    {
        transform.rotation = Quaternion.Euler(0, 0, ang);
    }

    public void UpdateLine(float length, float thickness)
    {
        lineLength = length; lineThickness = thickness;
        lineSpriteObj.localScale = new Vector3(lineLength, lineThickness, 1);
        lineSpriteObj.localPosition = new Vector3(lineLength / 2, 0, 0);
    }

    void IncreaseSpeed()
    {
        if (turnSpeedIncrease >= 10)
        {
            turnSpeedIncrease += 10;
        }
        else
        {
            turnSpeedIncrease = 10;
        }
    }

    IEnumerator SecTimer()
    {
        secPass = false;
        yield return new WaitForSeconds(15);
        secPass = true;
        yield return null;
        StartCoroutine(SecTimer());
    }
}
