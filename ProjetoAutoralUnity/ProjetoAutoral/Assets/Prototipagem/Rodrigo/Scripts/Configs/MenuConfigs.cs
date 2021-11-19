using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConfigs : MonoBehaviour
{
    public static MenuConfigs Instance { get; private set; }

    [System.NonSerialized] public List<KeyBasedOnMenuConfigs> inputKeysInGame = new List<KeyBasedOnMenuConfigs>();

    [System.NonSerialized] public KeyCode[] InputKeys = new KeyCode[14];

    private void Awake()
    {
        InputKeys[0] = KeyCode.Escape;
        InputKeys[1] = KeyCode.E;
        InputKeys[2] = KeyCode.Mouse0;
        InputKeys[3] = KeyCode.C;
        InputKeys[4] = KeyCode.F;
        InputKeys[5] = KeyCode.Q;
        InputKeys[6] = KeyCode.Mouse1;
        InputKeys[7] = KeyCode.W;
        InputKeys[8] = KeyCode.S;
        InputKeys[9] = KeyCode.A;
        InputKeys[10] = KeyCode.D;
        InputKeys[11] = KeyCode.Alpha1;
        InputKeys[12] = KeyCode.Alpha2;
        InputKeys[13] = KeyCode.Alpha3;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateAllInputKeysInGame()
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
        MoveUp = 7,
        MoveDown = 8,
        MoveLeft = 9,
        MoveRight = 10,
        InventorySlot1 = 11,
        InventorySlot2 = 12,
        InventorySlot3 = 13
    }
}
