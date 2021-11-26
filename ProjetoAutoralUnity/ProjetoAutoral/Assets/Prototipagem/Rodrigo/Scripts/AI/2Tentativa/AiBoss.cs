using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBoss : MonoBehaviour
{
    public static AiBoss Instance { get; private set; }
    [System.NonSerialized] public PathfindingV2 pathfindingV2;
    Vector3 positionToCenter;
    [SerializeField] public GameObject target;
    [SerializeField] Transform tp1;
    [SerializeField] Transform tp2;
    public Transform tp3;

    Transform grade3hp;
    Transform grade2hp;
    Transform grade1hp;


    public Animator animator;
    public bool attacking;

    /*[System.NonSerialized] */
    public int vida = 4;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    private void Start()
    {
        pathfindingV2 = GetComponent<PathfindingV2>();
        positionToCenter = positionToCenter = GetComponent<CapsuleCollider2D>().bounds.center - transform.position;
        target = pathfindingV2.player.gameObject;
        grade3hp = GameObject.Find("GradeBlock3Vidas").transform;
        grade2hp = grade3hp.parent.transform.Find("GradeBlock2Vidas");
        grade1hp = grade3hp.parent.transform.Find("GradeBlock1Vidas");
    }
    private void FixedUpdate()
    {
        if (enragedDash == null && !enragedCharging)
        {
            if (pathfindingV2.route.Count > 0 && followRoute == null)
            {
                followRoute = StartCoroutine(FollowRoute());
            }
        }
    }
    public void On1HpEnable()
    {
        target = pathfindingV2.player.gameObject;
        animator.ResetTrigger("HitWall");
        enragedDash = null;
        StartCoroutine(On1Hp());
    }
    private IEnumerator On1Hp()
    {
        while (true)
        {
            if (enragedDash == null)
            {
                if (!enragedCharging)
                {
                    enragedCharging = true;
                    animator.SetTrigger("Preparation");
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void TakeDamage()
    {
        vida--;
        animator.SetTrigger("HitHurt");
    }
    public void TpEnd()
    {
        Teleport();
        animator.SetInteger("Phases", (5 - vida));
        switch (vida)
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
        switch (vida)
        {
            case 3:
                transform.position = tp1.transform.position;
                break;
            case 2:
                transform.position = tp2.transform.position;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

                break;
        }
    }

    [System.NonSerialized] public bool enragedCharging;
    public void enragedChargingBool(int enragedChargingState)
    {
        if (enragedChargingState == 1)
        {
            enragedCharging = true;
        }
        else
        {
            enragedCharging = false;
        }
    }
    public void StartEnragedDash()
    {
        enragedCharging = false;
        enragedDash = StartCoroutine(EnragedDash());
    }
    public Coroutine followRoute;
    public IEnumerator FollowRoute()
    {
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
    public Coroutine enragedDash;
    public IEnumerator EnragedDash()
    {
        Vector3 deltaPos = (target.transform.position - positionToCenter) - transform.position;
        deltaPos = Vector3.Normalize(deltaPos);
        while (true)
        {
            transform.position += deltaPos * pathfindingV2.dashSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    public void HitWallEnd()
    {
        enragedDash = null;
    }
    public void OnActivationDamagePlayerOnTrigger()
    {
        if (HitBoxMeleeBoss.Instance.playerOnTrigger)
        {
            PMStateManager.Instance.TakeDamage(1);
        }
    }
    public void Attacking(int attackingBool)
    {
        if (attackingBool == 1)
        {
            attacking = true;
        }
        else attacking = false;
    }
    private IEnumerator Stun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        enragedDash = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enragedDash != null)
        {
            ObjectDestroyable OD = collision.GetComponent<ObjectDestroyable>();
            if (collision.CompareTag("Rachadura"))
            {
                pathfindingV2.search = false;
                Debug.Log("Rachadura");
                animator.SetTrigger("HitWall");
                Instantiate(pathfindingV2.pedraPf, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                StopCoroutine(enragedDash);
                pathfindingV2.StopAllPathCheck();
                pathfindingV2.found = false;
                pathfindingV2.route = new List<NodeInfo>();
            }
            else if (collision.name == "ObjetoFlamejante")
            {
                if (collision.transform.Find("FireLight2D").gameObject.activeInHierarchy)
                {
                    pathfindingV2.search = false;
                    animator.SetTrigger("HitWall");
                    StopCoroutine(enragedDash);
                    pathfindingV2.StopAllPathCheck();
                    pathfindingV2.found = false;
                    pathfindingV2.route = new List<NodeInfo>();
                    TakeDamage();
                }
                else
                {
                    pathfindingV2.search = false;
                    Debug.Log("Sofa");
                    animator.SetTrigger("HitWall");
                    StopCoroutine(enragedDash);
                    pathfindingV2.found = false;
                    pathfindingV2.route = new List<NodeInfo>();
                    pathfindingV2.StopAllPathCheck();
                    pathfindingV2.nowSearchingForGrid = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center);
                    pathfindingV2.pathCheckRunning[0] = pathfindingV2.StartCoroutine(pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center), cameFromNode = null }, pathfindingV2.playercc.bounds.center, 0, false));
                    pathfindingV2.search = true;
                    followRoute = null;
                }
            }
            else if (collision.name == "GavetaRachadura")
            {
                collision.transform.Find("ParticleEmissor").GetComponent<ParticleSystem>().Play();
                OD.ChangeSprite();
                collision.enabled = false;
                pathfindingV2.search = false;
                Debug.Log("Gaveta");
                animator.SetTrigger("HitWall");
                StopCoroutine(enragedDash);
                pathfindingV2.found = false;
                pathfindingV2.route = new List<NodeInfo>();
                pathfindingV2.StopAllPathCheck();
                pathfindingV2.nowSearchingForGrid = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center);
                pathfindingV2.pathCheckRunning[0] = pathfindingV2.StartCoroutine(pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center), cameFromNode = null }, pathfindingV2.playercc.bounds.center, 0, false));
                pathfindingV2.search = true;
                followRoute = null;
            }
            else if (OD != null)
            {
                OD.ChangeSprite();
                collision.enabled = false;
            }
            else if (collision.CompareTag("Wall"))
            {
                pathfindingV2.search = false;
                Debug.Log("Wall");
                animator.SetTrigger("HitWall");
                StopCoroutine(enragedDash);
                if (vida != 1)
                {
                    pathfindingV2.found = false;
                    pathfindingV2.route = new List<NodeInfo>();
                    pathfindingV2.StopAllPathCheck();
                    pathfindingV2.nowSearchingForGrid = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center);
                    pathfindingV2.pathCheckRunning[0] = pathfindingV2.StartCoroutine(pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center), cameFromNode = null }, pathfindingV2.playercc.bounds.center, 0, false));
                    pathfindingV2.search = true;
                    followRoute = null;
                }
            }
        }
        else
        {
            if (collision.name == "Flashlights")
            {
                if (enragedDash == null)
                {
                    if (!enragedCharging)
                    {
                        animator.ResetTrigger("CancelPreparation");
                        if (followRoute != null) StopCoroutine(followRoute);
                        enragedCharging = true;
                        animator.SetTrigger("Preparation");
                    }
                }
            }
        }
    }
    private IEnumerator delayStartPathfinding()
    {
        yield return new WaitForSeconds(0.1f);
        pathfindingV2.found = false;
        pathfindingV2.route = new List<NodeInfo>();
        pathfindingV2.StopAllPathCheck();
        pathfindingV2.nowSearchingForGrid = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center);
        pathfindingV2.pathCheckRunning[0] = pathfindingV2.StartCoroutine(pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center), cameFromNode = null }, pathfindingV2.playercc.bounds.center, 0, false));
        pathfindingV2.search = true;
        followRoute = null;
    }
    private void StopAndStun(float stunDuration)
    {
        StopCoroutine(enragedDash);
        pathfindingV2.found = false;
        pathfindingV2.route = new List<NodeInfo>();
        pathfindingV2.StopAllPathCheck();
        pathfindingV2.nowSearchingForGrid = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center);
        pathfindingV2.pathCheckRunning[0] = pathfindingV2.StartCoroutine(pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center), cameFromNode = null }, pathfindingV2.playercc.bounds.center, 0, false));
        StartCoroutine(Stun(stunDuration));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            if (enragedDash == null)
            {
                if (enragedCharging)
                {
                    animator.SetTrigger("CancelPreparation");
                    followRoute = null;
                    enragedCharging = false;
                }
            }
        }
    }
}
