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
            case KeyCode.None: return "";
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
            case KeyCode.Minus: return "-";
            case KeyCode.Equals: return "=";
            case KeyCode.Backspace: return "←";


            case KeyCode.Quote: return "´";
            case KeyCode.LeftBracket: return "[";
            case KeyCode.RightBracket: return "]";
            case KeyCode.Tilde: return "~";
            case KeyCode.Semicolon: return ";";
            case KeyCode.Slash: return "/";
            case KeyCode.Backslash: return @"\";
            case KeyCode.Period: return ".";
            case KeyCode.Comma: return ",";
            case KeyCode.CapsLock: return "Caps";
            case KeyCode.LeftShift: return "LShift";
            case KeyCode.LeftControl: return "LCtrl";
            case KeyCode.LeftCommand: return "LCmd";
            case KeyCode.LeftAlt: return "LAlt";
            case KeyCode.Space: return "Space";
            case KeyCode.RightAlt: return "RAlt";
            case KeyCode.AltGr: return "GrAlt";
            case KeyCode.RightCommand: return "RCmd";


            case KeyCode.RightShift: return "RShift";
            case KeyCode.RightControl: return "RCtrl";
            case KeyCode.Return: return "Return";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.UpArrow: return "↑";


            case KeyCode.ScrollLock: return "ScrollLock";
            case KeyCode.Pause: return "Pause";
            case KeyCode.Insert: return "Insert";
            case KeyCode.Delete: return "Del";
            case KeyCode.End: return "End";
            case KeyCode.PageDown: return "PgDown";
            case KeyCode.PageUp: return "PgUp";
            case KeyCode.Home: return "Home";
            case KeyCode.Numlock: return "Numlock";
            case KeyCode.KeypadDivide: return "/";
            case KeyCode.KeypadMultiply: return "*";
            case KeyCode.KeypadMinus: return "-";
            case KeyCode.KeypadPlus: return "+";
            case KeyCode.KeypadPeriod: return ".";
            case KeyCode.KeypadEnter: return "Enter";

            case KeyCode.Keypad0: return "0";
            case KeyCode.Keypad1: return "1";
            case KeyCode.Keypad2: return "2";
            case KeyCode.Keypad3: return "3";
            case KeyCode.Keypad4: return "4";
            case KeyCode.Keypad5: return "5";
            case KeyCode.Keypad6: return "6";
            case KeyCode.Keypad7: return "7";
            case KeyCode.Keypad8: return "8";
            case KeyCode.Keypad9: return "9";
            default: return null;
        }
    }
}
