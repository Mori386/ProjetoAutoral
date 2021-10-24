using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IOBaseState
{
    public abstract void EnterState(IOStateManager Manager);
    public abstract void OnTriggerEnter2DState(IOStateManager Manager, Collider2D collision);
    public abstract void OnTriggerExit2DState(IOStateManager Manager, Collider2D collision);

}
