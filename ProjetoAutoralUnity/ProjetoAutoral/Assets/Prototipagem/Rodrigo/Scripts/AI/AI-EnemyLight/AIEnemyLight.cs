using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyLight : MonoBehaviour
{
    PathfindingV2 pathfindingV2;
    public static AIEnemyLight Instance { get; private set; }
    Vector3 positionToCenter;
    [System.NonSerialized] public Animator animator;
    [System.NonSerialized] public bool isAttacking;
    [System.NonSerialized] public bool isPlayerTriggered;

    public AudioSource audioSource;
    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        pathfindingV2 = GetComponent<PathfindingV2>();
        positionToCenter = positionToCenter = GetComponent<CapsuleCollider2D>().bounds.center - transform.position;
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (pathfindingV2.route.Count > 0 && followRoute == null)
        {
            followRoute = StartCoroutine(FollowRoute());
        }
    }
    public Coroutine followRoute;
    public IEnumerator FollowRoute()
    {
        audioSource.Play();
        while (pathfindingV2.route.Count > 0)
        {
            Vector3 deltaPos = (MathMethods.GridToWorld(pathfindingV2.tilemapPointZero, pathfindingV2.tilemap.cellSize, pathfindingV2.route[0].gridPosition) - positionToCenter) - transform.position;
            deltaPos = Vector3.Normalize(deltaPos);
            if(Mathf.Abs(deltaPos.x) >= Mathf.Abs(deltaPos.y))
            {
                animator.SetBool("Front", false);
                animator.SetBool("Back", false);
                animator.SetBool("Side", true);
                if ((deltaPos.x / Mathf.Abs(deltaPos.x))>0)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, -180, 0, 0);
                }
            }
            else
            {
                animator.SetBool("Side", false);
                transform.rotation = new Quaternion(0, 0, 0, 0);
                if ((deltaPos.y / Mathf.Abs(deltaPos.y)) > 0)
                {
                    animator.SetBool("Front", false);
                    animator.SetBool("Back", true);
                }
                else
                {
                    animator.SetBool("Back", false);
                    animator.SetBool("Front", true);
                }
            }
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
        audioSource.Stop();
        followRoute = null;
    }
    public void DealDamagePlayerBasedOnTrigger()
    {
        if (isPlayerTriggered)
        {
            PMStateManager.Instance.TakeDamage(1);
        }
    }
    public void ChangeIsAttacking()
    {
        isAttacking = !isAttacking;
    }
}
