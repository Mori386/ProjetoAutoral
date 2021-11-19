using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathMethods
{
    public static Vector3Int roundToMore(Vector3 vector3)
    {
        Vector3Int roundedVector = new Vector3Int();
        if (vector3.x > 0)
        {
            roundedVector.x = Mathf.CeilToInt(vector3.x);
        }
        else if (vector3.x < 0)
        {
            roundedVector.x = Mathf.FloorToInt(vector3.x);
        }
        if (vector3.y > 0)
        {
            roundedVector.y = Mathf.CeilToInt(vector3.y);
        }
        else if (vector3.y < 0)
        {
            roundedVector.y = Mathf.FloorToInt(vector3.y);
        }
        if (vector3.z > 0)
        {
            roundedVector.z = Mathf.CeilToInt(vector3.z);
        }
        else if (vector3.z < 0)
        {
            roundedVector.z = Mathf.FloorToInt(vector3.z);
        }
        return roundedVector;
    }
    public static Vector2 WorldToGrid(Vector3 pointZero, Vector3 cellSize,Vector3 position)
    {
        return Vector2Int.RoundToInt(Vector3.Scale(position - pointZero, new Vector3(1/ cellSize.x, 1 / cellSize.y)));
    }
    public static Vector3 GridToWorld(Vector3 pointZero, Vector3 gridPosition, Vector3 cellSize)
    {
        return Vector3.Scale(gridPosition, cellSize) + pointZero;
    }
    public static Vector3 NearGrid(Vector3 pointZero, Vector3 cellSize, Vector3 position)
    {
        return GridToWorld(pointZero, WorldToGrid(pointZero, cellSize, position), cellSize);
    }
}
