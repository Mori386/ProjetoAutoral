﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingV2 : MonoBehaviour
{
    //Setup
    bool showGizmosOnPlay;
    private enum enemyType
    {
        Boss, Enemy01
    }
    [SerializeField] private enemyType EnemyType;
    AiBoss aiBoss;

    //Componentes
    [System.NonSerialized]public CapsuleCollider2D cc;

    //tilemap
    public Transform player;
    public Tilemap tilemap;
    [System.NonSerialized] public Vector3 tilemapPointZero;

    //Nodulos Salvos
    List<NodeInfo> wallNodes;
    public List<NodeInfo> route;
    List<NodeInfo> updatedRoute = new List<NodeInfo>();

    //Variaveis 
    private enum Directions
    {
        top, left, right, bottom
    }
    Directions[] directionSearchOrder = new Directions[4];

    public Coroutine[] pathCheckRunning;

    [SerializeField] LayerMask collideWithLayer;

    public float moveSpeed;

    public float chargeTime;

    public float dashSpeed;

    NodeInfo futureNode;

    float deltaPosInTheNextXSec;

    public GameObject pedraPf;

    [System.NonSerialized]public bool found = false;
    private void OnDrawGizmos()
    {
        if (showGizmosOnPlay)
        {
            if (futureNode != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero, futureNode.gridPosition, tilemap.cellSize), new Vector3(0.5f, 0.5f));
            }
            foreach (NodeInfo node in updatedRoute)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero, node.gridPosition, tilemap.cellSize), new Vector3(0.3f, 0.3f));
            }
            Gizmos.DrawCube(tilemapPointZero, new Vector3(0.1f, 0.1f));
            if (found)
            {
                foreach (NodeInfo nodeRoute in route)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(MathMethods.GridToWorld(tilemapPointZero, nodeRoute.gridPosition, tilemap.cellSize), new Vector3(0.2f, 0.2f));
                }
            }
        }
    }
    private void Awake()
    {
        if (EnemyType == enemyType.Boss)
        {
            aiBoss = gameObject.AddComponent<AiBoss>();
        }
        wallNodes = new List<NodeInfo>();
        route = new List<NodeInfo>();
        pathCheckRunning = new Coroutine[2];
        foreach (CapsuleCollider2D capsuleCollider2D in GetComponents<CapsuleCollider2D>())
        {
            if (!capsuleCollider2D.isTrigger) cc = GetComponent<CapsuleCollider2D>();
        }
        deltaPosInTheNextXSec = moveSpeed * 3f / (1 / tilemap.cellSize.x);
    }
    private void Start()
    {
        showGizmosOnPlay = true;
        tilemapPointZero = tilemap.GetCellCenterWorld(tilemap.origin);
        DefineDirectionPriorty(player.position);
        pathCheckRunning[0] = StartCoroutine(PathCheck(new NodeInfo() { gridPosition = MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, cc.bounds.center), cameFromNode = null }, player.position, 0, false));
    }
    private void Update()
    {
        //se mudar
        if (found && route[route.Count - 1].gridPosition != MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, player.position))
        {
            NodeInfo nearestNode = new NodeInfo() { distanceFromFinalPosition = Mathf.Infinity };
            int positionInRoute = 0;
            foreach (NodeInfo routeNode in route)
            {
                Vector3 deltaPos = player.position - MathMethods.GridToWorld(tilemapPointZero, routeNode.gridPosition, tilemap.cellSize);
                routeNode.distanceFromFinalPosition = Mathf.Abs(deltaPos.x) + Mathf.Abs(deltaPos.y);
            }
            for (int i = 0; i < route.Count; i++)
            {
                if (route[i].distanceFromFinalPosition < nearestNode.distanceFromFinalPosition)
                {
                    nearestNode = route[i];
                    positionInRoute = i;
                }
                else if (route[i].distanceFromFinalPosition == nearestNode.distanceFromFinalPosition)
                {
                    if (route[i].nodesToGetThere < nearestNode.nodesToGetThere)
                    {
                        nearestNode = route[i];
                        positionInRoute = i;
                    }
                    else if (route[i].nodesToGetThere == nearestNode.nodesToGetThere)
                    {
                        if (route[i].GetHashCode() < nearestNode.GetHashCode())
                        {
                            nearestNode = route[i];
                            positionInRoute = i;
                        }
                    }
                }
            }
            int nodesNeededToBeRemoved = route.Count - positionInRoute;
            for (int i = 0; i < nodesNeededToBeRemoved; i++)
            {
                route.Remove(route[positionInRoute]);
            }
            found = false;
            pathCheckRunning[0] = StartCoroutine(PathCheck(new NodeInfo() { gridPosition = nearestNode.gridPosition, cameFromNode = null }, player.position, nearestNode.nodesToGetThere, false));
        }
    }
    public IEnumerator PathCheck(NodeInfo nodeToStart, Vector3 finalPoint, int startNodesRoutesInt, bool secondCheck)
    {
        List<NodeInfo> nodeQueue = new List<NodeInfo>();
        List<NodeInfo> knownNodes = new List<NodeInfo>();
        NodeInfo nodeToCheck = nodeToStart;
        int nodeRoutesToGetThere = startNodesRoutesInt;
        while (true)
        {
            Vector3 deltaPos = finalPoint - MathMethods.GridToWorld(tilemapPointZero, nodeToCheck.gridPosition, tilemap.cellSize);
            NodeInfo thisNode = new NodeInfo() { gridPosition = nodeToCheck.gridPosition, cameFromNode = nodeToCheck.cameFromNode, distanceFromFinalPosition = Mathf.Abs(deltaPos.x) + Mathf.Abs(deltaPos.y) };
            if (nodeToCheck.gridPosition == MathMethods.WorldToGrid(tilemapPointZero, tilemap.cellSize, finalPoint))
            {
                NodeInfo nodeInfo;
                nodeInfo = nodeToCheck;
                List<NodeInfo> invertedRoute = new List<NodeInfo>();
                while (true)
                {
                    if (nodeInfo != null)
                    {
                        invertedRoute.Add(nodeInfo);
                        nodeInfo = nodeInfo.cameFromNode;
                    }
                    else break;
                }
                if (!secondCheck)
                {
                    for (int i = 0; i < invertedRoute.Count; i++)
                    {
                        route.Add(invertedRoute[invertedRoute.Count - 1 - i]);
                    }
                    found = true;
                    if (route.Count > 0)
                    {
                        if (Mathf.CeilToInt(deltaPosInTheNextXSec) < route.Count - 1)
                        {
                            futureNode = route[Mathf.CeilToInt(deltaPosInTheNextXSec)];
                            futureNode.cameFromNode = null;
                            pathCheckRunning[1] = StartCoroutine(PathCheck(futureNode, MathMethods.GridToWorld(tilemapPointZero, route[route.Count - 1].gridPosition, tilemap.cellSize), futureNode.nodesToGetThere, true));
                        }
                    }
                }
                else if (invertedRoute[0].nodesToGetThere+ Mathf.CeilToInt(deltaPosInTheNextXSec) < route[route.Count - 1].nodesToGetThere)
                {
                    updatedRoute = new List<NodeInfo>();
                    for (int i = invertedRoute.Count - 1; i >= 0; i--)
                    {
                        updatedRoute.Add(invertedRoute[i]);
                    }
                    for(int i =0; i<route.Count;i++)
                    {
                        if (route[i].gridPosition == updatedRoute[0].gridPosition)
                        {
                            while (true)
                            {
                                if (route.Count != i) route.Remove(route[i]);
                                else break;
                            }
                            for (int a = 0; a < updatedRoute.Count; a++)
                            {
                                route.Add(updatedRoute[a]);
                            }
                            break;
                        }
                    }
                    found = true;
                }
                yield break;
            }
            knownNodes.Add(thisNode);
            foreach (Directions direction in directionSearchOrder)
            {
                Vector3 nextNodeDistanceFromFinalPoint;
                RaycastHit2D raycastHit2D;
                bool cancelThisDirectionSearch = false;
                switch (direction)
                {
                    case Directions.top:
                        foreach (NodeInfo wallNode in wallNodes)
                        {
                            if (wallNode.gridPosition == (thisNode.gridPosition + new Vector2(0, 1)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        foreach (NodeInfo knownNode in knownNodes)
                        {
                            if (knownNode.gridPosition == (thisNode.gridPosition + new Vector2(0, 1)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        raycastHit2D = Physics2D.BoxCast(MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(0, 1), tilemap.cellSize),
                            new Vector3(0.25f, 0.25f),
                            0f,
                            new Vector2(0, 0),
                            Mathf.Infinity,
                            collideWithLayer);
                        if (!raycastHit2D || raycastHit2D.collider.isTrigger)
                        {
                            nextNodeDistanceFromFinalPoint = finalPoint - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(0, 1), tilemap.cellSize);
                            nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(0, 1), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                        }
                        else if (raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        {
                            wallNodes.Add(new NodeInfo() { gridPosition = thisNode.gridPosition + new Vector2(0, 1) });
                        }
                        break;
                    case Directions.bottom:
                        foreach (NodeInfo wallNode in wallNodes)
                        {
                            if (wallNode.gridPosition == (thisNode.gridPosition + new Vector2(0, -1)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        foreach (NodeInfo knownNode in knownNodes)
                        {
                            if (knownNode.gridPosition == (thisNode.gridPosition + new Vector2(0, -1)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        raycastHit2D = Physics2D.BoxCast(MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(0, -1), tilemap.cellSize),
                            new Vector3(0.25f, 0.25f),
                            0f,
                            new Vector2(0, 0),
                            Mathf.Infinity,
                            collideWithLayer);
                        if (!raycastHit2D || raycastHit2D.collider.isTrigger)
                        {
                            nextNodeDistanceFromFinalPoint = finalPoint - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(0, -1), tilemap.cellSize);
                            nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(0, -1), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                        }
                        else if (raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        {
                            wallNodes.Add(new NodeInfo() { gridPosition = thisNode.gridPosition + new Vector2(0, -1) });
                        }
                        break;
                    case Directions.left:
                        foreach (NodeInfo wallNode in wallNodes)
                        {
                            if (wallNode.gridPosition == (thisNode.gridPosition + new Vector2(-1, 0)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        foreach (NodeInfo knownNode in knownNodes)
                        {
                            if (knownNode.gridPosition == (thisNode.gridPosition + new Vector2(-1, 0)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        raycastHit2D = Physics2D.BoxCast(MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(-1, 0), tilemap.cellSize),
                            new Vector3(0.25f, 0.25f),
                            0f,
                            new Vector2(0, 0),
                            Mathf.Infinity,
                            collideWithLayer);
                        if (!raycastHit2D || raycastHit2D.collider.isTrigger)
                        {
                            nextNodeDistanceFromFinalPoint = finalPoint - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(-1, 0), tilemap.cellSize);
                            nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(-1, 0), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                        }
                        else if (raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        {
                            wallNodes.Add(new NodeInfo() { gridPosition = thisNode.gridPosition + new Vector2(-1, 0) });
                        }
                        break;
                    case Directions.right:
                        foreach (NodeInfo wallNode in wallNodes)
                        {
                            if (wallNode.gridPosition == (thisNode.gridPosition + new Vector2(1, 0)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        foreach (NodeInfo knownNode in knownNodes)
                        {
                            if (knownNode.gridPosition == (thisNode.gridPosition + new Vector2(1, 0)))
                            {
                                cancelThisDirectionSearch = true;
                                break;
                            }
                        }
                        if (cancelThisDirectionSearch) continue;
                        raycastHit2D = Physics2D.BoxCast(MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(1, 0), tilemap.cellSize),
                            new Vector3(0.25f, 0.25f),
                            0f,
                            new Vector2(0, 0),
                            Mathf.Infinity,
                            collideWithLayer);
                        if (!raycastHit2D || raycastHit2D.collider.isTrigger)
                        {
                            nextNodeDistanceFromFinalPoint = finalPoint - MathMethods.GridToWorld(tilemapPointZero, thisNode.gridPosition + new Vector2(1, 0), tilemap.cellSize);
                            nodeQueue.Add(new NodeInfo() { cameFromNode = thisNode, gridPosition = thisNode.gridPosition + new Vector2(1, 0), nodesToGetThere = nodeRoutesToGetThere + 1, distanceFromFinalPosition = Mathf.Abs(nextNodeDistanceFromFinalPoint.x) + Mathf.Abs(nextNodeDistanceFromFinalPoint.y) });
                        }
                        else if (raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        {
                            wallNodes.Add(new NodeInfo() { gridPosition = thisNode.gridPosition + new Vector2(1, 0) });
                        }
                        break;
                }
            }
            nodeQueue = clearRepeatedNodes(nodeQueue);
            yield return new WaitForFixedUpdate();
            foreach (NodeInfo nodeInfo in TopXLowestDistanceFromFinalPosition(1, nodeQueue))
            {
                nodeToCheck = nodeInfo;
                nodeRoutesToGetThere = nodeInfo.nodesToGetThere;
                nodeQueue.Remove(nodeInfo);
            }
        }
    }
    public void StopAllPathCheck()
    {
        foreach (Coroutine coroutine in pathCheckRunning)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }
    }
    List<NodeInfo> clearRepeatedNodes(List<NodeInfo> nodeQueueList)
    {
        List<NodeInfo> nodeQueue = nodeQueueList;
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
        return nodeQueue;
    }
    List<NodeInfo> TopXLowestDistanceFromFinalPosition(int X, List<NodeInfo> nodeQueue)
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
    void DefineDirectionPriorty(Vector3 finalPoint)
    {
        Vector2 deltaFinalPoint = new Vector3(finalPoint.x, finalPoint.y) - cc.bounds.center;
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
