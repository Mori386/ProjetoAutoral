using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemyLight : MonoBehaviour
{
    [SerializeField] bool future;
    private delegate void Sequence();
    private Sequence sequence;
    private void Start()
    {
        if (future) sequence = OnFuture;
        else sequence = OnPresent;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.CompareTag("Player"))
        {
            sequence();
        }
    }
    private void OnFuture()
    {
        if (!AIEnemyLight.Instance.gameObject.activeInHierarchy)
        {
            AIEnemyLight.Instance.gameObject.SetActive(true);
        }
        PMStateManager.Instance.defaultState.isEnemyOn = true;
    }
    private void OnPresent()
    {
        if (AIEnemyLight.Instance.gameObject.activeInHierarchy)
        {
            AIEnemyLight.Instance.gameObject.SetActive(false);
        }
        PMStateManager.Instance.defaultState.isEnemyOn = false;
    }
}
