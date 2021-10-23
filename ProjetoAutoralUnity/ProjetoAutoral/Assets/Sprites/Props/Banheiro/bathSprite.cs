using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bathSprite : MonoBehaviour
{
    public static bathSprite Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Sprite
        fullBathPresent, fullBathFuture, emptyBathPresent, emptyBathFuture;
}
