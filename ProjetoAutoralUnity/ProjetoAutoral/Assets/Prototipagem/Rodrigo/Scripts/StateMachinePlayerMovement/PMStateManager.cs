using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMStateManager : MonoBehaviour
{
    private enum timePeriodList
    {
        Present, Future
    }
    [SerializeField] private timePeriodList timePeriod;
    [System.NonSerialized] public Rigidbody2D rb; //Rigidbody2D do personagem 
    [System.NonSerialized] public Vector2 rawInputMove; //Variaveis baseadas no input de teclas do personagem(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
    public float moveSpeed; // velocidade de movimento do personagem 
    PMBaseState currentState;
    public PMDefaultState defaultState = new PMDefaultState();
    public PMControlOffState controlOffState = new PMControlOffState();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    public void SmoothSwitchState(PMBaseState State)
    {
        currentState = State;
    }
    public void TravelTime()
    {
        //Transição visual
        if (timePeriod == timePeriodList.Present)
        {
            transform.position += GameObject.Find("TilemapFuturo").transform.position - GameObject.Find("TilemapPresente").transform.position;
            timePeriod = timePeriodList.Future;
        }
        else
        {
            transform.position += GameObject.Find("TilemapPresente").transform.position - GameObject.Find("TilemapFuturo").transform.position;
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
}
