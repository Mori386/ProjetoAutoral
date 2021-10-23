using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsDataBase : MonoBehaviour
{
    public static void Interaction(string result)
    {
        switch (result)
        {
            //cases do Puzzle1
            case "SofaPresente":
                Debug.Log("InteraçãoSucesso");
                break;
        }
    }
}
