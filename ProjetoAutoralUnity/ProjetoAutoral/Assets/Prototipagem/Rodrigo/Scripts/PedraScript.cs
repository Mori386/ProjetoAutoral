using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Boss")
        {
            AIBossV2 aiBoss = collision.GetComponent<AIBossV2>();
            AIBossV2.Instance.audioSourceOneShot.PlayOneShot(AIBossV2.Instance.wallRock, AIBossV2.Instance.wallRockVolume);
            aiBoss.TakeDamage();
            Destroy(gameObject);
        }
    }
}
