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
        gridPosition.grid = tilemap.transform.parent.GetComponent<Grid>();
        gridPosition.tilemapCenter = tilemap.transform.position + tilemap.transform.position;
        gridPosition.tilemapCenter = gridPosition.NearGridPosition(gridPosition.tilemapCenter);
        gridPosition.vector2PositionSetUpType = EditorGUILayout.Toggle("Vector2 Position Setup", gridPosition.vector2PositionSetUpType);
        gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", gridPosition.gridTilemapPosition);
        if (firstTime)
        {
            Vector3 nearCell = gridPosition.NearGridPosition(gridPosition.transform.position - new Vector3(gridPosition.grid.cellSize.x * 0.5f, gridPosition.grid.cellSize.y * 0.5f));
            gridPosition.transform.position = nearCell + new Vector3(0.5f * gridPosition.grid.cellSize.x, 0.5f * gridPosition.grid.cellSize.y);
            Vector2 positionObject = new Vector2(nearCell.x - gridPosition.tilemapCenter.x, nearCell.y - gridPosition.tilemapCenter.y);
            positionObject = new Vector2(positionObject.x / gridPosition.grid.cellSize.x, positionObject.y / gridPosition.grid.cellSize.y);
            gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", new Vector2Int(Mathf.RoundToInt(positionObject.x), Mathf.RoundToInt(positionObject.y)));
            firstTime = false;
        }
        if (gridPosition.vector2PositionSetUpType)
        {
            Vector3 nearCell = gridPosition.NearGridPosition(gridPosition.tilemapCenter + new Vector3(gridPosition.gridTilemapPosition.x * gridPosition.grid.cellSize.x, gridPosition.gridTilemapPosition.y * gridPosition.grid.cellSize.y, 0));
            gridPosition.transform.position = nearCell + new Vector3(0.5f * gridPosition.grid.cellSize.x, 0.5f * gridPosition.grid.cellSize.y);
            Vector3 test = gridPosition.transform.position - gridPosition.tilemapCenter - new Vector3(0.5f * gridPosition.grid.cellSize.x, 0.5f * gridPosition.grid.cellSize.y);
            Debug.Log(new Vector2(test.x/gridPosition.gridTilemapPosition.x, test.y / gridPosition.gridTilemapPosition.y));
        }
        if (GUILayout.Button("Calibrate Position"))
        {
            Vector3 nearCell = gridPosition.NearGridPosition(gridPosition.transform.position - new Vector3(gridPosition.grid.cellSize.x * 0.5f, gridPosition.grid.cellSize.y * 0.5f));
            gridPosition.transform.position = nearCell + new Vector3(0.5f * gridPosition.grid.cellSize.x, 0.5f * gridPosition.grid.cellSize.y);
            Vector2 positionObject = new Vector2(nearCell.x - gridPosition.tilemapCenter.x, nearCell.y - gridPosition.tilemapCenter.y);
            positionObject = new Vector2(positionObject.x / gridPosition.grid.cellSize.x, positionObject.y / gridPosition.grid.cellSize.y);
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
