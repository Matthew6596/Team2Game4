using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpawn : MonoBehaviour
{
    public static PillarSpawn instance;

    public GameObject pillarBlock;

    public int minGapSize, maxGapSize;
    public float horizontalSpacing, moveSpeed;

    public GameObject[] foodPrefabs;

    public List<GameObject> Pillars = new();

    [NonSerialized]
    public bool pillarMoving=false;

    private void Start()
    {
        instance = this;
        StartPillars();
    }

    public void StartPillars()
    {
        Pillars.Clear();
        SpawnNewPillar();
        SpawnNewPillar();
        Pillars[0].transform.position = transform.position+(2 * horizontalSpacing * Vector3.left);
        Pillars[1].transform.position = transform.position+(horizontalSpacing * Vector3.left);
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
                pillar[i] = new GameObject("PillarGap" + i);
                pillar[i].transform.parent = pillarParent.transform;
            }
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

        //Disable player control
        if (!pillarMoving)
        {
            StartCoroutine(movePillars());
            pillarMoving = true;
        }
        pillarMoving = false;
        //Enable player control
    }

    IEnumerator movePillars()
    {
        float pos = 0;
        while (pos < horizontalSpacing)
        {
            foreach(GameObject p in Pillars)
            {
                p.transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);
            }
            pos += moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

}
