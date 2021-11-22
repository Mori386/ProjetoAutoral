using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CamaPuzzle3 : MonoBehaviour
{
    bool moved;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.Find("TeclaE").gameObject.SetActive(true);
            waitForInput = StartCoroutine(WaitForInput(collision.GetComponent<PMStateManager>()));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (waitingInput)
            {
                collision.transform.Find("TeclaE").gameObject.SetActive(false);
                StopCoroutine(waitForInput);
                waitForInput = null;
                waitingInput = false;
            }
        }

    }
    bool waitingInput;
    Coroutine waitForInput;
    private IEnumerator WaitForInput(PMStateManager playerMov)
    {
        waitingInput = true;
        while (true)
        {
            if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction])) break;
            yield return null;
        }
        waitingInput = false;
        playerMov.transform.Find("TeclaE").gameObject.SetActive(false);
        Vector3 startPos = transform.position;
        Vector3 deltaPosition;
        if(!moved)
        {
            deltaPosition = new Vector3(1, 0);
        }
        else
        {
            deltaPosition = new Vector3(-1, 0);
        }
        Tilemap tilemap = GameObject.Find("TilemapPresente").GetComponent<Tilemap>();
        deltaPosition = new Vector3(deltaPosition.x * tilemap.cellSize.x, deltaPosition.y * tilemap.cellSize.y, 0);
        playerMov.SmoothSwitchState(playerMov.controlOffState);
        if (!moved)
        {
            playerMov.animator.SetTrigger("PULLOBJECT");
        }
        else
        {
            playerMov.animator.SetTrigger("PUSHOBJECT");
            while (!playerMov.endedAnimation)
            {
                yield return null;
            }
            playerMov.endedAnimation = false;
        }
        Vector3 finalPositon = startPos + deltaPosition;
        while (
            new Vector3(Mathf.Round(transform.position.x * 100) / 100, Mathf.Round(transform.position.y * 100) / 100) != new Vector3(Mathf.Round(finalPositon.x * 100) / 100, Mathf.Round(finalPositon.y * 100) / 100)
            )
        {
            if (!moved) playerMov.transform.position += deltaPosition / 20;
            transform.position += deltaPosition / 20;
            yield return new WaitForSeconds(0.01f);
        }
        playerMov.animator.SetBool("PULLOBJECTMIDEND", true);
        GetComponent<GridPosition>().gridTilemapPosition += Vector2Int.RoundToInt(deltaPosition);
        GameObject objectFuture = GetComponent<ObjectBase>().objectOtherTimeline;
        GridPosition gridFromObjectFuture = objectFuture.GetComponent<GridPosition>();
        gridFromObjectFuture.gridTilemapPosition = GetComponent<GridPosition>().gridTilemapPosition;
        objectFuture.transform.position = gridFromObjectFuture.tilemapPointZero + new Vector3(gridFromObjectFuture.gridTilemapPosition.x * gridFromObjectFuture.tilemap.cellSize.x, gridFromObjectFuture.gridTilemapPosition.y * gridFromObjectFuture.tilemap.cellSize.y);
        playerMov.SmoothSwitchState(playerMov.defaultState);
        moved = !moved;
    }
}
