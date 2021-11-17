using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu])) transform.Find("LoadNextScene").gameObject.SetActive(true);
    }
}
