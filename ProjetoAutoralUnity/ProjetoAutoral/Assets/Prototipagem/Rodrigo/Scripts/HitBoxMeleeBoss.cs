using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxMeleeBoss : MonoBehaviour
{
    public static HitBoxMeleeBoss Instance { get; private set; }
    [System.NonSerialized] public bool playerOnTrigger;
    private void Awake()
    {
        Instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") & AiBoss.Instance.enragedDash == null)
        {
            if(AiBoss.Instance.followRoute != null) AiBoss.Instance.StopCoroutine(AiBoss.Instance.followRoute);
            meleeCheck = StartCoroutine(MeleeCheck());
            playerOnTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") & AiBoss.Instance.enragedDash == null)
        {
            if (meleeCheck != null) StopCoroutine(meleeCheck);
            StartCoroutine(WaitForLastAttack());
            meleeCheck = null;
            playerOnTrigger = false;
        }
    }
    private IEnumerator WaitForLastAttack()
    {
        while (true)
        {
            if (!AiBoss.Instance.attacking) break;
            yield return null;
        }
        AiBoss.Instance.pathfindingV2.route = new List<NodeInfo>();
        AiBoss.Instance.pathfindingV2.search = true;
        AiBoss.Instance.pathfindingV2.nowSearchingForGrid = new Vector3();
        AiBoss.Instance.followRoute = null;
    }
    Coroutine meleeCheck;
    private IEnumerator MeleeCheck()
    {
        while (true)
        {
            if (!AiBoss.Instance.attacking)
            {
                AiBoss.Instance.animator.SetTrigger("Melee");
                AiBoss.Instance.audioSource.PlayOneShot(AiBoss.Instance.bossHurt);
            }
            yield return null;
        }
    }
}
