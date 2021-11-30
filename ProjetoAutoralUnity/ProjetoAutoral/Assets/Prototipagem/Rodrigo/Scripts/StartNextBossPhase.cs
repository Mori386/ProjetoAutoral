using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartNextBossPhase : MonoBehaviour
{
    PlayableDirector director;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        director = GetComponent<PlayableDirector>();
        if (collision.CompareTag("Player"))
        {
            switch (AIBossV2.Instance.hp)
            {
                case 1:
                    AIBossV2.Instance.pathfindingV2.route = new List<NodeInfo>();

                    AIBossV2.Instance.transform.position = AIBossV2.Instance.tp3.position;
                    AIBossV2.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    AIBossV2.Instance.transform.Find("TriggerDetector").GetComponent<BoxCollider2D>().enabled = true;
                    AIBossV2.Instance.transform.Find("HitBoxDamage").GetComponent<BoxCollider2D>().enabled = true;
                    AIBossV2.Instance.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
                    director.Play();
                    break;
                case 2:
                case 3:
                case 4:
                    director.Play();
                    break;
            }
        }
    }
    public void TurnOnBoss()
    {
        if (AIBossV2.Instance.hp != 4)
        {
            AIBossV2.Instance.pathfindingV2.search = true;
            AIBossV2.Instance.pathfindingV2.nowSearchingForGrid = new Vector2();
            AIBossV2.Instance.followRoute = null;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
