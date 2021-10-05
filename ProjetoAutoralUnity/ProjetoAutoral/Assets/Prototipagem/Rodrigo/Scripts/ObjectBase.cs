using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public enum timePeriodList
    {
        Present, Future
    }
    [SerializeField] public timePeriodList timePeriod;
    [SerializeField] public GameObject objectOtherTimeline;
    private void Start()
    {
        StartCoroutine(WaitForStart());
    }
    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.5f);
        if (timePeriod == timePeriodList.Present)
        {
            SeachForObjectOtherTimeline(GameObject.Find("ObjectsFuture"));
        }
        else
        {
            SeachForObjectOtherTimeline(GameObject.Find("ObjectsPresent"));
        }
    }
    public void SeachForObjectOtherTimeline(GameObject List)
    {
        for (int i = 0; i <List.transform.childCount; i++)
        {
            if (List.transform.GetChild(i).GetComponent<GridPosition>().gridTilemapPosition == GetComponent<GridPosition>().gridTilemapPosition)
            {
                objectOtherTimeline = List.transform.GetChild(i).gameObject;
                Debug.Log(objectOtherTimeline);
                break;
            }
            else Debug.Log("Falhou");
        }
    }
}
