using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PMStateManager : MonoBehaviour
{
    public enum timePeriodList
    {
        Present, Future
    }
    public timePeriodList timePeriod;
    [System.NonSerialized] public Rigidbody2D rb; //Rigidbody2D do personagem 
    [System.NonSerialized] public Vector2 rawInputMove; //Variaveis baseadas no input de teclas do personagem(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
    public float moveSpeed; // velocidade de movimento do personagem 
    PMBaseState currentState;
    public PMDefaultState defaultState = new PMDefaultState();
    public PMControlOffState controlOffState = new PMControlOffState();
    [System.NonSerialized] public PlayerInventoryManager playerInventoryManager;
    [System.NonSerialized] public GameObject tilemapFuturo, tilemapPresente;
    public Vector2Int facingDirection;
    [System.NonSerialized] public Animator animator;

    [System.NonSerialized] public GameObject flashlight;
    [System.NonSerialized] public Rigidbody2D rbFlashlight;

    [System.NonSerialized] public AudioSource audioData;
    [System.NonSerialized] public PlayableDirector director;

    [System.NonSerialized] public bool endedAnimation;
    private void Awake()
    {
        facingDirection = new Vector2Int(0, 1);
        tilemapFuturo = GameObject.Find("TilemapFuturo");
        tilemapPresente = GameObject.Find("TilemapPresente");
        rb = GetComponent<Rigidbody2D>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        animator = GetComponent<Animator>();
        audioData = GetComponent<AudioSource>();
    }
    void Start()
    {
        flashlight = transform.Find("Flashlight").gameObject;
        rbFlashlight = flashlight.GetComponent<Rigidbody2D>();
        director = GameObject.Find("directorTime").GetComponent<PlayableDirector>();
        currentState = defaultState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }
    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    public void SwitchState(PMBaseState State)
    {
        currentState = State;
        currentState.EnterState(this);
    }
    public void SwitchStateBasedOnID(int IDState)
    {
        switch (IDState)
        {
            case 0:
                currentState = defaultState;
                currentState.EnterState(this);
                break;
            case 1:
                currentState = controlOffState;
                currentState.EnterState(this);
                break;
        }
    }
    public void SmoothSwitchState(PMBaseState State)
    {
        currentState = State;
    }
    public void TravelTime()
    {
        //Transição visual
        if (timePeriod == timePeriodList.Present)
        {
            transform.position += tilemapFuturo.transform.position - tilemapPresente.transform.position;
            timePeriod = timePeriodList.Future;
        }
        else
        {
            transform.position += tilemapPresente.transform.position - tilemapFuturo.transform.position;
            timePeriod = timePeriodList.Present;
        }
    }
    public Vector2 regulatorDirection(Vector2 rawDirection)
    {
        //A ideia desse comando é deixar com que independente a direção que o player está indo a velocidade dele seja fixa
        if (rawDirection.x == 0 && rawDirection.y == 0) return new Vector2(0, 0);
        else
        {
            Vector2 direction;
            direction.x = rawDirection.x / (Mathf.Abs(rawDirection.x) + Mathf.Abs(rawDirection.y));
            direction.y = rawDirection.y / (Mathf.Abs(rawDirection.x) + Mathf.Abs(rawDirection.y));
            return direction;
        }
    }
    public void AnimatorMethod(string method)
    {
        switch (method)
        {
            case "PushBackEnd":
            case "PushFrontEnd":
            case "PushRightEnd":
            case "PushLeftEnd":
                endedAnimation = true;
                break;
            case "PullRightEnd":
            case "PullLeftEnd":
            case "PullFrontEnd":
            case "PullBackEnd":
                animator.SetBool("PULLOBJECTMIDEND", false);
                break;
        }
    }
}
