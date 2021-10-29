using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform finalPoint;

    [SerializeField] private Tilemap tilemap;
    private Vector2 tilemapPointZero;

    private Vector2 centerCollider;

    [SerializeField] private LayerMask layerThatCantCollide;
    [SerializeField] private bool canDetectIsTriggeredColliders;
    private List<NodeInfo> NodeInfoList;
    private List<NodeInfo> NodesQueueList;
    private List<NodeInfo> nodeCameFrom;
    int coroutinesRunning = 0;


    Grid grid;
    // pode apagar esse pf dps 
    [SerializeField] private GameObject pf;
    private enum Directions
    {
        top, left, right, bottom
    }
    Directions[] directionSearchOrder = new Directions[4];
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(centerCollider, new Vector2(0.1f, 0.1f));
    }

    private void Awake()
    {
        NodeInfoList = new List<NodeInfo>();
        NodesQueueList = new List<NodeInfo>();
        nodeCameFrom = new List<NodeInfo>();
    }
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();

        tilemapPointZero = tilemap.origin + tilemap.transform.position;
        tilemapPointZero = GridPosition.NearGridPosition(tilemapPointZero);

        centerCollider = GetComponent<CapsuleCollider2D>().bounds.center;

        //Debug.Log(CellPositionCenter(centerCollider - tilemapPointZero));

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
        StartCoroutine(PathSetup(CellPositionCenter(centerCollider), null));
    }
    IEnumerator PathSetup(Vector2 startGridPos, NodeInfo cameFrom)
    {
        //Debug.Log(startGridPos);
        Vector2 test = CellPositionCenter(finalPoint.position);
        NodeInfo thisNode = new NodeInfo { gridPosition = startGridPos, cameFromNode = cameFrom };
        NodesQueueList.Remove(thisNode);
        NodeInfoList.Add(thisNode);
        if (startGridPos == new Vector2(test.x, test.y))
        {
            List<NodeInfo> route = new List<NodeInfo>();
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
            foreach (NodeInfo nodeInfo in route)
            {
                GameObject go = Instantiate(pf, Vector2.Scale(nodeInfo.gridPosition, grid.cellSize) - tilemapPointZero - new Vector2(grid.cellSize.x, grid.cellSize.y) * 0.5f, Quaternion.identity);
                go.GetComponent<SpriteRenderer>().color = Color.green;
                go.GetComponent<SpriteRenderer>().sortingOrder = 10;
                //Debug.Log(nodeInfo.gridPosition);
            }
            //Debug.Log(test);
            StopAllCoroutines();
            yield break;
        }
        coroutinesRunning++;
        Instantiate(pf, Vector2.Scale(startGridPos, grid.cellSize) - tilemapPointZero - new Vector2(grid.cellSize.x, grid.cellSize.y) * 0.5f, Quaternion.identity);
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
                        NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos + new Vector2(0, 1), cameFromNode = thisNode });
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
                        NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos + new Vector2(1, 0), cameFromNode = thisNode });
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
                        NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos - new Vector2(1, 0), cameFromNode = thisNode });
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
                        NodesQueueList.Add(new NodeInfo { gridPosition = startGridPos - new Vector2(0, 1), cameFromNode = thisNode });
                    }
                    #endregion
                    break;
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        coroutinesRunning--;
        checkCoroutineList();
    }
    private void checkCoroutineList()
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
        for (int i = 0; i <= 8 - coroutinesRunning; i++)
        {
            StartCoroutine(PathSetup(NodesQueueList[i].gridPosition, NodesQueueList[i].cameFromNode));
        }
        //Debug.Log(coroutineVector2List[0]);
        //}
    }
    private Vector3 CellPositionCenter(Vector3 position)
    {
        Vector3 nearGridPosition = new Vector3(0, 0, 0);
        if (nearGridPosition.x > 0)
        {
            nearGridPosition.x = Mathf.Floor(position.x- tilemapPointZero.x / grid.cellSize.x);
        }
        else
        {
            nearGridPosition.x = Mathf.Ceil(position.x - tilemapPointZero.x / grid.cellSize.x);
        }
        if (nearGridPosition.y > 0)
        {
            nearGridPosition.y = Mathf.Floor(position.y - tilemapPointZero.y / grid.cellSize.y);
        }
        else
        {
            nearGridPosition.y = Mathf.Ceil(position.y - tilemapPointZero.y / grid.cellSize.y);
        }
        return nearGridPosition;
    }
}
