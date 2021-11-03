using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTest : MonoBehaviour
{
    Pathfinding pathfinding;
    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider2D;
    Vector3 positionToCenter;
    [SerializeField] private float moveSpeed;
    [System.NonSerialized] public bool stunned;
    [System.NonSerialized] public bool enraged;
    public bool collideWithWall;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfinding = GetComponent<Pathfinding>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        positionToCenter = capsuleCollider2D.bounds.center - transform.position;
    }
    Coroutine move;
    Coroutine checkOnSamePosition;
    Coroutine enragedTimer;
    Coroutine enragedDash;

    void Update()
    {
        if (pathfinding.finished && !stunned && !enraged)
        {
            if (move != null) StopCoroutine(move);
            if (checkOnSamePosition != null) StopCoroutine(checkOnSamePosition);
            move = StartCoroutine(Move(pathfinding.route));
            pathfinding.finished = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.CompareTag("DealDamage"))
        {

        }
        */
        if (enragedTimer == null && enragedDash == null && collision.name== "Flashlight")
        {
            if (!stunned)
            {
                enraged = true;
                enragedTimer = StartCoroutine(EnragedTimer());
            }
        }
        if (enragedDash != null)
        {
            /*
            if (collision.CompareTag("Rachaduras"))
            {
                //dropa pedra e os krl 
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
            {
                ObjectDestroyable objectDestroyable = collision.GetComponent<ObjectDestroyable>();
                if (objectDestroyable != null)
                {
                    collision.GetComponent<SpriteRenderer>().sprite = objectDestroyable.destroyedSprite;
                    if (objectDestroyable.disableAllTriggerColliders)
                    {
                        foreach (BoxCollider2D boxCollider2D in collision.GetComponents<BoxCollider2D>())
                        {
                            boxCollider2D.enabled = false;
                        }
                    }
                    else
                    {
                        foreach (BoxCollider2D boxCollider2D in collision.GetComponents<BoxCollider2D>())
                        {
                            if (!boxCollider2D.isTrigger) boxCollider2D.enabled = false;
                        }
                    }
                }
            }
            if (collision.CompareTag("Player"))
            {
                //da dano ao player
            }
            */
            if (collision.CompareTag("Wall"))
            {
                Debug.Log("a");
                StopCoroutine(enragedDash);
                enragedDash = null;
                enraged = false;
                stunned = true;
                StartCoroutine(StunDuration(2));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enragedDash == null && enragedTimer != null && collision.name== "Flashlight")
        {
            enraged = false;
            StopCoroutine(enragedTimer);
            enragedTimer = null;
            Debug.Log("Solto");
            pathfinding.restartPathfinding();
        }
    }
    bool onTheSamePosition;

    public float enragedChargeTime;

    private IEnumerator StunDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        pathfinding.restartPathfinding();
        stunned = false;
    }
    private IEnumerator EnragedTimer()
    {
        Debug.Log("Carregando");
        if (move != null) StopCoroutine(move);
        if (checkOnSamePosition != null) StopCoroutine(checkOnSamePosition);
        yield return new WaitForSeconds(enragedChargeTime);
        enragedDash = StartCoroutine(EnragedDash());
        enragedTimer = null;
    }
    private IEnumerator EnragedDash()
    {
        Debug.Log("Descarga");
        //trigger enabled true de dano e de quebrar objetos
        Vector3 directionCharge = pathfinding.finalPoint.position - transform.position;
        directionCharge = Vector3.Normalize(directionCharge);
        if (!collideWithWall) Destroy(gameObject, 5f);
        while (true)
        {
            transform.position += directionCharge * moveSpeed * 9 * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    public NodeInfo goingToNode;
    private IEnumerator Move(List<NodeInfo> route)
    {
        int startNodeIndex = route.Count - 1;
        for (int i = route.Count - 2; i >= 0; i--)
        {
            if (Vector2.Distance(transform.position, pathfinding.gridToWorld(route[i].gridPosition) - positionToCenter) < Vector2.Distance(transform.position, pathfinding.gridToWorld(route[startNodeIndex].gridPosition) - positionToCenter))
            {
                startNodeIndex = i;
            }
        }
        for (int i = startNodeIndex; i >= 0; i--)
        {
            goingToNode = route[i];
            while (true)
            {
                //if (pathfinding.foundNewPos)
                //{
                //    yield break;
                //}
                Vector3 deltaPos = (pathfinding.gridToWorld(route[i].gridPosition) - positionToCenter) - transform.position;
                checkOnSamePosition = StartCoroutine(CheckOnSamePosition(i, route));
                while (!onTheSamePosition)
                {
                    transform.position += deltaPos / (20 / moveSpeed);
                    yield return new WaitForFixedUpdate();
                }
                break;
            }
            onTheSamePosition = false;
        }
    }

    private IEnumerator CheckOnSamePosition(int i, List<NodeInfo> route)
    {
        onTheSamePosition = false;
        while (true)
        {
            Vector2 roundedGridPos = (pathfinding.gridToWorld(route[i].gridPosition) - positionToCenter);
            roundedGridPos.x = Mathf.Round(roundedGridPos.x * 10) / 10;
            roundedGridPos.y = Mathf.Round(roundedGridPos.y * 10) / 10;
            Vector2 roundedPosition = transform.position;
            roundedPosition.x = Mathf.Round(roundedPosition.x * 10) / 10;
            roundedPosition.y = Mathf.Round(roundedPosition.y * 10) / 10;
            if (roundedGridPos == roundedPosition)
            {
                onTheSamePosition = true;
                yield break;
            }
            yield return null;
        }
    }
}
