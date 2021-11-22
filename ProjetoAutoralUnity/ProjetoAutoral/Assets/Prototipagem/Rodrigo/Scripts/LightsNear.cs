using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightsNear : MonoBehaviour
{
    public static Transform player { get; private set; }
    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<LightsNear>();
        }
        if (GetComponent<Light2D>() == null || name == "luzPGlobal" || name == "luzFGlobal")
        {
            Destroy(this);
        }
        switchLightOn = StartCoroutine(SwitchLightOn(GetComponent<Light2D>()));
    }
    Coroutine switchLightOn;
    private IEnumerator SwitchLightOn(Light2D light2D)
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (Vector2.Distance(player.position, transform.position) > 6)
            {
                light2D.enabled = false;
                switchLightOn = null;
                switchLightOff = StartCoroutine(SwitchLightOff(light2D));
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    Coroutine switchLightOff;
    private IEnumerator SwitchLightOff(Light2D light2D)
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (Vector2.Distance(player.position, transform.position) < 6)
            {
                light2D.enabled = true;
                switchLightOff = null;
                switchLightOn = StartCoroutine(SwitchLightOn(light2D));
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
