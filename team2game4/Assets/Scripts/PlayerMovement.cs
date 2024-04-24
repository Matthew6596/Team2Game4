using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump(InputAction.CallbackContext ctx)
    {   if(ctx.performed)
        {
            Debug.Log("click");
            //gm.ChangeStateTo(SlimeAnimationState.Jump);
            Vector3 targetPos;
            
            if (gm.targetCollidingObj == gm.nextPillar)
            {
                Debug.Log("pillarzone");
                targetPos =  new Vector3(gm.nextPillar.transform.position.x - 1, gm.nextPillar.transform.position.y, 0);
                gameObject.transform.position = targetPos;
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
        yield return new WaitForSeconds(2);
        MenuScript.changeScene("GameOver");
    }
}
