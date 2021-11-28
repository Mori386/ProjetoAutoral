using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMControlOffState : PMBaseState
{
    public override void EnterState(PMStateManager Manager)
    {

    }
    public override void UpdateState(PMStateManager Manager)
    {
        if (Input.GetKeyUp(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight]))
        {
            Manager.StopCoroutine(Manager.defaultState.focusOnCursorOn);
            Manager.defaultState.focusOnCursorOff = Manager.StartCoroutine(Manager.defaultState.FocusOnCursorOff(Manager));
            if (Manager.facingDirection.x != 0)
            {
                if (Manager.facingDirection.x == 1)
                {
                    Manager.flashlightLP.SetActive(false);
                    Manager.flashlightNLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(0.129f, -0.008f, 0);
                    Manager.defaultState.angle = -90;
                }
                else if (Manager.facingDirection.x == -1)
                {
                    Manager.flashlightLP.SetActive(false);
                    Manager.flashlightNLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(-0.139f, -0.01f, 0);
                    Manager.defaultState.angle = 90;
                }
            }
            else if (Manager.facingDirection.y != 0)
            {
                if (Manager.facingDirection.y == 1)
                {
                    Manager.flashlightLP.SetActive(false);
                    Manager.flashlightNLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(0.102f, -0.051f, 0);
                    Manager.defaultState.angle = 0;
                }
                else if (Manager.facingDirection.y == -1)
                {
                    Manager.flashlightNLP.SetActive(false);
                    Manager.flashlightLP.SetActive(true);
                    Manager.flashlightLP.transform.parent.localPosition = new Vector3(-0.0732f, -0.0342f, 0);
                    Manager.defaultState.angle = 180;
                }
            }
            Manager.flashlightLP.transform.parent.rotation = Quaternion.Euler(0, 0, Manager.defaultState.angle);
        }
    }
    public override void FixedUpdateState(PMStateManager Manager)
    {

    }
}
