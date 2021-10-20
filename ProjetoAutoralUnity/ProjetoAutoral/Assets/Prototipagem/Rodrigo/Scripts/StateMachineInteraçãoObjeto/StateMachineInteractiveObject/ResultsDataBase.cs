using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsDataBase : MonoBehaviour
{
    public static void Interaction(string result)
    {
        switch (result)
        {
            case "Test":
                Debug.Log("InteractionDebugLog");
                break;
        }
    }
}
