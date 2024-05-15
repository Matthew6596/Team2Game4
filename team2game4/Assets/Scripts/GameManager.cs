using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations; //why isn't this being used :(   //we just want it to *feel* like it's included
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject Instance;
    public static GameManager gm;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject;
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public MenuScript menuScript;
    public HungerScript hungerScript;
    public int stomachMeter = 50;

    public float bestSessionTime=float.MaxValue;
    public float prevTime=float.MaxValue;

    public GameObject foodVFX;
    public GameObject deathVFX;

    [Header("Player Movement")]
    public GameObject target;

    [Space]
    [Header("Pillars")]
    public GameObject currPillar;
    public GameObject nextPillar;
    public GameObject nextOpening;
    public GameObject targetCollidingObj;

    [Space]
    [Header("Debug Settings")]
    public int minPillarGap; //1-8
    public int maxPillarGap;
    public float pillarSpacing;
    public float safeZoneSlowDown; //0-1
    public float aimLineThickness, aimLineLength, aimLineSpeed; //0-0.5, 0-5, 1-100
    public float aimLineSpeedIncrease; //0-1
    public int hungerDepleteAmount; //1-10
    public int foodIncreaseAmount; //1-10
    public bool reticleOn;

    [Space]
    [Header("Slime Stuff")]
    //Slime Stuff
    public GameObject mainSlime;
    public Button idleBut, walkBut,jumpBut,attackBut,damageBut0,damageBut1,damageBut2;
    public Camera cam;
    private void Start()
    {
        Idle();
        /*
        idleBut.onClick.AddListener( delegate { Idle(); } );
        walkBut.onClick.AddListener(delegate {  ChangeStateTo(SlimeAnimationState.Walk); });
        jumpBut.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Jump); });
        attackBut.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Attack); });
        damageBut0.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<EnemyAi>().damType = 0; });
        damageBut1.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<EnemyAi>().damType = 1; });
        damageBut2.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<EnemyAi>().damType = 2; });
        */
    }

    private void Update()
    {
        if (mainSlime == null)
        {
            mainSlime = GameObject.Find("Slime_03 Sprout");
        }
    }

    void Idle()
    {
        //LookAtCamera();
        //mainSlime.GetComponent<EnemyAi>().CancelGoNextDestination();
        ChangeStateTo(SlimeAnimationState.Idle);
    }
    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (mainSlime == null) {
            Debug.Log("null slime");
            return; }
        if (state == mainSlime.GetComponent<EnemyAi>().currentState) {
            //Debug.Log("Already in state");
            return; }

       mainSlime.GetComponent<EnemyAi>().currentState = state ;
    }
    void LookAtCamera()
    {
       mainSlime.transform.rotation = Quaternion.Euler(new Vector3(mainSlime.transform.rotation.x, cam.transform.rotation.y, mainSlime.transform.rotation.z));   
    }

    public void StartWithPreset(int preset)
    {
        StartCoroutine(setPreset(preset));
    }
    IEnumerator setPreset(int p)
    {
        //Wait till scene is on sample scene (debug menu is only on sample scene)
        while (SceneManager.GetActiveScene().name != "SampleScene")
        {
            yield return null;
        }
        yield return null;


        //Set preset

        if (p != 3) //custom
        {
            DebugMenu.instance.SelectPreset(p);
            DebugMenu.instance.SetGMValues();

            //Reset Scene
            stomachMeter = 50;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            DebugMenu.instance.transform.GetChild(1).gameObject.SetActive(true);
        }

    }
}
