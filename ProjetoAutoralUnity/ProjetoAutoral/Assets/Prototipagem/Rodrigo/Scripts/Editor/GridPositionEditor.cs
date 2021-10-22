using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections;

[CustomEditor(typeof(GridPosition))]
public class GridPositionEditor : Editor
{
    bool firstTime = true;
    public enum TimePeriod
    {
        Present, Future
    }
    public TimePeriod timePeriod;
    private Grid grid;
    private Vector3 NearGridPosition(Vector3 position)
    {
        Vector3 nearGridPosition = new Vector3(0, 0, 0);
        nearGridPosition.x = Mathf.RoundToInt(position.x / grid.cellSize.x) * grid.cellSize.x;
        nearGridPosition.y = Mathf.RoundToInt(position.y / grid.cellSize.y) * grid.cellSize.y;
        return nearGridPosition;
    }
    public override void OnInspectorGUI()
    {
        GridPosition gridPosition = (GridPosition)target;
        ObjectBase objectBase = gridPosition.GetComponent<ObjectBase>();
        Tilemap tilemap;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        if (gridPosition.gameObject.GetComponent<ObjectBase>() != null)
        {
            if (gridPosition.gameObject.GetComponent<ObjectBase>().timePeriod == ObjectBase.timePeriodList.Present)
            {
                tilemap = GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
            }
            else
            {
                tilemap = GameObject.Find("TilemapFuturo").GetComponent<Tilemap>();
            }
        }
        else
        {
            SerializedProperty serializedPropertyTimeline = serializedObject.FindProperty("timePeriod");
            EditorGUILayout.PropertyField(serializedPropertyTimeline);
            if (timePeriod == TimePeriod.Present)
            {
                tilemap = GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
            }
            else
            {
                tilemap = GameObject.Find("TilemapFuturo").GetComponent<Tilemap>();
            }
        }
        grid = tilemap.transform.parent.GetComponent<Grid>();
        gridPosition.tilemapCenter = tilemap.origin + tilemap.transform.position;
        gridPosition.tilemapCenter = NearGridPosition(gridPosition.tilemapCenter);
        gridPosition.vector2PositionSetUpType = EditorGUILayout.Toggle("Vector2 Position Setup", gridPosition.vector2PositionSetUpType);
        gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", gridPosition.gridTilemapPosition);
        if (firstTime)
        {
            Vector3 nearCell = NearGridPosition(gridPosition.transform.position - new Vector3(grid.cellSize.x * 0.5f, grid.cellSize.y * 0.5f));
            gridPosition.transform.position = nearCell + new Vector3(0.5f * grid.cellSize.x, 0.5f * grid.cellSize.y);
            Vector2 positionObject = new Vector2(nearCell.x - gridPosition.tilemapCenter.x, nearCell.y - gridPosition.tilemapCenter.y);
            positionObject = new Vector2(positionObject.x / grid.cellSize.x, positionObject.y / grid.cellSize.y);
            gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", new Vector2Int(Mathf.RoundToInt(positionObject.x), Mathf.RoundToInt(positionObject.y)));
            firstTime = false;
        }
        if (gridPosition.vector2PositionSetUpType)
        {
            Vector3 nearCell = NearGridPosition(gridPosition.tilemapCenter + new Vector3(gridPosition.gridTilemapPosition.x * grid.cellSize.x, gridPosition.gridTilemapPosition.y * grid.cellSize.y, 0));
            gridPosition.transform.position = nearCell + new Vector3(0.5f * grid.cellSize.x, 0.5f * grid.cellSize.y);
            Vector3 test = gridPosition.transform.position - gridPosition.tilemapCenter - new Vector3(0.5f * grid.cellSize.x, 0.5f * grid.cellSize.y);
            Debug.Log(new Vector2(test.x/gridPosition.gridTilemapPosition.x, test.y / gridPosition.gridTilemapPosition.y));
        }
        if (GUILayout.Button("Calibrate Position"))
        {
            Vector3 nearCell = NearGridPosition(gridPosition.transform.position - new Vector3(grid.cellSize.x * 0.5f, grid.cellSize.y * 0.5f));
            gridPosition.transform.position = nearCell + new Vector3(0.5f * grid.cellSize.x, 0.5f * grid.cellSize.y);
            Vector2 positionObject = new Vector2(nearCell.x - gridPosition.tilemapCenter.x, nearCell.y - gridPosition.tilemapCenter.y);
            positionObject = new Vector2(positionObject.x / grid.cellSize.x, positionObject.y / grid.cellSize.y);
            gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", new Vector2Int(Mathf.RoundToInt(positionObject.x), Mathf.RoundToInt(positionObject.y)));
        }
        if (objectBase != null)
        {
            if (objectBase.timePeriod == ObjectBase.timePeriodList.Present && objectBase.objectOtherTimeline != null)
            {
                if (GUILayout.Button("Move On Other Timeline"))
                {
                    GameObject objectFuture = objectBase.objectOtherTimeline;
                    objectFuture.GetComponent<GridPosition>().gridTilemapPosition = gridPosition.gridTilemapPosition;
                    GridPosition gridFromObjectFuture = objectFuture.GetComponent<GridPosition>();
                    objectFuture.transform.position = gridFromObjectFuture.tilemapCenter + new Vector3(gridFromObjectFuture.gridTilemapPosition.x + 0.5f, gridFromObjectFuture.gridTilemapPosition.y + 0.5f, 0);
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
