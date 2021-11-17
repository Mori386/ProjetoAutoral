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
            if (ChangeIfNameIsWrong(action) != null) TMP.text = ChangeIfNameIsWrong(action);
            else TMP.text = MenuConfigs.Instance.InputKeys[(int)action].ToString();
        }
        else if (GetComponent<TextMeshPro>() != null)
        {
            TextMeshPro TMP = GetComponent<TextMeshPro>();
            if (ChangeIfNameIsWrong(action) != null) TMP.text = ChangeIfNameIsWrong(action);
            else TMP.text = MenuConfigs.Instance.InputKeys[(int)action].ToString();
        }
    }
    private string ChangeIfNameIsWrong(MenuConfigs.Action action)
    {
        switch (MenuConfigs.Instance.InputKeys[(int)action])
        {
            case KeyCode.Escape: return "Esc";
            case KeyCode.BackQuote: return "'";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";
            case KeyCode.Alpha0: return "0";
            default: return null;
        }
    }
}
