using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    public KeyCode keyPressed;
    private void OnGUI()
    {
        if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
        {
            Debug.Log(Event.current.keyCode);
            keyPressed = Event.current.keyCode;
            Destroy(this);
        }
    }
}
