using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public AudioClip carSFX;
    public AudioSource src;
    bool isPlayingSFX = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.x < 350 && !isPlayingSFX)
        {
            src.PlayOneShot(carSFX, 0.1f);
            isPlayingSFX = true;
        }

        if(gameObject.transform.position.x > 400)
            isPlayingSFX=false;
    }

}
