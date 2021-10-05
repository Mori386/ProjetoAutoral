using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridPosition : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3 tilemapCenter;
    [System.NonSerialized] public bool vector2PositionSetUpType;
    [System.NonSerialized] public Vector2Int gridTilemapPosition;
    void Start()
    {
        if(GetComponent<ObjectBase>().timePeriod == ObjectBase.timePeriodList.Present)
        {
            tilemap = GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
        }
        else
        {
            tilemap = GameObject.Find("TilemapFuturo").GetComponent<Tilemap>();
        }
        tilemapCenter = tilemap.origin + tilemap.transform.position;
        gridTilemapPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) - new Vector2Int(Mathf.RoundToInt(tilemapCenter.x), Mathf.RoundToInt(tilemapCenter.y));
    }
}