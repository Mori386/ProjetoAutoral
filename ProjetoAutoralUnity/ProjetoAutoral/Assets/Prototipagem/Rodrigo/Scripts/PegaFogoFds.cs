using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegaFogoFds : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AiBoss aiBoss = GameObject.Find("Boss").GetComponent<AiBoss>();
            aiBoss.target = gameObject;
            aiBoss.StopCoroutine(aiBoss.followRoute);
            aiBoss.followRoute = null;
            aiBoss.enragedChargeTime = StartCoroutine(aiBoss.EnragedChargeTime(2));
        }
    }
}
