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
    private GridPosition gridPosition;
    private void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
    }
    private void Start()
    {
        if (gridPosition != null) StartCoroutine(waitForSetup());
    }
    private IEnumerator waitForSetup()
    {
        while (ObjectsList.Instance == null)
        {
            yield return null;
        }
        while (!ObjectsList.Instance.finishedSearchingPresent || !ObjectsList.Instance.finishedSearchingFuture)
        {
            yield return null;
        }
        switch (timePeriod)
        {
            case timePeriodList.Present:
                foreach (GridPosition gp in ObjectsList.Instance.gridPositionObjectFutureList)
                {
                    if (gp != null)
                    {
                        if (gp.gridTilemapPosition == gridPosition.gridTilemapPosition)
                        {
                            objectOtherTimeline = gp.gameObject;
                        }
                    }
                }
                break;
            case timePeriodList.Future:
                foreach (GridPosition gp in ObjectsList.Instance.gridPositionObjectPresentList)
                {
                    if (gp != null)
                    {
                        if (gp.gridTilemapPosition == gridPosition.gridTilemapPosition)
                        {
                            objectOtherTimeline = gp.gameObject;
                        }
                    }
                }
                break;
        }
        //Debug.Log(objectOtherTimeline);
    }
    private bool finishedSearching = false;
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
