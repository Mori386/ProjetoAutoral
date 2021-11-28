using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    GameObject BaseMenu;
    
    GameObject OptionsTab;

    GameObject ControlsTab;
    GameObject controlsButtons;
    GameObject controlsOnWaitForInputMsg;

    private void Awake()
    {
        BaseMenu = transform.Find("BaseMenu").gameObject;
        OptionsTab = transform.Find("OptionsTab").gameObject;
        ControlsTab = transform.Find("ControlsTab").gameObject;
        controlsButtons = ControlsTab.transform.Find("ControlsButtons").gameObject;
        controlsOnWaitForInputMsg = ControlsTab.transform.Find("NextKeyMsg").gameObject;

        OptionsTab.transform.Find("VolumeSlider").GetComponent<Slider>().value = AudioListener.volume;

        menuButtonPressedConsequence = Resume;
    }
    public delegate void MenuButtonPressed();
    MenuButtonPressed menuButtonPressedConsequence;
    private void Update()
    {
        if (Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)MenuConfigs.Action.Menu]))
        {
            menuButtonPressedConsequence();
        }
    }
    public void Resume()
    {
        PMStateManager.Instance.SmoothSwitchState(PMStateManager.Instance.defaultState);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void Options()
    {
        BaseMenu.SetActive(false);
        OptionsTab.SetActive(true);
        menuButtonPressedConsequence = ExitOptions;
    }
    public void ExitOptions()
    {
        BaseMenu.SetActive(true);
        OptionsTab.SetActive(false);
        menuButtonPressedConsequence = Resume;
    }
    public void VolumeSlider(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    public void Controls()
    {
        BaseMenu.SetActive(false);
        ControlsTab.SetActive(true);
        menuButtonPressedConsequence = ExitControls;
    }
    public void OnChangeInput(int actionKeyID)
    {
        menuButtonPressedConsequence = null;
        controlsButtons.SetActive(false);
        controlsOnWaitForInputMsg.SetActive(true);
        StartCoroutine(WaitForNextControlsInput(actionKeyID));
    }
    private IEnumerator WaitForNextControlsInput(int actionKeyID)
    {
        KeyInput keyInput = gameObject.AddComponent<KeyInput>();
        while (true)
        {
            if (keyInput.keyPressed != KeyCode.None) break;
            yield return null;
        }
        for (int i = 0; i < MenuConfigs.Instance.InputKeys.Length; i++)
        {
            if (keyInput.keyPressed == MenuConfigs.Instance.InputKeys[i])
            {
                MenuConfigs.Instance.InputKeys[actionKeyID] = keyInput.keyPressed;
                MenuConfigs.Instance.InputKeys[i] = KeyCode.None;
                MenuConfigs.Instance.UpdateAllInputKeysInGame();
                controlsOnWaitForInputMsg.SetActive(false);
                controlsButtons.SetActive(true);
                menuButtonPressedConsequence = ExitControls;
                yield return new WaitForSeconds(0.1f);
                yield break;
            }
        }
        MenuConfigs.Instance.InputKeys[actionKeyID] = keyInput.keyPressed;
        MenuConfigs.Instance.UpdateAllInputKeysInGame();
        controlsOnWaitForInputMsg.SetActive(false);
        controlsButtons.SetActive(true);
        menuButtonPressedConsequence = ExitControls;
        yield return new WaitForSeconds(0.1f);
        yield break;
    }
    public void ExitControls()
    {
        ControlsTab.SetActive(false);
        BaseMenu.SetActive(true);
        menuButtonPressedConsequence = Resume;
    }
    public void Restart()
    {
        SceneManager.LoadScene(MenuConfigs.Instance.Puzzle+1);
        Time.timeScale = 1;
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
