using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallsNodes : MonoBehaviour
{
    public Tilemap tilemap;
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(tilemap.localBounds.center, tilemap.localBounds.size);
    }
    private void Awake()
    {

    }
}
