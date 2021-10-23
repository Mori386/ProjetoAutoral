using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOStateMovableTriggered : MovableObjectBaseState
{
    bool isMoving;
    public Vector2 wallDetect;
    GridPosition gridPosition;
    int layer = 768;
    public override void GizmosState(MovableObjectStateManager Manager)
    {
        SpriteRenderer spriteRenderer = Manager.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite.bounds.size.x != 0)
        {
            int i = 1;
            Vector3 a = new Vector3(Manager.transform.position.x + spriteRenderer.sprite.bounds.size.x / 2 - spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.pixelsPerUnit, Manager.transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 - spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
            Vector3 centerPivot = new Vector3(Manager.transform.position.x + spriteRenderer.sprite.bounds.size.x / 2 - spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.pixelsPerUnit, Manager.transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 - spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
            Vector2 objectSize = new Vector2(spriteRenderer.sprite.bounds.size.x / 0.64f, spriteRenderer.sprite.bounds.size.y / 0.64f);
            Gizmos.DrawCube(feetGridPos + new Vector3(gridPosition.tilemap.cellSize.x * (objectSize.x + 1), gridPosition.tilemap.cellSize.y * i), gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0));
        }
    }
    public override void EnterState(MovableObjectStateManager Manager)
    {
        gridPosition = Manager.GetComponent<GridPosition>();
        CheckWalls(Manager);
    }
    public override void UpdateState(MovableObjectStateManager Manager)
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Z))
        {
            SpriteRenderer spriteRenderer = Manager.GetComponent<SpriteRenderer>();
            Vector3 centerPivot = new Vector3(Manager.transform.position.x + spriteRenderer.sprite.bounds.size.x / 2 - spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.pixelsPerUnit, Manager.transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 - spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
            Vector2 playerPos = new Vector2(Manager.player.transform.position.x, Manager.player.transform.position.y);
            float angle = Mathf.Atan2(playerPos.y - centerPivot.y, playerPos.x - centerPivot.x) * Mathf.Rad2Deg;
            if (angle > 45 && angle <= 135)
            {
                if (wallDetect.y == -1)
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(0, 1, 0), new Vector2Int(0, 1), true));
                }
                else
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(0, -1, 0), new Vector2Int(0, -1), false));
                }
            }
            else if (angle > 135 && angle <= 180 || angle < -135 && angle >= -180)
            {
                if (wallDetect.x == 1)
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(-1, 0, 0), new Vector2Int(-1, 0), true));
                }
                else
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(1, 0, 0), new Vector2Int(1, 0), false));
                }
            }
            else if (angle <= 45 && angle > 0 || angle <= 0 && angle > -45)
            {
                if (wallDetect.x == -1)
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(1, 0, 0), new Vector2Int(1, 0), true));
                }
                else
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(-1, 0, 0), new Vector2Int(-1, 0), false));
                }
            }
            else if (angle <= -45 && angle > -135)
            {
                if (wallDetect.y == 1)
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(0, -1, 0), new Vector2Int(0, -1), true));
                }
                else
                {
                    if (!isMoving) Manager.StartCoroutine(MoveObjectAnimation(Manager, new Vector3(0, 1, 0), new Vector2Int(0, 1), false));
                }
            }
        }
    }
    IEnumerator MoveObjectAnimation(MovableObjectStateManager Manager, Vector3 deltaPosition, Vector2Int deltaGridPosition, bool MovePlayerTogether)
    {
        PMStateManager playerMov = Manager.player.GetComponent<PMStateManager>();
        Vector3 startPos = Manager.transform.position;
        deltaPosition = new Vector3(deltaPosition.x * Manager.gridPosition.tilemap.cellSize.x, deltaPosition.y * Manager.gridPosition.tilemap.cellSize.y, 0);
        isMoving = true;
        if (MovePlayerTogether) playerMov.SmoothSwitchState(playerMov.controlOffState);
        Vector3 finalPositon = startPos + deltaPosition;
        while (
            new Vector3(Mathf.Round(Manager.transform.position.x * 100) / 100, Mathf.Round(Manager.transform.position.y * 100) / 100) != new Vector3(Mathf.Round(finalPositon.x * 100) / 100, Mathf.Round(finalPositon.y * 100) / 100)
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
        CheckWalls(Manager);
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
    Vector3 feetGridPos;
    void CheckWalls(MovableObjectStateManager Manager)
    {
        wallDetect = new Vector2(0, 0);
        SpriteRenderer spriteRenderer = Manager.GetComponent<SpriteRenderer>();
        Vector3 centerPivot = new Vector3(Manager.transform.position.x + spriteRenderer.sprite.bounds.size.x / 2 - spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.pixelsPerUnit, Manager.transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 - spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
        Vector2 objectSize = new Vector2(spriteRenderer.sprite.bounds.size.x / 0.64f, spriteRenderer.sprite.bounds.size.y / 0.64f);
        feetGridPos = centerPivot - new Vector3(0, spriteRenderer.sprite.bounds.size.y / 2, 0);
        feetGridPos -= gridPosition.tilemap.cellSize * 0.5f + new Vector3(gridPosition.tilemap.cellSize.x * 0.5f, 0);
        RaycastHit2D[] raycastHit2D;
        for (int i = 1; i <= objectSize.y; i++)
        {
            if (wallDetect.x == 0)
            {
                raycastHit2D = Physics2D.BoxCastAll(
                feetGridPos + new Vector3(gridPosition.tilemap.cellSize.x * (objectSize.x + 1), gridPosition.tilemap.cellSize.y * i)
                , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, layer);
                foreach (RaycastHit2D hit2D in raycastHit2D)
                {
                    if (hit2D.collider.gameObject != Manager.gameObject)
                    {
                        if (!hit2D.collider.isTrigger)
                        {
                            Debug.Log(hit2D.collider.gameObject);
                            wallDetect.x = 1;
                        }
                    }
                }
                raycastHit2D = Physics2D.BoxCastAll(
                    feetGridPos + new Vector3(0, gridPosition.tilemap.cellSize.y * i)
                    , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, layer);
                foreach (RaycastHit2D hit2D in raycastHit2D)
                {
                    if (hit2D.collider.gameObject != Manager.gameObject)
                    {
                        if (!hit2D.collider.isTrigger)
                        {
                            //Debug.Log(hit2D.collider.gameObject);
                            wallDetect.x = -1;
                        }
                    }

                }
            }
        }
        for (int i = 1; i <= objectSize.x; i++)
        {
            if (wallDetect.y == 0)
            {
                raycastHit2D = Physics2D.BoxCastAll(
            feetGridPos + new Vector3(gridPosition.tilemap.cellSize.x * i, gridPosition.tilemap.cellSize.y * (objectSize.y + 1))
            , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, layer);
                foreach (RaycastHit2D hit2D in raycastHit2D)
                {
                    if (hit2D.collider.gameObject != Manager.gameObject)
                    {
                        if (!hit2D.collider.isTrigger)
                        {
                            //Debug.Log(hit2D.collider.gameObject);
                            wallDetect.y = 1;
                        }
                    }
                }
                raycastHit2D = Physics2D.BoxCastAll(
                feetGridPos + new Vector3(gridPosition.tilemap.cellSize.x * i, 0)
                , gridPosition.tilemap.cellSize - new Vector3(0.2f, 0.2f, 0), 0, new Vector2(0, 0), Mathf.Infinity, layer);
                foreach (RaycastHit2D hit2D in raycastHit2D)
                {
                    if (hit2D.collider.gameObject != Manager.gameObject)
                    {
                        if (!hit2D.collider.isTrigger)
                        {
                            //Debug.Log(hit2D.collider.gameObject);
                            wallDetect.y = -1;
                        }
                    }
                }
            }
        }
        Debug.Log(objectSize);
        Debug.Log(wallDetect);
    }
}
