using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        gameObject.transform.position = target.transform.position;
    }
}
