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

    public PathNode(Vector3Int v3) { X = v3.x; Y = v3.y; gridPosition = v3; }

    public void SetObstacle(bool isOb)
    {
        obstacle = isOb;
    }

    public override string ToString()
    {
        string str = "(" + X.ToString() + ", " + Y.ToString() + ")";
        if(parent != null)
        {
            /*str += " -> " + parent.X.ToString() + ", " + parent.Y.ToString();*/
            str += " -> " + parent.ToString();
        }
        return str;
    }
}