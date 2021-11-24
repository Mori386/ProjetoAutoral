using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenObjectDestroyedItemGoDown : MonoBehaviour
{
    public delegate void Consequence();
    public Consequence consequence;
    private void Start()
    {
        consequence = MatchesGoDown;
    }
    private void MatchesGoDown()
    {
        GameObject.Find("Box of Matches").transform.position += new Vector3(0, -0.39438f, 0);
    }
}
