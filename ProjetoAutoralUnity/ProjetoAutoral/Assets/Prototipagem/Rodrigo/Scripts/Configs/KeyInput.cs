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
        else if (Event.current.isMouse)
        {
            switch (Event.current.button)
            {
                case 0:
                    keyPressed = KeyCode.Mouse0;
                    break;
                case 1:
                    keyPressed = KeyCode.Mouse1;
                    break;
                case 2:
                    keyPressed = KeyCode.Mouse2;
                    break;
                case 3:
                    keyPressed = KeyCode.Mouse3;
                    break;
                case 4:
                    keyPressed = KeyCode.Mouse4;
                    break;
                case 5:
                    keyPressed = KeyCode.Mouse5;
                    break;
                case 6:
                    keyPressed = KeyCode.Mouse6;
                    break;

            }
        }
    }
}
