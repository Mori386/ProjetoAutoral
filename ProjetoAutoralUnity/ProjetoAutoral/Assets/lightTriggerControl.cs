using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightTriggerControl : MonoBehaviour
{
    [SerializeField]
    GameObject roundLight;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "lightTrigger")
        {
            roundLight.active = true;
        }
        else roundLight.active = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "lightTrigger")
        {
            roundLight.active = false;
        }
    }
}
