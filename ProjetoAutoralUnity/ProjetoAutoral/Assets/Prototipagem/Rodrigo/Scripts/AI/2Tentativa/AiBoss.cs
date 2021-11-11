using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBoss : MonoBehaviour
{
    PathfindingV2 pathfindingV2;
    Vector3 positionToCenter;
    [SerializeField] public GameObject target;

    [System.NonSerialized] public int vida = 4;
    private void Start()
    {
        pathfindingV2 = GetComponent<PathfindingV2>();
        positionToCenter = positionToCenter = GetComponent<CapsuleCollider2D>().bounds.center - transform.position;
        target = pathfindingV2.player.gameObject;
    }
    private void Update()
    {
        if (vida == 1)
        {
            if (enragedDash == null)
            {
                if (enragedChargeTime == null)
                {
                    enragedChargeTime = StartCoroutine(EnragedChargeTime(pathfindingV2.chargeTime));
                }
            }
        }
        else if (enragedDash == null)
        {
            if (enragedChargeTime != null)
            {
                if (Input.GetKeyUp(KeyCode.Tab))
                {
                    StopCoroutine(enragedChargeTime);
                    enragedChargeTime = null;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    StopCoroutine(followRoute);
                    followRoute = null;
                    enragedChargeTime = StartCoroutine(EnragedChargeTime(pathfindingV2.chargeTime));
                }
                else if (pathfindingV2.route.Count > 0 && followRoute == null) followRoute = StartCoroutine(FollowRoute());
            }
        }
    }
    public Coroutine followRoute;
    public IEnumerator FollowRoute()
    {
        while (true)
        {
            Vector3 deltaPos = (MathMethods.GridToWorld(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.route[0].gridPosition) - positionToCenter) - transform.position;
            deltaPos = Vector3.Normalize(deltaPos);
            while (Vector2.Distance(transform.position, MathMethods.GridToWorld(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.route[0].gridPosition) - positionToCenter) > 0.1f)
            {
                transform.position += deltaPos * pathfindingV2.moveSpeed * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            pathfindingV2.route.Remove(pathfindingV2.route[0]);
            yield return new WaitForFixedUpdate();
        }
    }
    public Coroutine enragedChargeTime;
    public IEnumerator EnragedChargeTime(float time)
    {
        yield return new WaitForSeconds(time);
        enragedDash = StartCoroutine(EnragedDash());
        enragedChargeTime = null;
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
            if (collision.name == "rachadura")
            {
                Instantiate(pathfindingV2.pedraPf, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                StopAndStun(2);
            }
            else if (collision.name == "ObjetoFlamejante")
            {
                vida--;
                Debug.Log(vida);
                StopAndStun(2); // animação diferente pra ele pega fogo
            }
            else if (collision.name == "GavetaRachadura")
            {
                collision.transform.Find("ParticleEmissor").GetComponent<ParticleSystem>().Play();
                OD.ChangeSprite();
                collision.enabled = false;
                StopAndStun(2);
            }
            else if (OD != null)
            {
                OD.ChangeSprite();
                collision.enabled = false;
            }
            else if (collision.CompareTag("Wall"))
            {
                StopAndStun(2);
            }
        }
    }
    private void StopAndStun(float stunDuration)
    {
        StopCoroutine(enragedDash);
        pathfindingV2.found = false;
        pathfindingV2.route = new List<NodeInfo>();
        pathfindingV2.StopAllPathCheck();
        pathfindingV2.pathCheckRunning[0] = pathfindingV2.StartCoroutine(pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.cc.bounds.center), cameFromNode = null }, pathfindingV2.player.position, 0, false));
        StartCoroutine(Stun(stunDuration));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
