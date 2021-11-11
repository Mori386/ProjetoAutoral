using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyable : MonoBehaviour
{
    public Sprite destroyedSprite;
    public bool disableAllTriggerColliders;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeSprite()
    {
        spriteRenderer.sprite = destroyedSprite;
    }
}
