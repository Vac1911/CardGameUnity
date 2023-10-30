using CardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// See: https://github.com/pixelfac/2D-Astar-Pathfinding-in-Unity/blob/master/Pathfinding2D.cs
public class Pathfinder
{
    Dictionary<Vector3Int, PathNode> grid;
    PathNode startNode, endNode;

    EncounterGrid encounterGrid;

    public Pathfinder(EncounterGrid encounterGrid)
    {
        this.encounterGrid = encounterGrid;
        BuildNavGraph();
    }

    public void BuildNavGraph()
    {
        grid = encounterGrid.GetNavGraph();
    }

    public void DebugNavGraph()
    {
        var bounds = encounterGrid.tilemap.cellBounds;
        var output = "";
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            output += "\n";
            for (int y = bounds.yMax; y < bounds.yMin; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                output += grid.ContainsKey(pos) ? "1" : "0";
            }
        }
        Debug.Log(output);
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var graph = encounterGrid.GetNavGraph();
        var path = FindPath(GetPathNode(start), GetPathNode(goal));

        return path.Select(node => GetGridPosition(node)).ToList();
    }

    public List<PathNode> FindPath(PathNode _startNode, PathNode _endNode)
    {
        BuildNavGraph();
        DebugNavGraph();
        /*Debug.Log(string.Join(Environment.NewLine, grid));*/

        startNode = _startNode;
        endNode = _endNode;

        List<PathNode> openSet = new List<PathNode>();
        HashSet<PathNode> closedSet = new HashSet<PathNode>();
        openSet.Add(startNode);
        Debug.Log("FindPath");

        //calculates path for pathfinding
        while (openSet.Count > 0)
        {
            PathNode node = openSet[0];

            //iterates through openSet and finds lowest FCost
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost <= node.FCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            //If target found, retrace path
            if (node == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            //adds neighbor nodes to openSet
            foreach (PathNode neighbour in GetNeighbors(node))
            {
                if (neighbour.obstacle || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<PathNode>();
    }

    //reverses calculated path so first node is closest to seeker
    List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;

    }

    //gets distance between 2 nodes for calculating cost
    int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public IEnumerable<PathNode> GetNeighbors(PathNode center)
    {
        //top row
        var pos = new Vector3Int(center.X - 1, center.Y - 1);
        if (IsValidNeighbor(pos))
            yield return grid[pos];
        pos = new Vector3Int(center.X, center.Y - 1);
        if (IsValidNeighbor(pos))
            yield return grid[pos];
        pos = new Vector3Int(center.X + 1, center.Y - 1);
        if (IsValidNeighbor(pos))
            yield return grid[pos];

        //middle row
        pos = new Vector3Int(center.X - 1, center.Y);
        if (IsValidNeighbor(pos))
            yield return grid[pos];
        pos = new Vector3Int(center.X + 1, center.Y);
        if (IsValidNeighbor(pos))
            yield return grid[pos];

        //bottom row
        pos = new Vector3Int(center.X - 1, center.Y + 1);
        if (IsValidNeighbor(pos))
            yield return grid[pos];
        pos = new Vector3Int(center.X, center.Y + 1);
        if (IsValidNeighbor(pos))
            yield return grid[pos];
        pos = new Vector3Int(center.X + 1, center.Y + 1);
        if (IsValidNeighbor(pos))
            yield return grid[pos];


    }

    private bool IsValidNeighbor(Vector3Int pos)
    {
        return grid.ContainsKey(pos);
    }

    protected PathNode GetPathNode(Vector3Int gridPosition)
    {
        return grid[gridPosition];
    }

    protected Vector3Int GetGridPosition(PathNode pathNode)
    {
        return new Vector3Int(pathNode.X, pathNode.Y, 0);
    }

    /*   protected Vector2Int GetOffset()
       {
           return new Vector2Int(encounterGrid.tilemap.cellBounds.xMin, encounterGrid.tilemap.cellBounds.yMin);
       }

       protected PathNode GetPathNode(Vector3Int gridPosition)
       {
           Vector2Int offset = GetOffset();
           return new PathNode(gridPosition.x + offset.x, gridPosition.y + offset.y);
       }

       protected Vector3Int GetGridPosition(PathNode pathNode)
       {
           Vector2Int offset = GetOffset();
           return new Vector3Int(pathNode.X + offset.x, pathNode.Y + offset.y, 0);
       }*/
}