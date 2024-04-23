using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    GameManager gm;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        gm.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 tempPos = gameObject.transform.position;
        /*{
            tempPos.x++;
            if(target.transform.position.y > gameObject.transform.position.y)
            {
                tempPos.y++;
            }
            else
            {
                tempPos.y--;
            }
            gameObject.transform.position = tempPos;
        } while (tempPos.x != targetPos.x) ; //infinite loop whoops */
    }
}
