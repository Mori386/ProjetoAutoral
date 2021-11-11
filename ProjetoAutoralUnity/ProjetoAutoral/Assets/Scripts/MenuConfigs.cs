using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConfigs : MonoBehaviour
{
    public static string getLanguage()
    {
        return "English";
    }
    public enum Action
    {
        Interaction,
        UseItem,
        DropItem,
        Radio,
        TimeTravel,
        PointFlashlight
    }
}
