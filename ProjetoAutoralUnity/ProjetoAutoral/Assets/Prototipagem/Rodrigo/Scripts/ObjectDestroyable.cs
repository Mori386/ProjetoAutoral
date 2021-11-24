using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyable : MonoBehaviour
{
    public Sprite destroyedSprite;
    public bool disableAllTriggerColliders;
    [System.NonSerialized] public SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeSprite()
    {
        spriteRenderer.sprite = destroyedSprite;
        Destroy(this);
    }
    private void OnDestroy()
    {
        WhenObjectDestroyedItemGoDown whenObjectDestroyedItemGo = GetComponent<WhenObjectDestroyedItemGoDown>();
        if (whenObjectDestroyedItemGo != null)
        {
            whenObjectDestroyedItemGo.consequence();
        }
    }
}
