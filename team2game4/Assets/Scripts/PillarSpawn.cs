using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PillarSpawn : MonoBehaviour
{
    GameManager gm;

    public static PillarSpawn instance;

    public GameObject pillarBlock;
    public GameObject safeSpot;

    public int minGapSize, maxGapSize;
    public float horizontalSpacing, moveSpeed;

    public GameObject[] foodPrefabs;

    public List<GameObject> Pillars = new();

    public GameObject[] planes;

    public GameObject player;
    [NonSerialized]
    public PlayerInput playerControl;

    [NonSerialized]
    public bool pillarMoving=false;

    private void Start()
    {
        gm = GameManager.gm;
        horizontalSpacing = gm.pillarSpacing;

        instance = this;
        //StartPillars(); //<<HEY!!! if DebugMenu script is not being used at all, uncomment this

        if (player == null)
        {
            player = new GameObject("missingPlayerReferenceIn_PillarSpawn");
        }
        else
        {
            playerControl = player.GetComponent<PlayerInput>();

            //Do math to set players init pos
        }
    }

    public void StartPillars()
    {
        Pillars.Clear();
        int numInitPillars = Mathf.FloorToInt(12/horizontalSpacing);
        for (int i = 0; i <numInitPillars; i++)
        {
            SpawnNewPillar();
            Pillars[i].transform.position = transform.position + ((numInitPillars-i) * horizontalSpacing * Vector3.left);
        }
        player.transform.position = transform.position + ((numInitPillars+1) * horizontalSpacing * Vector3.left);
        while (player.transform.position.x < -.5)
        {
            player.transform.position += Vector3.right;
            for(int i=0; i<numInitPillars; i++) Pillars[i].transform.position += Vector3.right;
            gameObject.transform.position += Vector3.right;
        }
    }

    public void SpawnNewPillar()
    {
        //Calculate gap size and location
        int gapSize = UnityEngine.Random.Range(minGapSize, maxGapSize+1);
        int gapLocation = UnityEngine.Random.Range(1, 10 - gapSize);
        int foodLocation = gapLocation + gapSize/2;

        //Make full pillar
        GameObject pillarParent = new("Pillar");
        pillarParent.tag = "Pillar";
        GameObject[] pillar = new GameObject[10];
        for (int i=0; i<10; i++)
        {
            pillar[i] = Instantiate(pillarBlock,Vector3.up*i,Quaternion.identity, pillarParent.transform);
        }

        //Add gap w/ food to pillar
        for(int i=gapLocation; i < gapLocation + gapSize; i++)
        {
            Destroy(pillar[i]);
            if (i == foodLocation)
            {
                pillar[i] = Instantiate(foodPrefabs[UnityEngine.Random.Range(0, foodPrefabs.Length)], pillarParent.transform);
                pillar[i].transform.position = Vector3.up * i;
            }
            else
            {
                pillar[i] = Instantiate(safeSpot,pillarParent.transform);
                pillar[i].transform.position = Vector3.up * i;
            }
            pillar[i].gameObject.tag = "SafeZone";
        }

        //Position pillar
        pillarParent.transform.position = transform.position;

        //Remember and remove old Pillar
        Pillars.Add(pillarParent);

        if (Pillars.Count >= (1/horizontalSpacing)*40)
        {
            Destroy(Pillars[0]);
            Pillars.RemoveAt(0);
        }
    }

    public void MovePillars()
    {
        SpawnNewPillar();

        if(!player.GetComponent<PlayerMovement>().dead)
        {
            gm.hungerScript.Increase();
            removePillarFruit();
        }
        AimingScript.instance.gameObject.SetActive(false);
        //Disable player control << to do
        playerControl.enabled = false;

        if (!pillarMoving)
        {
            //Making sure each pillar ends in *exact* correct position
            float[] endPositions = new float[Pillars.Count+3];
            for (int i = 0; i < Pillars.Count; i++) endPositions[i] = Pillars[i].transform.position.x - horizontalSpacing;
            //endPositions[Pillars.Count] = player.transform.position.x - horizontalSpacing;
            endPositions[Pillars.Count + 0] = planes[0].transform.position.x - horizontalSpacing;
            endPositions[Pillars.Count + 1] = planes[1].transform.position.x - horizontalSpacing;
            endPositions[Pillars.Count + 2] = planes[2].transform.position.x - horizontalSpacing;


            //Move Pillars
            //and player
            //Pillars.Add(player);
            Pillars.Add(planes[0]);
            Pillars.Add(planes[1]);
            Pillars.Add(planes[2]);
            StartCoroutine(movePillars(Pillars.ToArray(),endPositions));
            //Pillars.Remove(player);
            Pillars.Remove(planes[0]);
            Pillars.Remove(planes[1]);
            Pillars.Remove(planes[2]);

            pillarMoving = true; //set bool to prevent spam
        }
    }

    IEnumerator movePillars(GameObject[] movingObjs,float[] endPos)
    {
        //Move all pillars
        float pos = 0;
        while (pos < horizontalSpacing)
        {
            foreach(GameObject p in movingObjs)
            {
                p.transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);
            }
            pos += moveSpeed * Time.deltaTime;
            yield return null;
        }
        //When movement done, set pillars to exact end position
        for (int i = 0; i < movingObjs.Length; i++)
        {
            Vector3 p = movingObjs[i].transform.position;
            movingObjs[i].transform.position = new Vector3(endPos[i], p.y, p.z);
        }

        //Extra stuff to do when pillars done moving
        pillarMoving = false;
        PillarsDoneMoving();
    }

    public void PillarsDoneMoving()
    {
        AimingScript aim = AimingScript.instance;
        aim.gameObject.SetActive(true);
        aim.UpdateMinMaxAngles();
        aim.SetRotation(0);

        //enable player control << to do
        playerControl.enabled = true;
    }

    public Transform pillarPlayerOn()
    {
        foreach(GameObject p in Pillars)
        {
            if (p.transform.childCount >=10) return p.transform;
        }
        return null;
    }
    public void removePillarFruit()
    {
        Transform p = pillarPlayerOn();
        Debug.Log(p);
        if(p!=null)
            for(int i=p.childCount-1; i>=0; i--)
            {
                GameObject c = p.GetChild(i).gameObject;
                if (c.CompareTag("SafeZone"))
                {
                    //Vector3 pos = new Vector3(p.transform.position.x - 5, c.transform.position.y, c.transform.position.z);
                    //Instantiate(gm.foodVFX, pos, gm.mainSlime.transform.rotation);
                    //make particle here maybe

                    StartCoroutine(WaitToDestroy(c));
                    //Destroy(c);
                }
            }
    }

    //NEW
    IEnumerator WaitToDestroy(GameObject c)
    {
        yield return new WaitForSeconds(player.GetComponent<PlayerMovement>().duration); //Wait for player to stop moving
        Destroy(c);
    }

}
