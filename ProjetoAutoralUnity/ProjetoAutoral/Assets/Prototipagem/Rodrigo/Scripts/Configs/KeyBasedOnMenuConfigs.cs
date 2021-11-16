using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBasedOnMenuConfigs : MonoBehaviour
{
    private MenuConfigs.Action action;
    void Start()
    {
        FirstTime();
    }
    private void FirstTime()
    {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            TextMeshProUGUI TMP = GetComponent<TextMeshProUGUI>();
            switch (TMP.text)
            {
                case "Menu":
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu].ToString();
                    action = MenuConfigs.Action.Menu;
                    break;
                case "Interaction":
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction].ToString();
                    action = MenuConfigs.Action.Interaction;
                    break;
                case "UseItem":
                    break;
                case "DropItem":
                    break;
                case "Radio":
                    break;
                case "TimeTravel":
                    break;
                case "PointFlashlight":
                    break;
            }
        }
        else if (GetComponent<TextMeshPro>() != null)
        {
            TextMeshPro TMP = GetComponent<TextMeshPro>();
        }
    }
    public void UpdateTextBasedOnInputKey()
    {

    }
}
