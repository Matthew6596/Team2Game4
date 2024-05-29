using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    GameManager gm;
    public bool dead = false, won=false;
    PlayerInput inp;

    public AudioClip splatSfx, foodGetSfx, winSFX, deathSFX;
    AudioSource src;

    Coroutine winCo;

    public GameObject flashImgObj;
    Image flashImgRenderer;

    //New vars
    public bool isMoving = false;
    public float duration = 1.0f;

    Vector3 targetPos; //made this global

    bool isFirstJump = true;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        inp = GetComponent<PlayerInput>();
        src = GetComponent<AudioSource>();

        flashImgRenderer = flashImgObj.GetComponent<Image>();
        flashImgRenderer.color = new Color(255, 255, 255, 0);
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

        flashImgRenderer.color = new Color(255,255,255,Tween.LazyTween(flashImgRenderer.color.a, 0, 0.04f));
    }

    public void Jump(InputAction.CallbackContext ctx)
    {   if(ctx.performed &&!won&&!DebugMenu.instance.mouseOverBtn && !DebugMenu.instance.open)
        {
            gm.ChangeStateTo(SlimeAnimationState.Jump);
            Debug.Log("click");
            Vector3 startPos = gameObject.transform.position;
            bool isSafe;
            
            if (gm.targetCollidingObj == gm.nextPillar)
            {
                Debug.Log("pillarzone");
                isSafe = false;
                targetPos =  new Vector3(gameObject.transform.position.x - 0.75f, gm.nextPillar.transform.position.y, 0);
                StartCoroutine(MoveToTargetPos(startPos, targetPos, isSafe));
                //gameObject.transform.position = targetPos;

                dead = true;
                StartCoroutine("GameOver");

            }
            else if (gm.targetCollidingObj == gm.nextOpening)
            {
                Debug.Log("safezone");
                isSafe = true;
                targetPos = new Vector3(gameObject.transform.position.x, gm.nextOpening.transform.position.y, gm.nextOpening.transform.position.z);
                StartCoroutine(MoveToTargetPos(startPos, targetPos, isSafe));
                //gameObject.transform.position = targetPos;
            }

            PillarSpawn.instance.MovePillars();

            //Play sound after jump
            if(dead && !PillarSpawn.instance.pillarMoving)
            {
                
            }
            else if (!PillarSpawn.instance.pillarMoving)
            {
                
            }

        }
    }

    IEnumerator MoveToTargetPos(Vector3 startPos, Vector3 targetPos, bool isSafeT)
    {
        //Movement Code based on code by Programmer on StackOverflow
        //https://stackoverflow.com/questions/36850253/move-gameobject-over-time/36851965#36851965

        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        //float xDifference = Mathf.Abs(targetPos.x - startPos.x);
        float yDifference = Mathf.Abs(targetPos.y - startPos.y);

        //duration -= yDifference/100;
        duration = PillarSpawn.instance.horizontalSpacing / PillarSpawn.instance.moveSpeed;

        float ctr = 0;

        while (ctr < duration)
        {
            ctr += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, targetPos, ctr / duration);
            yield return null;
        }

        isMoving = false;

        if(isSafeT) //Food collect feedback
        {
            src.PlayOneShot(foodGetSfx);
            PlayerSqaushStretch.Instance.FoodCollect();

            Vector3 particlePos = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
            Instantiate(gm.foodVFX, particlePos, gm.mainSlime.transform.rotation);

            //Remember what the turn speed was on first jump, before increasing it
            if (isFirstJump) {
                AimingScript.instance.initialTurnSpeed = AimingScript.instance.turnSpeed;
                isFirstJump = false;
            }

            TargetScript.instance.IncreaseTurnSpeed(1); //Every time player jumps, aim line turn speed increases
        }
        else //Pillar hit feedback
        {
            flashImgRenderer.color = new Color(255, 255, 255, 1);

            Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Instantiate(gm.deathVFX, pos, gm.deathVFX.transform.rotation);

            src.PlayOneShot(splatSfx);
            src.PlayOneShot(deathSFX, 3.5f); //play deathSFX now so it's in sync

            //Function is Empty right now, you don't have to use this if you don't want
            PlayerSqaushStretch.Instance.PillarHit();

            //Reset turn speed when player die
            AimingScript.instance.turnSpeed = AimingScript.instance.initialTurnSpeed;
            isFirstJump = true;
        }
    }

    IEnumerator GameOver()
    {
        if(gm.stomachMeter <= 0)
        {
            src.PlayOneShot(deathSFX, 3.5f);
            flashImgRenderer.color = new Color(255, 255, 255, 1);
        }

        HungerScript.instance.depleteActive = false;
        //src.PlayOneShot(deathSFX, 3.5f);
        //flashImgRenderer.color = new Color(255, 255, 255, 1);

        inp.enabled = false;
        gm.prevTime = TimeTracker.instance.time; //don't set best time, didn't win

        yield return new WaitForSeconds(2);
        MenuScript.changeScene("GameOver");
    }

    IEnumerator Win()
    {
        HungerScript.instance.depleteActive = false;
        src.PlayOneShot(winSFX);

        inp.enabled = false; won = true;
        gm.prevTime = TimeTracker.instance.time;
        if (gm.prevTime < gm.bestSessionTime) gm.SetBestTime(); //Smaller/Faster time is better

        yield return new WaitForSeconds(2);
        MenuScript.changeScene("WinScene");
    }

    private void OnTriggerEnter(Collider other)
    {

        /*if(other.gameObject.CompareTag("Pillar"))
        {
            Vector3 pos = gameObject.transform.position;
            if (gameObject.transform.position.y > targetPos.y)
            {
                pos.y--;
                gameObject.transform.position = pos;
                //pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, gameObject.transform.position.z);
            }
            else if (gameObject.transform.position.y < targetPos.y)
            {
                pos.y++;
                gameObject.transform.position = pos;
                //pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
            }

        }*/
    }

}
