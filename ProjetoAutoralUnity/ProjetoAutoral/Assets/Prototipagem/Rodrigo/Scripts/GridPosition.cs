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
    public static Vector3 NearGridPosition(Vector3 position)
    {
        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();
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
            tilemapCenter = tilemap.origin + tilemap.transform.position;
            tilemapCenter = NearGridPosition(tilemapCenter);
            Vector3 nearCell = NearGridPosition(transform.position - new Vector3(tilemap.cellSize.x * 0.5f, tilemap.cellSize.y * 0.5f));
            transform.position = nearCell + new Vector3(0.5f * tilemap.cellSize.x, 0.5f * tilemap.cellSize.y);
            Vector2 positionObject = new Vector2(nearCell.x - tilemapCenter.x, nearCell.y - tilemapCenter.y);
            positionObject = new Vector2(positionObject.x / tilemap.cellSize.x, positionObject.y / tilemap.cellSize.y);
            gridTilemapPosition = new Vector2Int(Mathf.RoundToInt(positionObject.x), Mathf.RoundToInt(positionObject.y));
        }
    }
    private void Start()
    {
        grid = tilemap.transform.parent.GetComponent<Grid>();
    }
}