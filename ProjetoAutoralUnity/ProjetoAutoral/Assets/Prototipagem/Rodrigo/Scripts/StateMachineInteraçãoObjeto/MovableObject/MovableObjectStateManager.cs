using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectStateManager : MonoBehaviour
{
    [System.NonSerialized] public GameObject player; 
    public bool isMovable;
    [System.NonSerialized] public ObjectBase objectBase;
    [System.NonSerialized] public GridPosition gridPosition;
    MovableObjectBaseState currentState;
    public MOStateMovable movableState = new MOStateMovable();
    public MOStateMovableTriggered triggeredState = new MOStateMovableTriggered();
    public MOStateNotMovable notMovableState = new MOStateNotMovable();

    private void OnDrawGizmos()
    {

    }
    void Start()
    {
        objectBase = GetComponent<ObjectBase>();
        gridPosition = GetComponent<GridPosition>();
        player = GameObject.Find("Player");
        if(isMovable)
        {
            currentState = movableState;
        }
        else
        {
            currentState = notMovableState;
        }
        currentState.EnterState(this);
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }
    private void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnterState(this,collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentState.OnTriggerExitState(this,collision);
    }
    public void SwitchState(MovableObjectBaseState State)
    {
        currentState = State;
        currentState.EnterState(this);
    }
    public void SmoothSwitchState(MovableObjectBaseState State)
    {
        currentState = State;
    }
    public void moveOnFuture()
    {
        GameObject objectFuture = GetComponent<ObjectBase>().objectOtherTimeline;
        objectFuture.GetComponent<GridPosition>().gridTilemapPosition = GetComponent<GridPosition>().gridTilemapPosition;
        GridPosition gridFromObjectFuture = objectFuture.GetComponent<GridPosition>();
        objectFuture.transform.position = gridFromObjectFuture.tilemapCenter + new Vector3(gridFromObjectFuture.gridTilemapPosition.x + 0.5f, gridFromObjectFuture.gridTilemapPosition.y + 0.5f, 0);
    }
    public void ManualEnterStateCurrentState()
    {
        currentState.EnterState(this);
    }
}
