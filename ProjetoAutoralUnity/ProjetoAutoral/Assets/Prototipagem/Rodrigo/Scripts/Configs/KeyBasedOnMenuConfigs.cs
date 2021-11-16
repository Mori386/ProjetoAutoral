using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBasedOnMenuConfigs : MonoBehaviour
{
    [SerializeField] private MenuConfigs.Action action;
    void Start()
    {
        MenuConfigs.Instance.inputKeysInGame.Add(this);
        UpdateTextBasedOnInputKey();
    }
    public void UpdateTextBasedOnInputKey()
    {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            TextMeshProUGUI TMP = GetComponent<TextMeshProUGUI>();
            switch (action)
            {
                case MenuConfigs.Action.Menu:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu].ToString();
                    break;
                case MenuConfigs.Action.Interaction:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction].ToString();
                    break;
                case MenuConfigs.Action.UseItem:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.UseItem].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.UseItem].ToString();
                    break;
                case MenuConfigs.Action.DropItem:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.DropItem].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.DropItem].ToString();
                    break;
                case MenuConfigs.Action.Radio:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Radio].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Radio].ToString();
                    break;
                case MenuConfigs.Action.TimeTravel:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.TimeTravel].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.TimeTravel].ToString();
                    break;
                case MenuConfigs.Action.PointFlashlight:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight].ToString();
                    break;
            }
        }
        else if (GetComponent<TextMeshPro>() != null)
        {
            TextMeshPro TMP = GetComponent<TextMeshPro>();
            switch (action)
            {
                case MenuConfigs.Action.Menu:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu].ToString();
                    break;
                case MenuConfigs.Action.Interaction:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Interaction].ToString();
                    break;
                case MenuConfigs.Action.UseItem:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.UseItem].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.UseItem].ToString();
                    break;
                case MenuConfigs.Action.DropItem:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.DropItem].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.DropItem].ToString();
                    break;
                case MenuConfigs.Action.Radio:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Radio].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Radio].ToString();
                    break;
                case MenuConfigs.Action.TimeTravel:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.TimeTravel].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.TimeTravel].ToString();
                    break;
                case MenuConfigs.Action.PointFlashlight:
                    if (MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight].ToString() == "Escape") TMP.text = "Esc";
                    else TMP.text = MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.PointFlashlight].ToString();
                    break;
            }
        }
    }
}
