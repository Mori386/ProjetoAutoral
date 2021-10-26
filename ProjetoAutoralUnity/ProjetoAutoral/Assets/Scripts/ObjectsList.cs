using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsList : MonoBehaviour
{
    [System.NonSerialized] public GridPosition[] gridPositionObjectPresentList;
    [System.NonSerialized] public GridPosition[] gridPositionObjectFutureList;
    [System.NonSerialized] public bool finishedSearchingPresent;
    [System.NonSerialized] public bool finishedSearchingFuture;
    public static ObjectsList Instance { get; private set; }
    void Start()
    {
        Instance = this;
        GameObject objectsPresent = GameObject.Find("ObjectsPresent");
        GameObject objectsFuture = GameObject.Find("ObjectsFuture");
        gridPositionObjectPresentList = new GridPosition[SearchChildsGridPositionOnObjectParent(objectsPresent)];
        gridPositionObjectFutureList = new GridPosition[SearchChildsGridPositionOnObjectParent(objectsFuture)];
        StartCoroutine(SearchChildsGridPositionOnObjectParent(gridPositionObjectPresentList, objectsPresent, "finishedSearchingPresent"));
        StartCoroutine(SearchChildsGridPositionOnObjectParent(gridPositionObjectFutureList, objectsFuture, "finishedSearchingFuture"));
    }

    private int SearchChildsGridPositionOnObjectParent(GameObject parent)
    {
        int gameobjectsFound = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).GetComponent<GridPosition>() != null)
            {
                for (int c = 0; c < parent.transform.GetChild(i).childCount; c++)
                {
                    if (parent.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>() != null)
                    {
                        gameobjectsFound++;
                    }
                }
            }
            else
            {
                gameobjectsFound++;
            }
        }
        return gameobjectsFound;
    }
    private IEnumerator SearchChildsGridPositionOnObjectParent(GridPosition[] List, GameObject parent, string finishedSearching)
    {
        int orderInList = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).GetComponent<GridPosition>() == null)
            {
                if (parent.transform.GetChild(i).childCount > 0)
                {
                    for (int c = 0; c < parent.transform.GetChild(i).childCount; c++)
                    {
                        if (parent.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>() != null)
                        {
                            while (parent.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>().gridTilemapPosition == new Vector2Int(1234, 1234))
                            {
                                yield return null;
                            }
                            List[orderInList] = parent.transform.GetChild(i).GetChild(c).GetComponent<GridPosition>();
                            orderInList++;
                        }
                    }
                }
            }
            else
            {
                while (parent.transform.GetChild(i).GetComponent<GridPosition>().gridTilemapPosition == new Vector2Int(1234, 1234))
                {
                    Debug.Log("y");
                    yield return null;
                }
                List[orderInList] = parent.transform.GetChild(i).GetComponent<GridPosition>();
                orderInList++;
            }
        }
        switch (finishedSearching)
        {
            case "finishedSearchingPresent":
                finishedSearchingPresent = true;
                break;
            case "finishedSearchingFuture":
                finishedSearchingFuture = true;
                break;
        }
    }
}
