using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBossV2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            AIBossV2.Instance.flashlightTrigger = true;
            StartCoroutine(FlashlightTrigger());
        }
        else if (collision.CompareTag("Player"))
        {
            AIBossV2.Instance.playerOnTrigger = true;
            StartCoroutine(PlayerTrigger());
        }
    }
    private IEnumerator FlashlightTrigger()
    {
        while (AIBossV2.Instance.flashlightTrigger)
        {
            if (AIBossV2.Instance.BossState == AIBossV2.BossStateList.Routing)
            {
                AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.followRoute);
                AIBossV2.Instance.BossState = AIBossV2.BossStateList.Charging;
                AIBossV2.Instance.animator.ResetTrigger("CancelPreparation");
                AIBossV2.Instance.audioSourcePreAttack.clip = AIBossV2.Instance.preAttack;
                AIBossV2.Instance.audioSourcePreAttack.Play();
                AIBossV2.Instance.animator.SetTrigger("Preparation");
            }
            yield return null;
        }
        if (AIBossV2.Instance.BossState == AIBossV2.BossStateList.Charging)
        {
            AIBossV2.Instance.animator.SetTrigger("CancelPreparation");
            AIBossV2.Instance.audioSourcePreAttack.Stop();
            AIBossV2.Instance.followRoute = AIBossV2.Instance.StartCoroutine(AIBossV2.Instance.FollowRoute());
        }
    }
    private IEnumerator PlayerTrigger()
    {
        while (AIBossV2.Instance.playerOnTrigger)
        {
            if (AIBossV2.Instance.BossState == AIBossV2.BossStateList.Routing)
            {
                AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.followRoute);
                AIBossV2.Instance.animator.SetTrigger("Melee");
                AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.bossMelee);
                break;
            }
            else if (AIBossV2.Instance.BossState == AIBossV2.BossStateList.Dashing)
            {
                PMStateManager.Instance.TakeDamage(1);
            }
            yield return null;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            AIBossV2.Instance.flashlightTrigger = false;
        }
        else if (collision.CompareTag("Player"))
        {
            AIBossV2.Instance.playerOnTrigger = false;
        }
    }
}
