using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestFutureObject : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    Vector3 tilemapCenter;
    void Start()
    {
        tilemapCenter = tilemap.origin + tilemap.transform.position;
        transform.position = tilemapCenter;
    }

}
