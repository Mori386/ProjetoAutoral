using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverLayerUI : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    void Start()
    {
        Debug.Log(layerMask.value);
    }
}
