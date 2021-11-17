using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class controleDirectorTime : MonoBehaviour
{
    PlayableDirector director;
    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            director.Play();
        }
    }
}
