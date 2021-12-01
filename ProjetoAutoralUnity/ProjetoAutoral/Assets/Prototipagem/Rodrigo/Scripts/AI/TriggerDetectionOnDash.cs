using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionOnDash : MonoBehaviour
{
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.enragedDash);
            AIBossV2.Instance.animator.SetTrigger("HitWall");
            AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.wall, AIBossV2.Instance.wallVolume);
            if(AIBossV2.Instance.audioSourceDashExtended.isPlaying) AIBossV2.Instance.audioSourceDashExtended.Stop();
            AIBossV2.Instance.BossState = AIBossV2.BossStateList.Stun;
            if (AIBossV2.Instance.hp != 1)
            {
                AIBossV2.Instance.pathfindingV2.search = true;
                AIBossV2.Instance.pathfindingV2.nowSearchingForGrid = new Vector2();
                yield return new WaitForSeconds(0.25f);
                if (!AIBossV2.Instance.playerOnTrigger & !AIBossV2.Instance.flashlightTrigger)
                {
                    AIBossV2.Instance.followRoute = AIBossV2.Instance.StartCoroutine(AIBossV2.Instance.FollowRoute());
                }
                else
                {
                    AIBossV2.Instance.BossState = AIBossV2.BossStateList.Routing;
                }
            }
            else
            {
                AIBossV2.Instance.BossState = AIBossV2.BossStateList.WaitingForNextActivation;
            }
            Destroy(this);
        }
        else if (collision.CompareTag("Rachadura"))
        {
            AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.enragedDash);
            AIBossV2.Instance.animator.SetTrigger("HitWall");
            AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.wall, AIBossV2.Instance.wallVolume);
            if (AIBossV2.Instance.audioSourceDashExtended.isPlaying) AIBossV2.Instance.audioSourceDashExtended.Stop();
            AIBossV2.Instance.BossState = AIBossV2.BossStateList.Stun;
            Instantiate(AIBossV2.Instance.pathfindingV2.pedraPf, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(this);
        }
        else
        {
            ObjectDestroyable OD = collision.GetComponent<ObjectDestroyable>();
            switch (collision.name)
            {
                case "ObjetoFlamejante":
                    if (collision.transform.Find("FireLight2D").gameObject.activeInHierarchy)
                    {
                        AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.enragedDash);
                        AIBossV2.Instance.animator.SetTrigger("HitWall");
                        AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.wall, AIBossV2.Instance.wallVolume);
                        if (AIBossV2.Instance.audioSourceDashExtended.isPlaying) AIBossV2.Instance.audioSourceDashExtended.Stop();
                        AIBossV2.Instance.TakeDamage();
                    }
                    else
                    {
                        AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.enragedDash);
                        AIBossV2.Instance.animator.SetTrigger("HitWall");
                        AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.wall, AIBossV2.Instance.wallVolume);
                        if (AIBossV2.Instance.audioSourceDashExtended.isPlaying) AIBossV2.Instance.audioSourceDashExtended.Stop();
                        AIBossV2.Instance.BossState = AIBossV2.BossStateList.Stun;
                        AIBossV2.Instance.pathfindingV2.search = true;
                        AIBossV2.Instance.pathfindingV2.nowSearchingForGrid = new Vector2();
                        yield return new WaitForSeconds(0.25f);
                        if (!AIBossV2.Instance.playerOnTrigger & !AIBossV2.Instance.flashlightTrigger)
                        {
                            AIBossV2.Instance.followRoute = AIBossV2.Instance.StartCoroutine(AIBossV2.Instance.FollowRoute());
                        }
                        else
                        {
                            AIBossV2.Instance.BossState = AIBossV2.BossStateList.Routing;
                        }
                    }
                    Destroy(this);
                    break;
                case "GavetaRachadura":
                    AIBossV2.Instance.StopCoroutine(AIBossV2.Instance.enragedDash);
                    AIBossV2.Instance.animator.SetTrigger("HitWall");
                    AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.wall, AIBossV2.Instance.wallVolume);
                    if (AIBossV2.Instance.audioSourceDashExtended.isPlaying) AIBossV2.Instance.audioSourceDashExtended.Stop();
                    collision.transform.Find("ParticleEmissor").GetComponent<ParticleSystem>().Play();
                    OD.ChangeSprite();
                    collision.enabled = false;
                    AIBossV2.Instance.BossState = AIBossV2.BossStateList.Stun;
                    AIBossV2.Instance.pathfindingV2.search = true;
                    AIBossV2.Instance.pathfindingV2.nowSearchingForGrid = new Vector2();
                    yield return new WaitForSeconds(0.25f);
                    if (!AIBossV2.Instance.playerOnTrigger & !AIBossV2.Instance.flashlightTrigger)
                    {
                        AIBossV2.Instance.followRoute = AIBossV2.Instance.StartCoroutine(AIBossV2.Instance.FollowRoute());
                    }
                    else
                    {
                        AIBossV2.Instance.BossState = AIBossV2.BossStateList.Routing;
                    }
                    Destroy(this);
                    break;
                case "rachadura4":
                    collision.enabled = false;
                    StartCoroutine(delayDeath());
                    Destroy(this);
                    break;
                default:
                    if (OD != null)
                    {
                        OD.ChangeSprite();
                        collision.enabled = false;
                    }
                    break;
            }
        }
    }
    private IEnumerator delayDeath()
    {
        yield return new WaitForSeconds(1f);
        AIBossV2.Instance.audioSourceDashExtended.Stop();
        foreach (AudioSource audio in AIBossV2.Instance.GetComponents<AudioSource>())
        {
            if (audio.clip.name == "ambientBoss")
            {
                audio.Stop();
                break;
            }
        }
        AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.bossDeath, AIBossV2.Instance.bossDeathVolume);
        yield return new WaitForSeconds(AIBossV2.Instance.bossDeath.length);
        Destroy(gameObject);
    }
}
