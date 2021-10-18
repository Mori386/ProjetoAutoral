using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOStateMovableTriggered : MovableObjectBaseState
{
    bool isMoving;
    public Vector2 wallDetect;
    GridPosition gridPosition;
    public override void GizmosState(MovableObjectStateManager Manager)
    {

    }
    public override void EnterState(MovableObjectStateManager Manager)
    {
        gridPosition = Manager.GetComponent<GridPosition>();
        CheckWalls();
    }
    public override void UpdateState(MovableObjectStateManager Manager)
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Z))
        {
            Vector2 directionPlayerPush = (Manager.transform.position - Manager.player.transform.position);
            if (Mathf.Abs(directionPlayerPush.x) > Mathf.Abs(directionPlayerPush.y))
            {
                if (directionPlayerPush.x / Mathf.Abs(directionPlayerPush.x) == wallDetect.x)
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, -new Vector3(directionPlayerPush.x / Mathf.Abs(directionPlayerPush.x), 0, 0), -new Vector2Int(Mathf.RoundToInt(directionPlayerPush.x / Mathf.Abs(directionPlayerPush.x)), 0), true));
                }
                else
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(directionPlayerPush.x / Mathf.Abs(directionPlayerPush.x), 0, 0), new Vector2Int(Mathf.RoundToInt(directionPlayerPush.x / Mathf.Abs(directionPlayerPush.x)), 0), false));
                }
            }
            else
            {
                if (directionPlayerPush.y / Mathf.Abs(directionPlayerPush.y) == wallDetect.y)
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager,-new Vector3(0, directionPlayerPush.y / Mathf.Abs(directionPlayerPush.y), 0), -new Vector2Int(0, Mathf.RoundToInt(directionPlayerPush.y / Mathf.Abs(directionPlayerPush.y))),true));
                }
                else
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(0, directionPlayerPush.y / Mathf.Abs(directionPlayerPush.y), 0), new Vector2Int(0, Mathf.RoundToInt(directionPlayerPush.y / Mathf.Abs(directionPlayerPush.y))), false));
                }
            }
        }
    }
    IEnumerator MoveObjectAnimation(MovableObjectStateManager Manager, Vector3 deltaPosition, Vector2Int deltaGridPosition, bool MovePlayerTogether)
    {
        PMStateManager playerMov = Manager.player.GetComponent<PMStateManager>();
        Vector3 startPos = Manager.transform.position;
        deltaPosition = new Vector3(deltaPosition.x * Manager.gridPosition.tilemap.cellSize.x, deltaPosition.y * Manager.gridPosition.tilemap.cellSize.y,0);
        isMoving = true;
        if(MovePlayerTogether) playerMov.SmoothSwitchState(playerMov.controlOffState);
        Vector3 finalPositon = startPos + deltaPosition; 
        while (
            new Vector3(Mathf.Round(Manager.transform.position.x*100)/100, Mathf.Round(Manager.transform.position.y * 100) / 100) != new Vector3(Mathf.Round(finalPositon.x * 100) / 100, Mathf.Round(finalPositon.y * 100) / 100)
            )
        {
            if (MovePlayerTogether) playerMov.transform.position += deltaPosition / 20;
            Manager.transform.position += deltaPosition / 20;
            yield return new WaitForSeconds(0.01f);
        }
        Manager.GetComponent<GridPosition>().gridTilemapPosition += deltaGridPosition;
        if (Manager.objectBase.timePeriod == ObjectBase.timePeriodList.Present)
        {
            if (Manager.objectBase.objectOtherTimeline != null)
            {
                Manager.moveOnFuture();
            }
        }
        CheckWalls();
        isMoving = false;
        if (MovePlayerTogether) playerMov.SmoothSwitchState(playerMov.defaultState);
    }
    public override void FixedUpdateState(MovableObjectStateManager Manager)
    {

    }
    public override void OnTriggerEnterState(MovableObjectStateManager Manager, Collider2D collision)
    {

    }
    public override void OnTriggerExitState(MovableObjectStateManager Manager, Collider2D collision)
    {
        if (collision.gameObject == Manager.player)
        {
            Manager.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 255, 255);
            Manager.SmoothSwitchState(Manager.movableState);
        }
    }
    void CheckWalls()
    {
        wallDetect = new Vector2(0, 0);
        if (Physics2D.BoxCast(
            new Vector3(gridPosition.tilemap.origin.x + gridPosition.gridTilemapPosition.x, gridPosition.tilemap.origin.y + gridPosition.gridTilemapPosition.y, 0) + new Vector3(0.5f, 1.5f, 0)
            , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, 256))
        {
            wallDetect.y = 1;
        }
        else if (Physics2D.BoxCast(
            new Vector3(gridPosition.tilemap.origin.x + gridPosition.gridTilemapPosition.x, gridPosition.tilemap.origin.y + gridPosition.gridTilemapPosition.y, 0) + new Vector3(0.5f, -0.5f, 0)
            , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, 256))
        {
            wallDetect.y = -1;
        }
        if (Physics2D.BoxCast(
            new Vector3(gridPosition.tilemap.origin.x + gridPosition.gridTilemapPosition.x, gridPosition.tilemap.origin.y + gridPosition.gridTilemapPosition.y, 0) + new Vector3(1.5f, 0.5f, 0)
            , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, 256))
        {
            wallDetect.x = 1;
        }
        else if (Physics2D.BoxCast(
            new Vector3(gridPosition.tilemap.origin.x + gridPosition.gridTilemapPosition.x, gridPosition.tilemap.origin.y + gridPosition.gridTilemapPosition.y, 0) + new Vector3(-0.5f, 0.5f, 0)
            , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, 256))
        {
            wallDetect.x = -1;
        }
    }
}
