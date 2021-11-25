using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class IOStateManager : MonoBehaviour
{
    IOBaseState currentState;
    IODefaultState defaultState = new IODefaultState();
    public bool canSuccessiveInteract;
    public string textOnDisabledSuccessiveInteract;

    public bool singleTimeUse;
    public bool needItemToInteract;
    public Item[] itemsNeeded;
    public enum NeedToInteract
    {
        needAllItemsToInteract,needOneOfTheItemsToInteract
    }
    public NeedToInteract needToInteract;
    public bool destroyItemOnSuccessiveInteraction;
    public string textBoxOnSuccessiveInteraction;
    public string textBoxOnFailedInteraction;
    public TextMeshProUGUI textBox;
    public GameObject ui;

    public bool playAudio;
    public AudioSource audioSource;
    public AudioClip audioClip;

    public string onSuccessiveInteractionConsequence;
    private void Start()
    {
        currentState = defaultState;
        textBox = GameObject.Find("Canvas").transform.Find("TextBox").transform.Find("Text").GetComponent<TextMeshProUGUI>();
        ui = GameObject.Find("Canvas").transform.Find("UiInventory").gameObject;
        currentState.EnterState(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enabled)currentState.OnTriggerEnter2DState(this, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enabled) currentState.OnTriggerExit2DState(this, collision);
    }
}
