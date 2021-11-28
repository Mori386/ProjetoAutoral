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
    private int menuConfigsPuzzle;
    private void Start()
    {
        if (loadSceneOnStart)
        {
            SceneManager.LoadScene(nextScene);
        }
        menuConfigsPuzzle = MenuConfigs.Instance.Puzzle;
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
    public void restartScene()
    {
        SceneManager.LoadScene(menuConfigsPuzzle + 1);
    }
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
