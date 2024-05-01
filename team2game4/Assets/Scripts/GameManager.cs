using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations; //why isn't this being used :(

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
    void Idle()
    {
        LookAtCamera();
        mainSlime.GetComponent<EnemyAi>().CancelGoNextDestination();
        ChangeStateTo(SlimeAnimationState.Idle);
    }
    public void ChangeStateTo(SlimeAnimationState state)
    {
       if (mainSlime == null) return;    
       if (state == mainSlime.GetComponent<EnemyAi>().currentState) return;

       mainSlime.GetComponent<EnemyAi>().currentState = state ;
    }
    void LookAtCamera()
    {
       mainSlime.transform.rotation = Quaternion.Euler(new Vector3(mainSlime.transform.rotation.x, cam.transform.rotation.y, mainSlime.transform.rotation.z));   
    }
}
