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
    [SerializeField]
    private bool inhibitCollisionLoad;
    private void Start()
    {
        if (loadSceneOnStart)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(inhibitCollisionLoad == false)
        {
            GetComponent<PlayableDirector>().Play();
        }
    }
   
    public void loadScene()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void restartScene()
    {
        SceneManager.LoadScene(MenuConfigs.Instance.Puzzle + 1);
    }
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
