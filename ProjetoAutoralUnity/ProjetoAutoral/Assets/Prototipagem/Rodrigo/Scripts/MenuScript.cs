using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    GameObject baseMenu;
    GameObject optionsMenu;
    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        optionsMenu = canvas.transform.Find("OptionsTab").gameObject;
        baseMenu = canvas.transform.Find("BaseMenu").gameObject;
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Jogar");
    }
    public void Credits()
    {
        Debug.Log("Creditos");
    }
    public void Controls()
    {
        Debug.Log("Controles");
    }
    public void Options()
    {
        Debug.Log("Opções");
        baseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        StartCoroutine(waitForInput(KeyCode.Escape));
    }
    public void VolumeSlider(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    public void ExitOptions()
    {
        baseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    public IEnumerator waitForInput(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
        {
            yield return null;
        }
        ExitOptions();
    }
}
