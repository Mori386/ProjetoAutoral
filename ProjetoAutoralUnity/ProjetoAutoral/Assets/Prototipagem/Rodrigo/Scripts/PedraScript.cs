using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Boss")
        {
            AiBoss aiBoss = collision.GetComponent<AiBoss>();
            aiBoss.audioSource.PlayOneShot(aiBoss.wallRock);
            aiBoss.TakeDamage();
            Destroy(gameObject);
        }
    }
}
