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

    public int maxCoroutines;
    private int coroutinesRunning;

    List<Vector3> gizmosRoute = new List<Vector3>();
    Coroutine[] pathCheckRunning;

    bool found = false;
    private void OnDrawGizmos()
    {
        if (showGizmosOnPlay)
        {
            Gizmos.DrawCube(tilemapPointZero, new Vector3(0.1f, 0.1f));
            if (found)
            {
                foreach (Vector3 gridPosNode in gizmosRoute)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero, gridPosNode, tilemap.cellSize), new Vector3(0.2f, 0.2f));
                }
            }
            else
            {
                foreach (NodeInfo nodeInfo in knownNodes)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero, nodeInfo.gridPosition, tilemap.cellSize), new Vector3(0.1f, 0.1f));
                }
            }
        }
    }
    private void Awake()
    {
        wallNodes = new List<NodeInfo>();
        nodeQueue = new List<NodeInfo>();
        knownNodes = new List<NodeInfo>();
        route = new List<NodeInfo>();
        pathCheckRunning = new Coroutine[maxCoroutines];
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
        pathCheckRunning[0] = StartCoroutine(PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, cc.bounds.center), cameFromNode = null }, 0));
    }
    private void Update()
    {
        //se mudar
        if (found)
        {
            NodeInfo nearestNode = new NodeInfo() {distanceFromFinalPosition = Mathf.Infinity};
            for (int i=0; i < route.Count;i++)
            {
                if (route[i].distanceFromFinalPosition < nearestNode.distanceFromFinalPosition)
                {
                    nearestNode = route[i];
                }
            }
        }
        else
        {

        }
    }
    private IEnumerator PathCheck(NodeInfo nodeToCheck, int nodeRoutesToGetThere)
    {
        Debug.Log(nodeToCheck.gridPosition);
        coroutinesRunning++;
        Vector3 deltaPos = finalPoint.position - MathMethods.GridToWorld(tilemapPointZero, nodeToCheck.gridPosition, tilemap.cellSize);
        NodeInfo thisNode = new NodeInfo() { gridPosition = nodeToCheck.gridPosition, cameFromNode = nodeToCheck.cameFromNode, distanceFromFinalPosition = Mathf.Abs(deltaPos.x) + Mathf.Abs(deltaPos.y) };
        if (nodeToCheck.gridPosition == MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, finalPoint.position))
        {
            NodeInfo nodeInfo;
            nodeInfo = nodeToCheck;
            for (int i = 0; i <= nodeRoutesToGetThere; i++)
            {
                Debug.Log(nodeInfo.gridPosition);
                gizmosRoute.Add(nodeInfo.gridPosition);
                route.Add(nodeInfo);
                nodeInfo = nodeInfo.cameFromNode;
            }
            foreach (Coroutine coroutine in pathCheckRunning)
            {
                if(coroutine!=null)StopCoroutine(coroutine);
            }
            found = true;
            yield break;
        }
        knownNodes.Add(thisNode);
        foreach (Directions direction in directionSearchOrder)
        {
            Vector3 nextNodeDistanceFromFinalPoint;
            switch (direction)
            {
                case Directions.top:
                    nextNodeDistanceFromFinalPoint = finalPoint.position - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(0, 1), tilemap.cellSize);
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(0, 1), nodesToGetThere = nodeRoutesToGetThere + 1,distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                    break;
                case Directions.bottom:
                    nextNodeDistanceFromFinalPoint = finalPoint.position - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(0, -1), tilemap.cellSize);
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(0, -1), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                    break;
                case Directions.left:
                    nextNodeDistanceFromFinalPoint = finalPoint.position - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(-1, 0), tilemap.cellSize);
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(-1, 0), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                    break;
                case Directions.right:
                    nextNodeDistanceFromFinalPoint = finalPoint.position - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(1, 0), tilemap.cellSize);
                    nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(1, 0), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                    break;
            }
        }
        clearRepeatedNodes();
        coroutinesRunning--;
        yield return new WaitForFixedUpdate();
        int a = coroutinesRunning;
        foreach (NodeInfo nodeInfo in TopXLowestDistanceFromFinalPosition(maxCoroutines - coroutinesRunning))
        {
            pathCheckRunning[a] = StartCoroutine(PathCheck(nodeInfo, nodeInfo.nodesToGetThere));
            nodeQueue.Remove(nodeInfo);
            a++;
        }
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
                    if (nodeInfo.nodesToGetThere < nodeInfo2.nodesToGetThere)
                    {
                        bool found = false;
                        foreach (NodeInfo remove in removeThisNodes)
                        {
                            if (remove == nodeInfo2) found = true;
                        }
                        if (!found) removeThisNodes.Add(nodeInfo2);
                    }
                    else if (nodeInfo.nodesToGetThere > nodeInfo2.nodesToGetThere)
                    {
                        bool found = false;
                        foreach (NodeInfo remove in removeThisNodes)
                        {
                            if (remove == nodeInfo) found = true;
                        }
                        if (!found) removeThisNodes.Add(nodeInfo);
                    }
                    else
                    {
                        if (nodeInfo.GetHashCode() > nodeInfo2.GetHashCode())
                        {
                            bool found = false;
                            foreach (NodeInfo remove in removeThisNodes)
                            {
                                if (remove == nodeInfo) found = true;
                            }
                            if (!found) removeThisNodes.Add(nodeInfo);
                        }
                        else
                        {
                            bool found = false;
                            foreach (NodeInfo remove in removeThisNodes)
                            {
                                if (remove == nodeInfo2) found = true;
                            }
                            if (!found) removeThisNodes.Add(nodeInfo2);
                        }
                    }
                }
            }
        }
        foreach (NodeInfo nodeInfo in removeThisNodes)
        {
            nodeQueue.Remove(nodeInfo);
        }
    }
    List<NodeInfo> TopXLowestDistanceFromFinalPosition(int X)
    {
        List<NodeInfo> LowestDistanceList = new List<NodeInfo>();
        for (int i = 0; i < X; i++)
        {
            LowestDistanceList.Add(new NodeInfo());
        }
        for (int i = 0; i < X; i++)
        {
            LowestDistanceList[i] = new NodeInfo() { distanceFromFinalPosition = float.PositiveInfinity };
        }
        foreach (NodeInfo nodeQ in nodeQueue)
        {
            for (int i = 0; i < X; i++)
            {
                if (nodeQ.distanceFromFinalPosition < LowestDistanceList[i].distanceFromFinalPosition)
                {
                    for (int a = X - 1 - 1; a > i; a--)
                    {
                        LowestDistanceList[a] = LowestDistanceList[a - 1];
                    }
                    LowestDistanceList[i] = nodeQ;
                    break;
                }
            }
        }
        return LowestDistanceList;
    }
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
