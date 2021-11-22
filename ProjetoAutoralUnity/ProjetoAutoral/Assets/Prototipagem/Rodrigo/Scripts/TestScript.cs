using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private delegate void DelegateTest();
    DelegateTest delegateTest;
    private void Start()
    {
        delegateTest += Action1;
        delegateTest += Action2;
        delegateTest += Action3;
        delegateTest += Action4;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            delegateTest();
        }
    }
    private void Action1()
    {
        Debug.Log("1");

    }
    private void Action2()
    {
        Debug.Log("2");
    }private void Action3()
    {
        Debug.Log("3");
    }
    private void Action4()
    {
        Debug.Log("4");
    }
}
