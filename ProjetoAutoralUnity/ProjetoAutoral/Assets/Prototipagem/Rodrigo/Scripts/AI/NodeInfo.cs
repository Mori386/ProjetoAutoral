﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo
{
    public Vector2 gridPosition;
    public NodeInfo cameFromNode;
    public float distanceFromFinalPosition;
    public int nodesToGetThere;
}
