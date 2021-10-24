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
    [System.NonSerialized] public GameObject objectOtherTimeline;
    private void Start()
    {
        StartCoroutine(waitForSetup());
    }
    private IEnumerator waitForSetup()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (GetComponent<GridPosition>() != null)
        {
            if (timePeriod == timePeriodList.Present)
            {
                SeachForObjectOtherTimeline(GameObject.Find("ObjectsFuture"));
            }
            else
            {
                SeachForObjectOtherTimeline(GameObject.Find("ObjectsPresent"));
            }
        }
    }
    private bool finishedSearching=false;
    public void SeachForObjectOtherTimeline(GameObject List)
    {
        for (int i = 0; i < List.transform.childCount; i++)
        {
            if (List.transform.GetChild(i).GetComponent<GridPosition>() == null)
            {
                for (int c = 0; c < List.transform.GetChild(i).childCount; c++)
                {
                    if (List.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>() != null)
                    {
                        //Debug.Log(List.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>().gridTilemapPosition);
                        if (List.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>().gridTilemapPosition == GetComponent<GridPosition>().gridTilemapPosition)
                        {
                            objectOtherTimeline = List.transform.GetChild(i).GetChild(c).gameObject;
                            break;
                        }
                    }
                }
                if (finishedSearching) break;
            }
            else
            {
                if (List.transform.GetChild(i).GetComponent<GridPosition>().gridTilemapPosition == GetComponent<GridPosition>().gridTilemapPosition)
                {
                    objectOtherTimeline = List.transform.GetChild(i).gameObject;
                    break;
                }
            }
        }
    }
}
