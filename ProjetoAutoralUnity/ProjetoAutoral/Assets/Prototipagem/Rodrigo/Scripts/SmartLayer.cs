using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLayer : MonoBehaviour
{
    public static Transform player { get; private set; }
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    IEnumerator CheckLayerChange()
    {
        if (player.position.y > transform.position.y)
        {
            sprite
        }
    }
}
