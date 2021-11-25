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

    public bool playAudio;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void OnDrawGizmos()
    {
        //currentState.GizmosState(this);
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
        GridPosition gridFromObjectFuture = objectFuture.GetComponent<GridPosition>();
        gridFromObjectFuture.gridTilemapPosition = GetComponent<GridPosition>().gridTilemapPosition;
        objectFuture.transform.position = gridFromObjectFuture.tilemapPointZero + new Vector3(gridFromObjectFuture.gridTilemapPosition.x*gridFromObjectFuture.tilemap.cellSize.x, gridFromObjectFuture.gridTilemapPosition.y * gridFromObjectFuture.tilemap.cellSize.y);
    }
    public void ManualEnterStateCurrentState()
    {
        currentState.EnterState(this);
    }
}
