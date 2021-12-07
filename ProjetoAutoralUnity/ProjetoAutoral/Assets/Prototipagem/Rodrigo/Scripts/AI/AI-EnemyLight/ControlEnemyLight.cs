using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlEnemyLight : MonoBehaviour
{
    [SerializeField] bool future;
    private delegate void Sequence();
    private Sequence sequence;
    public static ControlEnemyLight InstanceControlPresent { get; private set; }
    public static ControlEnemyLight InstanceControlFuture { get; private set; }
    private void Start()
    {
        if (future)
        {
            InstanceControlFuture = this;
            sequence = OnFuture;
        }
        else
        {
            InstanceControlPresent = this;
            sequence = OnPresent;
        }
    }
    [System.NonSerialized] public bool playerTriggered;
    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.CompareTag("Player"))
        {
            playerTriggered = true;
            sequence();
            if (future)
            {
                GameObject Watch = GameObject.Find("Watch");
                Watch.GetComponent<Image>().sprite = Watch.GetComponent<WatchSprites>().DefaultClock;
                Watch.transform.GetChild(0).gameObject.SetActive(true);
                AIEnemyLight.Instance.gameObject.SetActive(true);
                if (AIEnemyLight.Instance.followRoute != null)
                {
                    AIEnemyLight.Instance.StopCoroutine(AIEnemyLight.Instance.followRoute);
                    AIEnemyLight.Instance.followRoute = AIEnemyLight.Instance.StartCoroutine(AIEnemyLight.Instance.FollowRoute());
                }
            }
            else
            {
                GameObject Watch = GameObject.Find("Watch");
                Watch.GetComponent<Image>().sprite = Watch.GetComponent<WatchSprites>().DefaultClock;
                Watch.transform.GetChild(0).gameObject.SetActive(true);
                AIEnemyLight.Instance.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTriggered = false;
            if (future)
            {
                GameObject Watch = GameObject.Find("Watch");
                Watch.GetComponent<Image>().sprite = Watch.GetComponent<WatchSprites>().DisabledClock;
                Watch.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    private void OnFuture()
    {
        PMStateManager.Instance.defaultState.isEnemyOn = true;
    }
    private void OnPresent()
    {
        PMStateManager.Instance.defaultState.isEnemyOn = false;
    }
}
