using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    public float lineLength;
    public float lineThickness;
    public float turnSpeed;

    public Transform player;

    public float maxPillarHeight;
    public float playerDistFromPillar;

    float maxAng, minAng;
    bool swapMinMax = false;

    Transform lineSpriteObj;

    // Start is called before the first frame update
    void Start()
    {
        lineSpriteObj = transform.GetChild(0);
        lineSpriteObj.localScale = new Vector3(lineLength,lineThickness,1);
        lineSpriteObj.localPosition = new Vector3(lineLength / 2, 0, 0);

        transform.rotation = Quaternion.Euler(0, 0, 20);

        //temp
        minAng = 0;
        maxAng = 270;

        UpdateMinMaxAngles();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(turnSpeed * Time.deltaTime*Vector3.forward);
        float zAng = transform.rotation.eulerAngles.z;
        if (swapMinMax)
        {
            if (zAng <= maxAng && zAng >= minAng)
            {
                turnSpeed *= -1;
            }
        }
        else
        {
            if (zAng >= maxAng || zAng <= minAng)
            {
                turnSpeed *= -1;
            }
        }
    }

    public void UpdateMinMaxAngles()
    {
        maxAng = Mathf.Rad2Deg * Mathf.Atan2(maxPillarHeight - player.position.y, playerDistFromPillar);
        minAng = Mathf.Rad2Deg * Mathf.Atan2(-player.position.y, playerDistFromPillar);

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
}
