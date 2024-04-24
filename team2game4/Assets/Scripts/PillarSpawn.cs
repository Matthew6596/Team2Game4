using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PillarSpawn : MonoBehaviour
{
    public static PillarSpawn instance;

    public GameObject pillarBlock;
    public GameObject safeSpot;

    public int minGapSize, maxGapSize;
    public float horizontalSpacing, moveSpeed;

    public GameObject[] foodPrefabs;

    public List<GameObject> Pillars = new();

    public GameObject player;
    [NonSerialized]
    public PlayerInput playerControl;

    [NonSerialized]
    public bool pillarMoving=false;

    private void Start()
    {
        instance = this;
        StartPillars();

        if (player == null)
        {
            player = new GameObject("missingPlayerReferenceIn_PillarSpawn");
        }
        else
        {
            playerControl = player.GetComponent<PlayerInput>();
        }
    }

    public void StartPillars()
    {
        Pillars.Clear();
        SpawnNewPillar();
        SpawnNewPillar();
        SpawnNewPillar();
        Pillars[0].transform.position = transform.position + (3 * horizontalSpacing * Vector3.left);
        Pillars[1].transform.position = transform.position+(2 * horizontalSpacing * Vector3.left);
        Pillars[2].transform.position = transform.position+(horizontalSpacing * Vector3.left);
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

        if (Pillars.Count >= 9)
        {
            Destroy(Pillars[0]);
            Pillars.RemoveAt(0);
        }
    }

    public void MovePillars()
    {
        SpawnNewPillar();

        if(!player.GetComponent<PlayerMovement>().dead)
            removePillarFruit();
        AimingScript.instance.gameObject.SetActive(false);
        //Disable player control << to do
        playerControl.enabled = false;

        if (!pillarMoving)
        {
            //Making sure each pillar ends in *exact* correct position
            float[] endPositions = new float[Pillars.Count+1];
            for (int i = 0; i < Pillars.Count; i++) endPositions[i] = Pillars[i].transform.position.x - horizontalSpacing;
            endPositions[Pillars.Count] = player.transform.position.x - horizontalSpacing;

            //Move Pillars
            //and player
            Pillars.Add(player);
            StartCoroutine(movePillars(Pillars.ToArray(),endPositions));
            Pillars.Remove(player);

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
            if (p.transform.childCount > 8) return p.transform;
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
                    //make particle here maybe
                    Destroy(c);
                }
            }
    }

}
