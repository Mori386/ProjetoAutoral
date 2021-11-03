using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    public Transform finalPoint;

    [SerializeField] private Tilemap tilemap;

    public LayerMask collideWithLayers;
    private Vector2 tilemapPointZero;

    private Vector2 centerCollider;

    public bool finished;
    public List<NodeInfo> route = new List<NodeInfo>();

    private List<NodeInfo> NodeInfoList;
    private List<NodeInfo> NodesQueueList;
    int coroutinesRunning = 0;

    AiTest aiTest;
    Grid grid;
    // pode apagar esse pf dps 
    [SerializeField] private GameObject pf;
    private enum Directions
    {
        top, left, right, bottom
    }
    Directions[] directionSearchOrder = new Directions[4];
    Vector3 roberto;
    Vector3 joia;

    Vector3 atualFinalPosition;

    public bool foundNewPos;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawCube(centerCollider, new Vector2(0.1f, 0.1f));
        Gizmos.DrawCube(roberto, joia);
    }
    private void Awake()
    {
        NodeInfoList = new List<NodeInfo>();
        NodesQueueList = new List<NodeInfo>();
    }
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        aiTest = GetComponent<AiTest>();

        tilemapPointZero = tilemap.origin + tilemap.transform.position;
        tilemapPointZero = GridPosition.NearGridPosition(tilemapPointZero);
        centerCollider = GetComponent<CapsuleCollider2D>().bounds.center;

        roberto = gridToWorld(CellPositionCenter(centerCollider));

        Vector2 deltaFinalPoint = new Vector2(finalPoint.position.x, finalPoint.position.y) - centerCollider;
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
        StartCoroutine(PathSetup(CellPositionCenter(centerCollider), null, CellPositionCenter(finalPoint.position)));
        atualFinalPosition = CellPositionCenter(finalPoint.position);
    }
    private void FixedUpdate()
    {
        if (atualFinalPosition != CellPositionCenter(finalPoint.position) && !aiTest.stunned && !aiTest.enraged)
        {
            foundNewPos = true;
            StopAllCoroutines();
            coroutinesRunning = 0;
            route = new List<NodeInfo>();
            NodeInfoList = new List<NodeInfo>();
            NodesQueueList = new List<NodeInfo>();
            Vector2 deltaFinalPoint = new Vector2(finalPoint.position.x, finalPoint.position.y) - centerCollider;
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
            centerCollider = GetComponent<CapsuleCollider2D>().bounds.center;
            StartCoroutine(PathSetup(CellPositionCenter(centerCollider), null, CellPositionCenter(finalPoint.position)));
            atualFinalPosition = CellPositionCenter(finalPoint.position);
        }
    }
    public void restartPathfinding()
    {
        foundNewPos = true;
        StopAllCoroutines();
        coroutinesRunning = 0;
        route = new List<NodeInfo>();
        NodeInfoList = new List<NodeInfo>();
        NodesQueueList = new List<NodeInfo>();
        Vector2 deltaFinalPoint = new Vector2(finalPoint.position.x, finalPoint.position.y) - centerCollider;
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
        centerCollider = GetComponent<CapsuleCollider2D>().bounds.center;
        StartCoroutine(PathSetup(CellPositionCenter(centerCollider), null, CellPositionCenter(finalPoint.position)));
        atualFinalPosition = CellPositionCenter(finalPoint.position);
    }
    IEnumerator PathSetup(Vector2 startGridPos, NodeInfo cameFrom, Vector3 finalPointPosition)
    {
        NodeInfo thisNode = new NodeInfo { gridPosition = startGridPos, cameFromNode = cameFrom };
        NodesQueueList.Remove(thisNode);
        NodeInfoList.Add(thisNode);
        if (startGridPos == new Vector2(finalPointPosition.x, finalPointPosition.y))
        {
            route.Add(thisNode);
            NodeInfo node = thisNode;
            while (true)
            {
                if (node.cameFromNode != null)
                {
                    route.Add(node.cameFromNode);
                    node = node.cameFromNode;
                }
                else
                {
                    break;
                }
            }
            if (aiTest.goingToNode != null)
            {
                route.Remove(route[route.Count-1]);
                route.Add(aiTest.goingToNode);
            }
            //foreach (NodeInfo nodeInfo in route)
            //{
            //GameObject go = Instantiate(pf, Vector2.Scale(nodeInfo.gridPosition, grid.cellSize) + tilemapPointZero - new Vector2(grid.cellSize.x, grid.cellSize.y) * 0.5f, Quaternion.identity);
            //go.GetComponent<SpriteRenderer>().color = new Color(0,1,0,0.1f);
            //go.GetComponent<SpriteRenderer>().sortingOrder = 10;
            //Debug.Log(nodeInfo.gridPosition);
            //}
            //Debug.Log(test);
            finished = true;
            StopAllCoroutines();
            yield break;
        }
        coroutinesRunning++;
        //Instantiate(pf, Vector2.Scale(startGridPos, grid.cellSize) + tilemapPointZero - new Vector2(grid.cellSize.x, grid.cellSize.y) * 0.5f, Quaternion.identity);
        foreach (Directions direction in directionSearchOrder)
        {
            bool found = false;
            switch (direction)
            {
                case Directions.top:
                    #region
                    foreach (NodeInfo nodeInfo in NodeInfoList)
                    {
                        if ((startGridPos + new Vector2(0, 1)) == nodeInfo.gridPosition)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        roberto = gridToWorld(startGridPos + new Vector2(0, 1));
                        joia = new Vector3(0.25f, 0.25f);
                        RaycastHit2D gridHit = Physics2D.BoxCast(gridToWorld(startGridPos + new Vector2(0, 1)), new Vector3(0.25f, 0.25f), 0f, new Vector2(0, 0), Mathf.Infinity, collideWithLayers);
                        if(!gridHit || gridHit.collider.isTrigger)
                        {
                            //Instantiate(pf, gridToWorld(startGridPos + new Vector2(0, 1)), Quaternion.identity).GetComponent<SpriteRenderer>().color= new Color(0, 0, 1, 0.1f);
                            NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos + new Vector2(0, 1), cameFromNode = thisNode, distanceFromFinalPosition = Vector2.Distance(finalPoint.position, gridToWorld(startGridPos)) });
                        }
                    }
                    #endregion
                    break;

                case Directions.right:
                    #region
                    foreach (NodeInfo nodeInfo in NodeInfoList)
                    {
                        if ((startGridPos + new Vector2(1, 0)) == nodeInfo.gridPosition)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        RaycastHit2D gridHit = Physics2D.BoxCast(gridToWorld(startGridPos + new Vector2(1, 0)), new Vector3(0.25f, 0.25f), 0f, new Vector2(0, 0), Mathf.Infinity, collideWithLayers);
                        if (!gridHit || gridHit.collider.isTrigger)
                        {
                            //Instantiate(pf, gridToWorld(startGridPos + new Vector2(1, 0)), Quaternion.identity).GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.1f);
                            NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos + new Vector2(1, 0), cameFromNode = thisNode, distanceFromFinalPosition = Vector2.Distance(finalPoint.position, gridToWorld(startGridPos)) });
                        }
                    }
                    #endregion
                    break;

                case Directions.left:
                    #region
                    foreach (NodeInfo nodeInfo in NodeInfoList)
                    {
                        if ((startGridPos - new Vector2(1, 0)) == nodeInfo.gridPosition)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        RaycastHit2D gridHit = Physics2D.BoxCast(gridToWorld(startGridPos - new Vector2(1, 0)), new Vector3(0.25f, 0.25f), 0f, new Vector2(0, 0), Mathf.Infinity, collideWithLayers);
                        if (!gridHit || gridHit.collider.isTrigger)
                        {
                            //Instantiate(pf, gridToWorld(startGridPos - new Vector2(1, 0)), Quaternion.identity).GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.1f);
                            NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos - new Vector2(1, 0), cameFromNode = thisNode, distanceFromFinalPosition = Vector2.Distance(finalPoint.position, gridToWorld(startGridPos)) });
                        }
                    }
                    #endregion
                    break;

                case Directions.bottom:
                    #region
                    foreach (NodeInfo nodeInfo in NodeInfoList)
                    {
                        if ((startGridPos - new Vector2(0, 1)) == nodeInfo.gridPosition)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        RaycastHit2D gridHit = Physics2D.BoxCast(gridToWorld(startGridPos - new Vector2(0, 1)), new Vector3(0.25f, 0.25f), 0f, new Vector2(0, 0), Mathf.Infinity, collideWithLayers);
                        if (!gridHit || gridHit.collider.isTrigger)
                        {
                            //Instantiate(pf, gridToWorld(startGridPos - new Vector2(0, 1)), Quaternion.identity).GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.1f);
                            NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos - new Vector2(0, 1), cameFromNode = thisNode, distanceFromFinalPosition = Vector2.Distance(finalPoint.position, gridToWorld(startGridPos)) });
                        }
                    }
                    #endregion
                    break;
            }
        }
        yield return new WaitForFixedUpdate();
        coroutinesRunning--;
        checkCoroutineList(finalPointPosition);
    }
    public Vector3 gridToWorld(Vector3 gridPos)
    {
        return Vector2.Scale(gridPos, grid.cellSize) + tilemapPointZero - new Vector2(grid.cellSize.x, grid.cellSize.y) * 0.5f;
    }
    private void checkCoroutineList(Vector3 finalPointPosition)
    {
        //if (coroutinesRunning < 30)
        //{
        List<NodeInfo> removeThis = new List<NodeInfo>();
        foreach (NodeInfo nodeInfo in NodeInfoList)
        {
            for (int i = 0; i < NodesQueueList.Count; i++)
            {
                if (nodeInfo.gridPosition == NodesQueueList[i].gridPosition)
                {
                    removeThis.Add(NodesQueueList[i]);
                }
            }
        }
        foreach (NodeInfo node in removeThis)
        {
            NodesQueueList.Remove(node);
        }
        int coroutineNeeded = 16 - coroutinesRunning;
        List<NodeInfo> highPriorityNodes = new List<NodeInfo>();
        for (int i = 0; i < coroutineNeeded; i++)
        {
            highPriorityNodes.Add(new NodeInfo() { distanceFromFinalPosition = float.PositiveInfinity });
        }
        foreach (NodeInfo nodeInfo in NodesQueueList)
        {
            for (int i = 0; i < coroutineNeeded; i++)
            {
                if (nodeInfo.distanceFromFinalPosition < highPriorityNodes[i].distanceFromFinalPosition)
                {
                    highPriorityNodes[i] = nodeInfo;
                    for (int a = coroutineNeeded - 1; a > i; a--)
                    {
                        highPriorityNodes[a] = highPriorityNodes[a - 1];
                    }
                    break;
                }
            }
        }
        for (int i = 0; i < coroutineNeeded; i++)
        {
            StartCoroutine(PathSetup(highPriorityNodes[i].gridPosition, highPriorityNodes[i].cameFromNode, finalPointPosition));
        }
        //Debug.Log(coroutineVector2List[0]);
        //}
    }
    public Vector3 CellPositionCenter(Vector3 position)
    {
        Vector3 nearGridPosition = new Vector3(0, 0, 0);
        if (nearGridPosition.x > 0)
        {
            nearGridPosition.x = Mathf.Floor((position.x - tilemapPointZero.x) / grid.cellSize.x);
        }
        else
        {
            nearGridPosition.x = Mathf.Ceil((position.x - tilemapPointZero.x) / grid.cellSize.x);
        }
        if (nearGridPosition.y > 0)
        {
            nearGridPosition.y = Mathf.Floor((position.y - tilemapPointZero.y) / grid.cellSize.y);
        }
        else
        {
            nearGridPosition.y = Mathf.Ceil((position.y - tilemapPointZero.y) / grid.cellSize.y);
        }
        return nearGridPosition;
    }
}
