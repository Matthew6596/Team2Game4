using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    GameManager gm;
    public bool dead = false, won=false;
    PlayerInput inp;

    public AudioClip splatSfx, foodGetSfx, winSFX, deathSFX;
    AudioSource src;

    Coroutine winCo;

    //New vars
    bool isMoving = false;

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
        if(gm.stomachMeter <= 0 && !dead) 
        {
            dead = true;
            gm.stomachMeter = 0;
            StartCoroutine("GameOver");
        }

        if(gm.stomachMeter >= 100)
        {
            gm.stomachMeter = 100;
            if(winCo==null)
                winCo = StartCoroutine("Win");
        }
        if (dead && inp.enabled) inp.enabled = false;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {   if(ctx.performed &&!won&&!DebugMenu.instance.mouseOverBtn && !DebugMenu.instance.open)
        {
            Debug.Log("click");
            Vector3 startPos = gameObject.transform.position;
            gm.ChangeStateTo(SlimeAnimationState.Jump);
            Vector3 targetPos;
            
            if (gm.targetCollidingObj == gm.nextPillar)
            {
                Debug.Log("pillarzone");
                targetPos =  new Vector3(gm.nextPillar.transform.position.x - 1, gm.nextPillar.transform.position.y, 0);
                StartCoroutine(MoveToTargetPos(startPos));
                //gameObject.transform.position = targetPos;
                src.PlayOneShot(splatSfx);

                Vector3 pos = new Vector3(gameObject.transform.position.x - 5, gameObject.transform.position.y, gameObject.transform.position.z);
                Instantiate(gm.deathVFX, pos, gm.deathVFX.transform.rotation);

                dead = true;
                StartCoroutine("GameOver");

            }
            else if (gm.targetCollidingObj == gm.nextOpening)
            {
                Debug.Log("safezone");
                src.PlayOneShot(foodGetSfx);
                targetPos = gm.targetCollidingObj.transform.position;
                StartCoroutine(MoveToTargetPos(startPos));
                //gameObject.transform.position = targetPos;
            }

            PillarSpawn.instance.MovePillars();
        }
    }

    IEnumerator MoveToTargetPos(Vector3 startPos)
    {
        if(isMoving)
        {
            yield break;
        }

        Vector3 targetPos = new Vector3(gameObject.transform.position.x, gm.targetCollidingObj.transform.position.y, gm.targetCollidingObj.transform.position.z);

        isMoving = true;

        float ctr = 0;

        while (ctr < 0.75f)
        {
            ctr += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, targetPos, ctr / 0.75f);
            yield return null;
        }

        isMoving = false;
    }

    IEnumerator GameOver()
    {
        src.PlayOneShot(deathSFX);

        inp.enabled = false;
        gm.prevTime = TimeTracker.instance.time; //don't set best time, didn't win

        yield return new WaitForSeconds(2);
        MenuScript.changeScene("GameOver");
    }

    IEnumerator Win()
    {
        src.PlayOneShot(winSFX);

        inp.enabled = false; won = true;
        gm.prevTime = TimeTracker.instance.time;
        if (gm.prevTime < gm.bestSessionTime) gm.bestSessionTime = gm.prevTime; //Smaller/Faster time is better

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
