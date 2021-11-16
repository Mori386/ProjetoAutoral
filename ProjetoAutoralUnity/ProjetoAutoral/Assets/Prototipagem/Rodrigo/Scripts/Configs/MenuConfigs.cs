using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConfigs : MonoBehaviour
{
    public static MenuConfigs Instance { get; private set; }

    public List<KeyBasedOnMenuConfigs> inputKeysInGame = new List<KeyBasedOnMenuConfigs>();

    [System.NonSerialized] public KeyCode[] InputKeys = new KeyCode[7];

    private void Awake()
    {
        InputKeys[0] = KeyCode.Escape;
        InputKeys[1] = KeyCode.E;
        InputKeys[2] = KeyCode.Mouse0;
        InputKeys[3] = KeyCode.C;
        InputKeys[4] = KeyCode.F;
        InputKeys[5] = KeyCode.Q;
        InputKeys[6] = KeyCode.Mouse1;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateAllInputKeysInGame()
    {
        foreach (KeyBasedOnMenuConfigs keyBased in inputKeysInGame)
        {
            keyBased.UpdateTextBasedOnInputKey();
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        inputKeysInGame = new List<KeyBasedOnMenuConfigs>();
    }
    public static string getLanguage()
    {
        return "English";
    }
    public enum Action
    {
        Menu = 0,
        Interaction = 1,
        UseItem = 2,
        DropItem = 3,
        Radio = 4,
        TimeTravel = 5,
        PointFlashlight = 6,
    }
}
