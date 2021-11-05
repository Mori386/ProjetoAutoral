using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallNodesV2
{
    public bool finished;
    public List<NodeInfoV2> wallList;
    public WallNodesV2(BoundsInt boundsTilemap, Tilemap tilemap)
    {
        finished = false;
        wallList = new List<NodeInfoV2>();
        BoundsInt boundsInt = new BoundsInt();
        boundsInt.size = MathMethods.roundToMore(tilemap.size);
        boundsInt.position = MathMethods.roundToMore(tilemap.transform.position- Vector3.Scale(tilemap.size,tilemap.cellSize)/1.5f);
        gizmosBounds = boundsInt;
        Debug.Log(tilemap.GetTilesBlock(boundsInt).Length);
        //procura parede
    }
    public BoundsInt gizmosBounds;
}
