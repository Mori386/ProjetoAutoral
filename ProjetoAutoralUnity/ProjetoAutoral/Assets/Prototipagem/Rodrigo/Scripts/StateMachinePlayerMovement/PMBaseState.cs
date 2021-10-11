using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PMBaseState 
{
    public abstract void EnterState(PMStateManager Manager);
    public abstract void UpdateState(PMStateManager Manager);
    public abstract void FixedUpdateState(PMStateManager Manager);

}
