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
    public void SeachForObjectOtherTimeline(GameObject List)
    {
        for (int i = 0; i < List.transform.childCount; i++)
        {
            if (List.transform.GetChild(i).GetComponent<GridPosition>() == null)
            {
                if (List.transform.GetChild(i).childCount > 0)
                {
                    for (int c = 0; c < List.transform.GetChild(i).childCount; c++)
                    {
                        if (List.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>() != null)
                        {
                            if (List.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>().gridTilemapPosition == GetComponent<GridPosition>().gridTilemapPosition)
                            {
                                objectOtherTimeline = List.transform.GetChild(i).GetChild(c).gameObject;
                                break;
                            }
                        }
                    }
                }
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
