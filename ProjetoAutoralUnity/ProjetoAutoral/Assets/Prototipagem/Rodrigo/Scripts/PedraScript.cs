using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Boss")
        {
            collision.GetComponent<AiBoss>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
