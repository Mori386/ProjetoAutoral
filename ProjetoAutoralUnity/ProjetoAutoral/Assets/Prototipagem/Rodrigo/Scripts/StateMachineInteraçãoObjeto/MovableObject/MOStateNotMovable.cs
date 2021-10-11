using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOStateNotMovable : MovableObjectBaseState
{
    public override void GizmosState(MovableObjectStateManager Manager)
    {

    }
    public override void EnterState(MovableObjectStateManager Manager)
    {
        foreach (BoxCollider2D box in Manager.GetComponents<BoxCollider2D>())
        {
            if (box.isTrigger == true) box.enabled = false;
        }
    }
    public override void UpdateState(MovableObjectStateManager Manager)
    {

    }
    public override void FixedUpdateState(MovableObjectStateManager Manager)
    {

    }
    public override void OnTriggerEnterState(MovableObjectStateManager Manager,Collider2D collision)
    {
        Debug.Log("NotMovTriggerEnter");
    }
    public override void OnTriggerExitState(MovableObjectStateManager Manager, Collider2D collision)
    {
        Debug.Log("NotMovTriggerExit");
    }
}
