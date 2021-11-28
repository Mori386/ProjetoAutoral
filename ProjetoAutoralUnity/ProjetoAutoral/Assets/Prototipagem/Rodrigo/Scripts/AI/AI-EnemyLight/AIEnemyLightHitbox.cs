using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyLightHitbox : MonoBehaviour
{
    AIEnemyLight aiEnemyLight;
    private void Awake()
    {
        aiEnemyLight = transform.parent.GetComponent<AIEnemyLight>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            if (aiEnemyLight.followRoute != null) aiEnemyLight.StopCoroutine(aiEnemyLight.followRoute);
            if(aiEnemyLight.audioSource.isPlaying)aiEnemyLight.audioSource.Stop();
            aiEnemyLight.animator.enabled = false;
        }
        else if (collision.CompareTag("Player"))
        {
            if (aiEnemyLight.followRoute != null) aiEnemyLight.StopCoroutine(aiEnemyLight.followRoute);
            if (aiEnemyLight.audioSource.isPlaying) aiEnemyLight.audioSource.Stop();
            attackPlayer = StartCoroutine(AttackPlayer());
            aiEnemyLight.isPlayerTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            aiEnemyLight.followRoute = null;
            aiEnemyLight.animator.enabled = true;
        }
        else if (collision.CompareTag("Player"))
        {
            aiEnemyLight.isPlayerTriggered = false;
            StopCoroutine(attackPlayer);
            attackPlayer = null;
            StartCoroutine(WaitForLastAttack());
        }
    }
    private IEnumerator WaitForLastAttack()
    {
        while (true)
        {
            if (!aiEnemyLight.isAttacking) break;
            yield return null;
        }
        aiEnemyLight.followRoute = null;
    }
    Coroutine attackPlayer;
    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            if (!aiEnemyLight.isAttacking)
            {
                Vector3 deltaPos = Vector3.Normalize(PMStateManager.Instance.GetComponent<CapsuleCollider2D>().bounds.center - aiEnemyLight.transform.position);
                if (Mathf.Abs(deltaPos.x) >= Mathf.Abs(deltaPos.y))
                {
                    aiEnemyLight.animator.SetBool("Front", false);
                    aiEnemyLight.animator.SetBool("Back", false);
                    aiEnemyLight.animator.SetBool("Side", true);
                    if ((deltaPos.x / Mathf.Abs(deltaPos.x)) > 0)
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
                    aiEnemyLight.animator.SetBool("Side", false);
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    if ((deltaPos.y / Mathf.Abs(deltaPos.y)) > 0)
                    {
                        aiEnemyLight.animator.SetBool("Front", false);
                        aiEnemyLight.animator.SetBool("Back", true);
                    }
                    else
                    {
                        aiEnemyLight.animator.SetBool("Back", false);
                        aiEnemyLight.animator.SetBool("Front", true);
                    }
                }
                aiEnemyLight.animator.SetTrigger("Attack");
            }
            yield return null;
        }
    }
}
