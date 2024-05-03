using CardGame;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// See: https://github.com/pixelfac/2D-Astar-Pathfinding-in-Unity/blob/master/Pathfinding2D.cs
public class Pathfinder
{
    List<PathNode> grid;
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

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var graph = encounterGrid.GetNavGraph();
        var path = FindPath(GetPathNode(start), GetPathNode(goal));

        return path.Select(node => node.gridPosition).ToList();
    }

    public List<PathNode> FindPath(PathNode _startNode, PathNode _endNode)
    {
        BuildNavGraph();

        startNode = _startNode;
        endNode = _endNode;

        List<PathNode> openSet = new List<PathNode>();
        HashSet<PathNode> closedSet = new HashSet<PathNode>();
        openSet.Add(startNode);

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
                return RetracePath();
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

        return RetracePath();
    }

    //reverses calculated path so first node is closest to seeker
    List<PathNode> RetracePath()
    {
        List<PathNode> path = new();

        // Idk why but endNode.parent is null here, so this is a workaround
        PathNode currentNode = GetPathNode(endNode.gridPosition);

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
            yield return GetPathNode(pos);
        pos = new Vector3Int(center.X, center.Y - 1);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);
        pos = new Vector3Int(center.X + 1, center.Y - 1);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);

        //middle row
        pos = new Vector3Int(center.X - 1, center.Y);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);
        pos = new Vector3Int(center.X + 1, center.Y);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);

        //bottom row
        pos = new Vector3Int(center.X - 1, center.Y + 1);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);
        pos = new Vector3Int(center.X, center.Y + 1);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);
        pos = new Vector3Int(center.X + 1, center.Y + 1);
        if (IsValidNeighbor(pos))
            yield return GetPathNode(pos);
    }

    private bool IsValidNeighbor(Vector3Int pos)
    {
        return GetPathNode(pos) != null;
    }

    protected PathNode GetPathNode(Vector3Int pos)
    {
        return grid.Find(n => n.gridPosition == pos);
    }

    protected Vector3Int GetGridPosition(PathNode pathNode)
    {
        return new Vector3Int(pathNode.X, pathNode.Y, 0);
    }
}