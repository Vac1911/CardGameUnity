using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathNode
{

    public int gCost, hCost;
    public bool obstacle = false;
    public Vector3Int gridPosition;

    public int X, Y;
    public PathNode parent = null;

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public PathNode(int x, int y) { X = x; Y = y; }
    public PathNode(float x, float y) { X = (int)x; Y = (int)y; }
    public PathNode(Vector2 v2) { X = (int)v2.x; Y = (int)v2.y; }
    public PathNode(Vector2Int v2) { X = v2.x; Y = v2.y; }
    public PathNode(Vector3 v3) { X = (int)v3.x; Y = (int)v3.y; }
    public PathNode(Vector3Int v3) { X = v3.x; Y = v3.y; }

    public void SetObstacle(bool isOb)
    {
        obstacle = isOb;
    }

    public override string ToString()
    {
        return base.ToString() + ": " + X.ToString() + ", " + Y.ToString();
    }
}
