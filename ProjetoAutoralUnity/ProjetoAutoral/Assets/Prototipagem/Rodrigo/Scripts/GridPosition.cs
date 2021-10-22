using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridPosition : MonoBehaviour
{
    public bool firstTime = true;
    public Tilemap tilemap;
    public Vector3 tilemapCenter;
    [System.NonSerialized] public bool vector2PositionSetUpType;
    [System.NonSerialized] public Vector2Int gridTilemapPosition;
    public Grid grid;
    public Vector3 NearGridPosition(Vector3 position)
    {
        Vector3 nearGridPosition = new Vector3(0, 0, 0);
        nearGridPosition.x = Mathf.RoundToInt(position.x / grid.cellSize.x) * grid.cellSize.x;
        nearGridPosition.y = Mathf.RoundToInt(position.y / grid.cellSize.y) * grid.cellSize.y;
        return nearGridPosition;
    }
    void Awake()
    {
        if (GetComponent<ObjectBase>() != null)
        {
            if (GetComponent<ObjectBase>().timePeriod == ObjectBase.timePeriodList.Present)
            {
                tilemap = GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
            }
            else
            {
                tilemap = GameObject.Find("TilemapFuturo").GetComponent<Tilemap>();
            }
            tilemapCenter = tilemap.transform.position + tilemap.transform.position;
            tilemapCenter = NearGridPosition(tilemapCenter);
            gridTilemapPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) - new Vector2Int(Mathf.RoundToInt(tilemapCenter.x), Mathf.RoundToInt(tilemapCenter.y));
        }
    }
    private void Start()
    {
        grid = tilemap.transform.parent.GetComponent<Grid>();
    }
}