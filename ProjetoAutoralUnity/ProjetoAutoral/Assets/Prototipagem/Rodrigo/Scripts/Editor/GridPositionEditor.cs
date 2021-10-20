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
        Present,Future
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
            if(timePeriod==TimePeriod.Present)
            {
                tilemap = GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
            }
            else
            {
                tilemap = GameObject.Find("TilemapFuturo").GetComponent<Tilemap>();
            }
        }
        gridPosition.tilemapCenter = tilemap.origin + tilemap.transform.position;
        gridPosition.vector2PositionSetUpType = EditorGUILayout.Toggle("Vector2 Position Setup", gridPosition.vector2PositionSetUpType);
        if (firstTime)
        {
            gridPosition.transform.position = new Vector3(tilemap.WorldToCell(gridPosition.transform.position).x + 0.5f, tilemap.WorldToCell(gridPosition.transform.position).y + 0.5f, tilemap.WorldToCell(gridPosition.transform.position).z) + tilemap.transform.position;
            Vector2 positionObject = new Vector2(gridPosition.transform.position.x, gridPosition.transform.position.y) - new Vector2(gridPosition.tilemapCenter.x + 0.5f, gridPosition.tilemapCenter.y + 0.5f);
            gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", new Vector2Int(Mathf.RoundToInt(positionObject.x), Mathf.RoundToInt(positionObject.y)));
            firstTime = false;
        }
        gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", gridPosition.gridTilemapPosition);
        if (gridPosition.vector2PositionSetUpType)
        {
            gridPosition.transform.position = gridPosition.tilemapCenter + new Vector3(gridPosition.gridTilemapPosition.x + 0.5f, gridPosition.gridTilemapPosition.y + 0.5f, 0);
        }
        if (GUILayout.Button("Calibrate Position"))
        {
            gridPosition.transform.position = new Vector3(tilemap.WorldToCell(gridPosition.transform.position).x + 0.5f, tilemap.WorldToCell(gridPosition.transform.position).y + 0.5f, tilemap.WorldToCell(gridPosition.transform.position).z) + tilemap.transform.position;
            Vector2 positionObject = new Vector2(gridPosition.transform.position.x, gridPosition.transform.position.y) - new Vector2(gridPosition.tilemapCenter.x + 0.5f, gridPosition.tilemapCenter.y + 0.5f);
            gridPosition.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", new Vector2Int(Mathf.RoundToInt(positionObject.x), Mathf.RoundToInt(positionObject.y)));
        }
        if(objectBase!=null)
        {
            if (objectBase.timePeriod == ObjectBase.timePeriodList.Present && objectBase.objectOtherTimeline!=null)
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
