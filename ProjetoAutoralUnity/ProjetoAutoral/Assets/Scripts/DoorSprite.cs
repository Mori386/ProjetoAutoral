using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSprite : MonoBehaviour
{
    public static DoorSprite Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Sprite
        doorSidePresentOpen, doorSidePresentClosed, doorSideFutureOpen, doorSideFutureClosed,
        doorFrontPresentOpen, doorFrontPresentClosed, doorFrontFutureOpen, doorFrontFutureClosed;
}