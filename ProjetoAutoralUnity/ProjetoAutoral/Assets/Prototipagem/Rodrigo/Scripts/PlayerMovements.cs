using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovements : MonoBehaviour
{
    Rigidbody2D rb; //Rigidbody2D do personagem 
    Vector2 rawInputMove; //Variaveis baseadas no input de teclas do personagem(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
    [SerializeField] float moveSpeed; // velocidade de movimento do personagem 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // pega o rigidbody dele mesmo, vulgo o proprio gameObject obs: não é preciso falar de quem pegar o rigidbody2D se caso for pegar dele mesmo
    }
    void Update()
    {
        rawInputMove.x = Input.GetAxisRaw("Horizontal");//adiciona variaveis baseadas no input de teclas(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
        rawInputMove.y = Input.GetAxisRaw("Vertical");// mesma coisa que o de cima so que para os botoes de mover na vertical
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + regulatorDirection(rawInputMove) * moveSpeed * Time.fixedDeltaTime);
    }
    private Vector2 regulatorDirection(Vector2 rawDirection)
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
