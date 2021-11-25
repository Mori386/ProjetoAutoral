using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PMStateManager : MonoBehaviour
{
    public static PMStateManager Instance { get; private set; }
    public enum timePeriodList
    {
        Present, Future
    }
    public timePeriodList timePeriod;
    [System.NonSerialized] public Rigidbody2D rb; //Rigidbody2D do personagem 
    [System.NonSerialized] public Vector2 rawInputMove; //Variaveis baseadas no input de teclas do personagem(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
    public float moveSpeed; // velocidade de movimento do personagem 

    public int hp;
    [System.NonSerialized] public Slider hpSlider;

    PMBaseState currentState;
    public PMDefaultState defaultState = new PMDefaultState();
    public PMControlOffState controlOffState = new PMControlOffState();
    [System.NonSerialized] public PlayerInventoryManager playerInventoryManager;
    [System.NonSerialized] public GameObject tilemapFuturo, tilemapPresente;
    public Vector2Int facingDirection;
    [System.NonSerialized] public Animator animator;

    [System.NonSerialized] public GameObject flashlightNLP;
    [System.NonSerialized] public GameObject flashlightLP;

    [Header("Interactions")]
    public bool canTimeTravel;


    [Header("Audio Source")]
    public AudioSource audioSourceWalk;
    public AudioSource audioSourceOneShot;


    [Header("FLashlight Audio Clip")]
    public AudioClip audioClipFlashlightOn;
    public AudioClip audioClipFlashlightOff;

    [Header("Player Take Damage Audio Clip")]
    public AudioClip audioClipPlayerTakeDamage;

    [Header("Walking Audio Clip")]
    public AudioClip audioClipStep1;
    public AudioClip audioClipStep2;
    public AudioClip audioClipStep3;
    public AudioClip audioClipStep4;
    public AudioClip audioClipStep5;
    public AudioClip audioClipStep6;
    public AudioClip audioClipStep7;
    public AudioClip audioClipStep8;
    public AudioClip audioClipStep9;
    public AudioClip audioClipStep10;

    [System.NonSerialized] public PlayableDirector director;
    [System.NonSerialized] public bool endedAnimation;


    //Lights
    [System.NonSerialized] public GameObject lightPresent, lightFuture;
    private void Awake()
    {
        Instance = this;
        facingDirection = new Vector2Int(0, 1);
        tilemapFuturo = GameObject.Find("TilemapFuturo");
        tilemapPresente = GameObject.Find("TilemapPresente");
        rb = GetComponent<Rigidbody2D>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        animator = GetComponent<Animator>();
        hpSlider = GameObject.Find("HealthSlidderControler").GetComponent<Slider>();
        hpSlider.maxValue = hp;
        hpSlider.value = hpSlider.maxValue;
        if (canTimeTravel) defaultState.interactionCheck += defaultState.TimeTravelCheck;
    }
    void Start()
    {
        flashlightNLP = transform.Find("Flashlights").transform.Find("FlashlightNotLightPlayer").gameObject;
        flashlightLP = transform.Find("Flashlights").transform.Find("FlashlightLightPlayer").gameObject;
        if (GameObject.Find("directorTime") != null) director = GameObject.Find("directorTime").GetComponent<PlayableDirector>();
        if (canTimeTravel)
        {
            Transform Light = GameObject.Find("controleLight").transform;
            lightPresent = Light.Find("luzP").gameObject;
            lightFuture = Light.Find("luzFGlobal").gameObject;
            SwitchLight();
        }
        playerInventoryManager.inventory.AddItem(new Item { itemType = Item.ItemType.Flashlight, amount = 1, active = false });
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
            transform.Find("RoundLight").gameObject.SetActive(true);
        }
        else
        {
            transform.position += tilemapPresente.transform.position - tilemapFuturo.transform.position;
            timePeriod = timePeriodList.Present;
            transform.Find("RoundLight").gameObject.SetActive(false);
        }
        SwitchLight();
    }
    public void SwitchLight()
    {
        switch (timePeriod)
        {
            case timePeriodList.Present:
                lightFuture.SetActive(false);
                lightPresent.SetActive(true);
                break;
            case timePeriodList.Future:
                lightPresent.SetActive(false);
                lightFuture.SetActive(true);
                break;
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
    public IEnumerator InvincibilityTimer(float time)
    {
        OnInvincibilityTimer = true;
        float timer = 0;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        while (timer < time)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.7f+(timer/time*0.3f));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255);
        OnInvincibilityTimer = false;
    }
    [System.NonSerialized] bool OnInvincibilityTimer;
    public void TakeDamage(int amountDamage)
    {
        if (!OnInvincibilityTimer)
        {
            hp -= amountDamage;
            if (hp <= 0)
            {
                //morre 
                //por hora
                hp = Mathf.RoundToInt(hpSlider.maxValue);
            }
            UpdateHealthBar();
            audioSourceOneShot.PlayOneShot(audioClipPlayerTakeDamage);
            StartCoroutine(InvincibilityTimer(2));
        }
    }
    public void UpdateHealthBar()
    {
        if (hp >= 0) hpSlider.value = hp;
        else hpSlider.value = 0;
    }
}
