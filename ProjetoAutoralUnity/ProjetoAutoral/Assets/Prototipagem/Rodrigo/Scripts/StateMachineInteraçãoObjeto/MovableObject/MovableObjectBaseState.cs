using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObjectBaseState
{
    public abstract void GizmosState(MovableObjectStateManager Manager);
    public abstract void EnterState(MovableObjectStateManager Manager);
    public abstract void UpdateState(MovableObjectStateManager Manager);
    public abstract void FixedUpdateState(MovableObjectStateManager Manager);
    public abstract void OnTriggerEnterState(MovableObjectStateManager Manager, Collider2D collision);
    public abstract void OnTriggerExitState(MovableObjectStateManager Manager, Collider2D collision);
}
