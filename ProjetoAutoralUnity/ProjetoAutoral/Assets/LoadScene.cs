using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private bool loadSceneOnStart;
    [SerializeField]
    private int nextScene;
    private void Start()
    {
        if (loadSceneOnStart)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<PlayableDirector>().Play();
    }
    public void loadScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
