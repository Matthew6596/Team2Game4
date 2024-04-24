using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    GameManager gm;
    public bool dead = false;
    PlayerInput inp;

    public AudioClip splatSfx;
    AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        inp = GetComponent<PlayerInput>();
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.stomachMeter <= 0) 
        {
            gm.stomachMeter = 0;
            StartCoroutine("GameOver");
        }

        if(gm.stomachMeter >= 100)
        {
            gm.stomachMeter = 100;
            StartCoroutine("Win");
        }
        if (dead && inp.enabled) inp.enabled = false;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {   if(ctx.performed &&!DebugMenu.instance.mouseOverBtn && !DebugMenu.instance.open)
        {
            Debug.Log("click");
            gm.ChangeStateTo(SlimeAnimationState.Jump);
            Vector3 targetPos;
            
            if (gm.targetCollidingObj == gm.nextPillar)
            {
                Debug.Log("pillarzone");
                targetPos =  new Vector3(gm.nextPillar.transform.position.x - 1, gm.nextPillar.transform.position.y, 0);
                gameObject.transform.position = targetPos;
                src.PlayOneShot(splatSfx);
                dead = true;
                StartCoroutine("GameOver");

            }
            else if (gm.targetCollidingObj == gm.nextOpening)
            {
                Debug.Log("safezone");
                targetPos = gm.nextOpening.transform.position;
                gameObject.transform.position = targetPos;
            }

            PillarSpawn.instance.MovePillars();
        }
    }

    IEnumerator GameOver()
    {
        inp.enabled = false;
        yield return new WaitForSeconds(2);
        MenuScript.changeScene("GameOver");
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(2);
        MenuScript.changeScene("WinScene");
    }

    /*
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("SafeZone") || other.gameObject.CompareTag("Food"))
        {
            
        }
    }
    */
}
