using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossV2 : MonoBehaviour
{
    public static AIBossV2 Instance { get; private set; }
    [System.NonSerialized] public PathfindingV2 pathfindingV2;
    Vector3 positionToCenter;
    [System.NonSerialized] public Animator animator;


    [System.NonSerialized] public bool playerOnTrigger;
    [System.NonSerialized] public bool flashlightTrigger;

    [SerializeField] Transform tp1;
    [SerializeField] Transform tp2;
    public Transform tp3;

    Transform grade3hp;
    Transform grade2hp;
    Transform grade1hp;

    public enum BossStateList
    {
        Routing, Charging, Dashing, Stun, Attacking,WaitingForNextActivation
    }
    public BossStateList BossState;

    [Header("Atributes")]
    public float dashSpeed;
    public int hp = 4;

    [Header("Audio Sources")]
    public AudioSource audioSourceOneShot;
    public AudioSource audioSourcePreAttack;
    public AudioSource audioSourceDashExtended;

    [Header("Audio Clips")]
    public AudioClip bossMelee;
    public AudioClip bossDeath;
    public AudioClip bossHurt;
    public AudioClip dashExtended;
    public AudioClip preAttack;
    public AudioClip wall;
    public AudioClip wallRock;

    [System.NonSerialized]public float bossMeleeVolume=1;
    [System.NonSerialized]public float bossDeathVolume=1;
    [System.NonSerialized]public float bossHurtVolume=1;
    [System.NonSerialized]public float wallVolume=1;
    [System.NonSerialized]public float wallRockVolume=1;
    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        pathfindingV2 = GetComponent<PathfindingV2>();
        positionToCenter = GetComponent<CapsuleCollider2D>().bounds.center - transform.position;
        grade3hp = GameObject.Find("GradeBlock3Vidas").transform;
        grade2hp = grade3hp.parent.transform.Find("GradeBlock2Vidas");
        grade1hp = grade3hp.parent.transform.Find("GradeBlock1Vidas");
        gameObject.SetActive(false);
    }
    private void Start()
    {
        target = PMStateManager.Instance.gameObject;
    }
    public void On1HpEnable()
    {
        target = pathfindingV2.player.gameObject;
        animator.ResetTrigger("HitWall");
        BossState = BossStateList.WaitingForNextActivation;
        StartCoroutine(On1Hp());
    }
    private IEnumerator On1Hp()
    {
        while (true)
        {
            if (BossState==BossStateList.WaitingForNextActivation) animator.SetTrigger("Preparation");
            yield return new WaitForFixedUpdate();
        }
    }
    private void FixedUpdate()
    {
        if (pathfindingV2.route.Count > 0 & followRoute == null)
        {
            followRoute = StartCoroutine(FollowRoute());
        }
    }
    public Coroutine followRoute;
    public IEnumerator FollowRoute()
    {
        BossState = BossStateList.Routing;
        while (pathfindingV2.route.Count > 0)
        {
            Vector3 deltaPos = (MathMethods.GridToWorld(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.route[0].gridPosition) - positionToCenter) - transform.position;
            deltaPos = Vector3.Normalize(deltaPos);
            float distance = Vector2.Distance(transform.position, MathMethods.GridToWorld(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.route[0].gridPosition) - positionToCenter);
            while (distance > 0.1f)
            {
                transform.position += deltaPos * pathfindingV2.moveSpeed * Time.fixedDeltaTime;
                if (pathfindingV2.route.Count > 0) distance = Vector2.Distance(transform.position, MathMethods.GridToWorld(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.route[0].gridPosition) - positionToCenter);
                else
                {
                    followRoute = null;
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
            pathfindingV2.route.Remove(pathfindingV2.route[0]);
            yield return new WaitForFixedUpdate();
        }
        followRoute = null;
    }

    public void TakeDamage()
    {
        hp--;
        animator.SetTrigger("HitHurt");
        audioSourceOneShot.PlayOneShot(bossHurt,bossHurtVolume);
    }
    // Animator Commands 
    public void ChangeBossState(BossStateList bossStateList)
    {
        BossState = bossStateList;
        if (bossStateList == BossStateList.Charging)
        {
            audioSourcePreAttack.clip = preAttack;
            audioSourcePreAttack.Play();
        }
    }

    //Teleport
    public void TpEnd()
    {
        Teleport();
        animator.SetInteger("Phases", (5 - hp));
        switch (hp)
        {
            default:
            case 3:
                StartCoroutine(moveObject(new Vector3(0, -1.6f), grade3hp.transform, 1));
                break;
            case 2:
                StartCoroutine(moveObject(new Vector3(0, -1.6f), grade2hp.transform, 1));
                break;
            case 1:
                StartCoroutine(moveObject(new Vector3(0.9f, 0), grade1hp.transform, 1));
                break;
        }
    }
    private IEnumerator moveObject(Vector3 deltaPos, Transform transformObject, float speed)
    {
        Vector3 startPos = transformObject.position;
        Vector3 n_deltaPos = Vector3.Normalize(deltaPos);
        while (Vector2.Distance(transformObject.position, startPos + deltaPos) > 0.1f)
        {
            transformObject.position += n_deltaPos * speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    public void Teleport()
    {
        switch (hp)
        {
            case 3:
                transform.position = tp1.transform.position;
                break;
            case 2:
                transform.position = tp2.transform.position;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.transform.Find("TriggerDetector").GetComponent<BoxCollider2D>().enabled = false;
                gameObject.transform.Find("HitBoxDamage").GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

                break;
        }
    }
    //Attack Melee 
    public void IfPlayerOnTriggerDoDamage()
    {
        if (playerOnTrigger)
        {
            PMStateManager.Instance.TakeDamage(1);
        }
    }
    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.25f);
        if (playerOnTrigger)
        {
            animator.SetTrigger("Melee");
            audioSourceOneShot.PlayOneShot(bossMelee,bossMeleeVolume);
        }
        else
        {
            followRoute = null;
        }
    }
    //Dash
    Vector3 DashDeltaPos;
    [System.NonSerialized] public GameObject target;
    public void StartEnragedDash()
    {
        BossState = BossStateList.Dashing;
        pathfindingV2.StopAllPathCheck();
        pathfindingV2.search = false;
        pathfindingV2.route = new List<NodeInfo>();
        transform.Find("TriggerDetector").gameObject.AddComponent<TriggerDetectionOnDash>();
        audioSourceDashExtended.clip = dashExtended;
        audioSourceDashExtended.Play();
        if(audioSourcePreAttack.isPlaying)audioSourcePreAttack.Stop();
        enragedDash = StartCoroutine(EnragedDash());
    }
    public void DefineDashDeltaPos()
    {
        DashDeltaPos = Vector3.Normalize(target.transform.position - positionToCenter - transform.position);
    }
    [System.NonSerialized] public Coroutine enragedDash;
    private IEnumerator EnragedDash()
    {
        while (true)
        {
            transform.position += DashDeltaPos * dashSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
