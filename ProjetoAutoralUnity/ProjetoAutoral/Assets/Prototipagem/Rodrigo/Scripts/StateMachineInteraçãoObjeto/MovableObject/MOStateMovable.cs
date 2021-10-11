using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOStateMovable : MovableObjectBaseState
{
    public override void GizmosState(MovableObjectStateManager Manager)
    {

    }
    public override void EnterState(MovableObjectStateManager Manager)
    {
        foreach(BoxCollider2D box in Manager.GetComponents<BoxCollider2D>())
        {
            if(box.isTrigger==true) box.enabled = true;
        }
    }
    public override void UpdateState(MovableObjectStateManager Manager)
    {

    }
    public override void FixedUpdateState(MovableObjectStateManager Manager)
    {

    }
    public override void OnTriggerEnterState(MovableObjectStateManager Manager, Collider2D collision)
    {
        if(collision.gameObject==Manager.player)
        {
            Manager.GetComponent<SpriteRenderer>().color = Color.yellow;
            Manager.SwitchState(Manager.triggeredState);
        }
    }
    public override void OnTriggerExitState(MovableObjectStateManager Manager, Collider2D collision)
    {

    }
}
