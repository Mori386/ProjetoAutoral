using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFds : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    Rigidbody2D rb;
    Vector3 moveInputs;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        moveInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + moveInputs * moveSpeed * Time.fixedDeltaTime);
    }
}
