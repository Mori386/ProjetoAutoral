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
            switch (AiBoss.Instance.vida)
            {
                case 1:
                    AiBoss.Instance.pathfindingV2.route = new List<NodeInfo>();

                    AiBoss.Instance.transform.position = AiBoss.Instance.tp3.position;
                    AiBoss.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    AiBoss.Instance.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    AiBoss.Instance.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
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
        if (AiBoss.Instance.vida != 4)
        {
            AiBoss.Instance.pathfindingV2.nowSearchingForGrid = MathMethods.WorldToGrid(AiBoss.Instance.pathfindingV2.tilemapPointZero, AiBoss.Instance.pathfindingV2.tilemap.cellSize, AiBoss.Instance.pathfindingV2.cc.bounds.center);
            AiBoss.Instance.pathfindingV2.pathCheckRunning[0] = AiBoss.Instance.pathfindingV2.StartCoroutine(AiBoss.Instance.pathfindingV2.PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(AiBoss.Instance.pathfindingV2.tilemapPointZero, AiBoss.Instance.pathfindingV2.tilemap.cellSize, AiBoss.Instance.pathfindingV2.cc.bounds.center), cameFromNode = null }, AiBoss.Instance.pathfindingV2.playercc.bounds.center, 0, false));
            AiBoss.Instance.pathfindingV2.search = true;
            AiBoss.Instance.followRoute = null;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
