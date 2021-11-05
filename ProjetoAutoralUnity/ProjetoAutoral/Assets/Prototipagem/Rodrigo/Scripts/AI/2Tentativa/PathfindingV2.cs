using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingV2 : MonoBehaviour
{
    //Setup
    bool showGizmosOnPlay;

    //Componentes
    CapsuleCollider2D cc;

    //tilemap
    [SerializeField] Transform finalPoint;
    [SerializeField] Tilemap tilemap;
    Vector3 tilemapPointZero;

    //Nodulos Salvos
    List<NodeInfo> wallNodes;
    List<NodeInfo> nodeQueue;
    List<NodeInfo> knownNodes;
    List<NodeInfo> route;

    //Variaveis 
    private enum Directions
    {
        top, left, right, bottom
    }
    Directions[] directionSearchOrder = new Directions[4];
    private int maxCoroutines;
    private int coroutinesRunning;
    private void OnDrawGizmos()
    {
        if (showGizmosOnPlay)
        {
            Gizmos.DrawCube(tilemapPointZero, new Vector3(0.1f, 0.1f));
            //Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero,new Vector3(1,1),tilemap.cellSize), new Vector3(0.1f, 0.1f));
            //Gizmos.DrawCube(MathMethods.WorldToGrid(tilemapPointZero,tilemap.cellSize,MathMethods.GridToWorld(tilemapPointZero, new Vector3(1, 1), tilemap.cellSize)), new Vector3(0.1f, 0.1f));
            //Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero,MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, MathMethods.GridToWorld(tilemapPointZero, new Vector3(1, 1), tilemap.cellSize)),tilemap.cellSize), new Vector3(0.1f, 0.1f));
        }
    }
    private void Awake()
    {
        wallNodes = new List<NodeInfo>();
        nodeQueue = new List<NodeInfo>();
        knownNodes = new List<NodeInfo>();
        route = new List<NodeInfo>();
        foreach (CapsuleCollider2D capsuleCollider2D in GetComponents<CapsuleCollider2D>())
        {
            if (!capsuleCollider2D.isTrigger) cc = GetComponent<CapsuleCollider2D>();
        }
    }
    private void Start()
    {
        showGizmosOnPlay = true;
        tilemapPointZero = tilemap.GetCellCenterWorld(tilemap.origin);
        DefineDirectionPriorty();
        StartCoroutine(PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, cc.bounds.center), cameFromNode = null }, 0));
    }
    private IEnumerator PathCheck(NodeInfo nodeToCheck, int nodeRoutesToGetThere)
    {
        Vector3 deltaPos = finalPoint.position - MathMethods.GridToWorld(tilemapPointZero, nodeToCheck.gridPosition, tilemap.cellSize);
        NodeInfo thisNode = new NodeInfo() { gridPosition = nodeToCheck.gridPosition, cameFromNode = nodeToCheck.cameFromNode, distanceFromFinalPosition = deltaPos.x + deltaPos.y };
        knownNodes.Add(thisNode);
        foreach (Directions direction in directionSearchOrder)
        {
            switch (direction)
            {
                case Directions.top:
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(0, 1), nodesToGetThere = nodeRoutesToGetThere + 1 });
                    break;
                case Directions.bottom:
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(0, -1), nodesToGetThere = nodeRoutesToGetThere + 1 });
                    break;
                case Directions.left:
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(-1, 0), nodesToGetThere = nodeRoutesToGetThere + 1 });
                    break;
                case Directions.right:
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(1, 0), nodesToGetThere = nodeRoutesToGetThere + 1 });
                    break;
            }
        }
        yield break;
    }
    private void startCoroutines()
    {

    }
    void clearRepeatedNodes()
    {
        List<NodeInfo> removeThisNodes = new List<NodeInfo>();
        foreach (NodeInfo nodeInfo in nodeQueue)
        {
            foreach (NodeInfo nodeInfo2 in nodeQueue)
            {
                if (nodeInfo != nodeInfo2 && nodeInfo.gridPosition == nodeInfo2.gridPosition)
                {
                    if (nodeInfo.nodesToGetThere >= nodeInfo2.nodesToGetThere)
                    {
                        bool found=false;
                        foreach (NodeInfo remove in removeThisNodes)
                        {
                            if (remove == nodeInfo2) found = true;
                        }
                        if(!found)removeThisNodes.Add(nodeInfo2);
                    }
                    else
                    {
                        bool found = false;
                        foreach (NodeInfo remove in removeThisNodes)
                        {
                            if (remove == nodeInfo) found = true;
                        }
                        if(!found)removeThisNodes.Add(nodeInfo);
                    }
                }
            }
        }
    }
    //NodeInfo[] TopXLowestDistanceFromFinalPosition(int X)
    //{

    //}
    void DefineDirectionPriorty()
    {
        Vector2 deltaFinalPoint = new Vector3(finalPoint.position.x, finalPoint.position.y) - cc.bounds.center;
        if (Mathf.Abs(deltaFinalPoint.x) > Mathf.Abs(deltaFinalPoint.y))
        {
            int i = 0;
            if (deltaFinalPoint.x >= 0)
            {
                directionSearchOrder[i] = Directions.right;
                i = i + 2;
                directionSearchOrder[i] = Directions.left;
                i = 0;
            }
            else if (deltaFinalPoint.x < 0)
            {
                directionSearchOrder[i] = Directions.left;
                i = i + 2;
                directionSearchOrder[i] = Directions.right;
                i = 0;
            }
            i++;
            if (deltaFinalPoint.y >= 0)
            {
                directionSearchOrder[i] = Directions.top;
                i = i + 2;
                directionSearchOrder[i] = Directions.bottom;
                i = 0;
            }
            else if (deltaFinalPoint.y < 0)
            {
                directionSearchOrder[i] = Directions.bottom;
                i = i + 2;
                directionSearchOrder[i] = Directions.top;
                i = 0;
            }
        }
        else if (Mathf.Abs(deltaFinalPoint.x) < Mathf.Abs(deltaFinalPoint.y))
        {
            int i = 0;
            if (deltaFinalPoint.y >= 0)
            {
                directionSearchOrder[i] = Directions.top;
                i = i + 2;
                directionSearchOrder[i] = Directions.bottom;
                i = 0;
            }
            else if (deltaFinalPoint.y < 0)
            {
                directionSearchOrder[i] = Directions.bottom;
                i = i + 2;
                directionSearchOrder[i] = Directions.top;
                i = 0;
            }
            i++;
            if (deltaFinalPoint.x >= 0)
            {
                directionSearchOrder[i] = Directions.right;
                i = i + 2;
                directionSearchOrder[i] = Directions.left;
                i = 0;
            }
            else if (deltaFinalPoint.x < 0)
            {
                directionSearchOrder[i] = Directions.left;
                i = i + 2;
                directionSearchOrder[i] = Directions.right;
                i = 0;
            }
        }
        else
        {
            directionSearchOrder[0] = Directions.top;
            directionSearchOrder[1] = Directions.right;
            directionSearchOrder[2] = Directions.bottom;
            directionSearchOrder[3] = Directions.left;
        }
    }
}
