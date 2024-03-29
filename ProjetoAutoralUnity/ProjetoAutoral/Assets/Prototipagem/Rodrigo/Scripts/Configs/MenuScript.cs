﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    GameObject baseMenu;

    GameObject optionsMenu;

    GameObject controlsMenu;
    GameObject controlsOnWaitForInputMsg;
    GameObject controlsButtons;

    GameObject CreditsTab;

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        optionsMenu = canvas.transform.Find("OptionsTab").gameObject;
        baseMenu = canvas.transform.Find("BaseMenu").gameObject;
        controlsMenu = canvas.transform.Find("ControlsTab").gameObject;
        controlsOnWaitForInputMsg = controlsMenu.transform.Find("NextKeyMsg").gameObject;
        controlsButtons = controlsMenu.transform.Find("ControlsButtons").gameObject;
        CreditsTab = canvas.transform.Find("CreditosTab").gameObject;

        optionsMenu.transform.Find("VolumeSlider").GetComponent<Slider>().value = AudioListener.volume;
    }
    private void Start()
    {
        if (MenuConfigs.Instance.highestPuzzle <= 0)
        {
            baseMenu.transform.Find("Continue").gameObject.GetComponent<CanvasGroup>().alpha = 0.33f;
            baseMenu.transform.Find("Continue").gameObject.GetComponent<Button>().enabled = false;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {
        if (MenuConfigs.Instance.highestPuzzle > 0)
        {
            SceneManager.LoadScene(MenuConfigs.Instance.highestPuzzle+1);
        }
    }
    public void Credits()
    {
        baseMenu.SetActive(false);
        CreditsTab.SetActive(true);
        waitForInput = StartCoroutine(WaitForInput(MenuConfigs.Action.Menu, ExitCredits));
    }
    public void ExitCredits()
    {
        CreditsTab.SetActive(false);
        baseMenu.SetActive(true);
    }
    public void Controls()
    {
        baseMenu.SetActive(false);
        controlsMenu.SetActive(true);
        waitForInput = StartCoroutine(WaitForInput(MenuConfigs.Action.Menu, ExitControls));
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void OnChangeInput(int actionKeyID)
    {
        controlsButtons.SetActive(false);
        controlsOnWaitForInputMsg.SetActive(true);
        StartCoroutine(WaitForNextControlsInput(actionKeyID));
    }
    private IEnumerator WaitForNextControlsInput(int actionKeyID)
    {
        StopCoroutine(waitForInput);
        waitForInput = null;
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
                yield return new WaitForSeconds(0.1f);
                waitForInput = StartCoroutine(WaitForInput(MenuConfigs.Action.Menu, ExitControls));
                yield break;
            }
        }
        MenuConfigs.Instance.InputKeys[actionKeyID] = keyInput.keyPressed;
        MenuConfigs.Instance.UpdateAllInputKeysInGame();
        controlsOnWaitForInputMsg.SetActive(false);
        controlsButtons.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        waitForInput = StartCoroutine(WaitForInput(MenuConfigs.Action.Menu, ExitControls));
        yield break;
    }
    public void ExitControls()
    {
        controlsMenu.SetActive(false);
        baseMenu.SetActive(true);
    }
    public void Options()
    {
        baseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        waitForInput = StartCoroutine(WaitForInput(MenuConfigs.Action.Menu, ExitOptions));
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
    private Coroutine waitForInput;
    public IEnumerator WaitForInput(MenuConfigs.Action actionKey, waitForInputConsequence consequence)
    {
        waitForInputConsequence WaitForInputConsequence;
        WaitForInputConsequence = consequence;
        while (!Input.GetKeyDown(MenuConfigs.Instance.InputKeys[(int)actionKey]))
        {
            yield return null;
        }
        WaitForInputConsequence();
    }
    public delegate void waitForInputConsequence();
}
