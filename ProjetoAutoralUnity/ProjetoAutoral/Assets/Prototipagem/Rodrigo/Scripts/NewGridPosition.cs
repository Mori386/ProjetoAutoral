using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewGridPosition : MonoBehaviour
{
    public Vector3 tilemapPointZero;
    public Tilemap tilemap;
    public ObjectBase.timePeriodList timePeriod;

    public bool vector2PositionSetUp;
    public Vector2Int gridTilemapPosition;

    public SpriteRenderer spriteRenderer;
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y)
            , new Vector2(0.1f, 0.1f));
    }
    public Tilemap tilemapFind()
    {
        if (GetComponent<ObjectBase>().timePeriod == ObjectBase.timePeriodList.Present)
        {
            timePeriod = ObjectBase.timePeriodList.Present;
            return GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
        }
        else
        {
            timePeriod = ObjectBase.timePeriodList.Future;
            return GameObject.Find("TilemapFuturo").GetComponent<Tilemap>();
        }
    }
    public Vector3 findTilemapPointZero(Tilemap tilemap)
    {
        return tilemap.GetCellCenterWorld(tilemap.WorldToCell(GameObject.Find(tilemap.name + "PointZero").transform.position));
    }
    private void Awake()
    {
        tilemap = tilemapFind();
        tilemapPointZero = findTilemapPointZero(tilemap);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
