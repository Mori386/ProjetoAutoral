using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(NewGridPosition))]
public class NewGridPositionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NewGridPosition Main = (NewGridPosition)target;
        SerializedObject serializedSelf = new SerializedObject(this);
        SerializedObject serializedMain = new SerializedObject(Main);
        Main.tilemap = Main.tilemapFind();
        Main.tilemapPointZero = Main.findTilemapPointZero(Main.tilemap);
        Main.vector2PositionSetUp = EditorGUILayout.Toggle("Vector2 Position Setup", Main.vector2PositionSetUp);
        Main.gridTilemapPosition = EditorGUILayout.Vector2IntField("Position", Main.gridTilemapPosition);
        Main.spriteRenderer = Main.GetComponent<SpriteRenderer>();
        if (Main.vector2PositionSetUp)
        {
            Main.transform.position = Main.tilemapPointZero + Vector3.Scale(new Vector3(Main.gridTilemapPosition.x, Main.gridTilemapPosition.y), Main.tilemap.cellSize);
        }
        if (GUILayout.Button("Calibrate Position"))
        {
            Main.transform.position = MathMethods.NearGrid(Main.tilemapPointZero,Main.tilemap.cellSize, Main.transform.position);
            Main.gridTilemapPosition = Vector2Int.RoundToInt(MathMethods.WorldToGrid(Main.tilemapPointZero, Main.tilemap.cellSize, Main.transform.position));
        }
    }
}
