using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLayer : MonoBehaviour
{
    public static Transform player { get; private set; }
    private SpriteRenderer spriteRenderer;
    private Vector2 deltaPivotToCenter;
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y) + deltaPivotToCenter, new Vector2(0.1f, 0.1f));
    }
    private void Awake()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        deltaPivotToCenter = new Vector2((spriteRenderer.bounds.size / 2).x, (spriteRenderer.bounds.size / 2).y) - spriteRenderer.sprite.pivot / spriteRenderer.sprite.pixelsPerUnit;
        StartCoroutine(CheckLayerChange());
    }
    IEnumerator CheckLayerChange()
    {
        while (true)
        {
            if (player.position.y > transform.position.y + deltaPivotToCenter.y)
            {
                spriteRenderer.sortingOrder = 3;
            }
            else
            {
                spriteRenderer.sortingOrder = 1;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
