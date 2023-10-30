using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace CardGame.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public int width = 8;
        public int height = 15;
        public int pathDensity = 6;

        public List<NodeType> types = new List<NodeType>();

        protected List<Path> paths = new List<Path>();
        protected List<Node> nodes = new List<Node>();

        public void Generate()
        {
            // Implementation of https://steamcommunity.com/sharedfiles/filedetails/?id=2830078257

            CreatePaths();
            CreateNodes();
        }

        public List<Node> GetNodes()
        {
            return nodes;
        }

        protected void CreatePaths()
        {

            for (int i = 0; i < pathDensity; i++)
            {
                paths = paths.Concat(CreateRoute()).ToList();
            }
            paths = paths.Distinct().ToList();
        }

        protected void CreateNodes()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach (var path in paths)
            {
                positions.Add(path.start);
                positions.Add(path.end);
            }

            positions = positions.Distinct().ToList().OrderBy(p => p.y).ThenBy(p => p.x).ToList();

            foreach (var position in positions)
            {
                var node = new Node(position, types[0]);
                var nodePaths = paths.Where(path => path.start == position).ToArray();
                foreach(var path in nodePaths) {
                    node.AddConnection(Array.IndexOf(positions.ToArray(), path.end));
                }
                nodes.Add(node);
            }
            /*AdjustNodePositions();*/
        }

        protected void AdjustNodePositions()
        {
            List<Vector3> adjustedPositions = new List<Vector3>();
            foreach(var node in nodes)
            {
                var neighbors = GetConnectedNodes(node);
                if (neighbors.Count > 1)
                {
                    var centroid = neighbors.Aggregate(new Vector3(), (acc, n) => acc + n.position) / neighbors.Count;
                    /*adjustedPositions.Add(Vector3.MoveTowards(node.position, centroid,  0.1f));*/
                    adjustedPositions.Add(centroid);
                }
                else
                {
                    adjustedPositions.Add(node.position);
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].position = adjustedPositions[i];
            }
        }

        protected List<Node> GetConnectedNodes(Node node)
        {
            int nodeIndex = Array.IndexOf(nodes.ToArray(), node);
            List<Node> connectedNodes = node.connections.Select(c => nodes[c.destinationIndex]).ToList();

            return connectedNodes;
        }

        protected List<Path> CreateRoute()
        {
            List<Path> route = new List<Path>();

            // Create Random Path
            int x = Random.Range(0, width);
            Vector2Int currentPosition = new Vector2Int(x, 0);

            for (int y = 0; y < height; y++)
            {
                Vector2Int nextPosition = GetNextPosition(currentPosition);
                route.Add(new Path(currentPosition, nextPosition));
                currentPosition = nextPosition;
            }

            return route;
        }

        protected Vector2Int GetNextPosition(Vector2Int currentPosition)
        {
            List<Vector2Int> candidates = new List<Vector2Int> {
                new Vector2Int(currentPosition.x - 1, currentPosition.y + 1),
                new Vector2Int(currentPosition.x,     currentPosition.y + 1),
                new Vector2Int(currentPosition.x + 1, currentPosition.y + 1)
            };

            List<Vector2Int> positions = candidates.Where(p => p.x >= 0 && p.x <= width - 1 && !IsCrossingExistingPath(currentPosition, p)).ToList();

            if(positions.Count == 0)
            {
                foreach(Vector2Int p in candidates)
                {
                    foreach (Path path in paths)
                    {
                        if (path.IsCrossing(currentPosition, p))
                        {
                            Debug.Log("intersect");
                            Debug.Log(currentPosition.ToString() + " -> " + p.ToString());
                            Debug.Log(path.ToString());
                            continue;
                        }
                    }
                }
            }

            return positions.RandomItem();
        }

        protected bool IsCrossingExistingPath(Vector2Int p1, Vector2Int p2)
        {
            
            foreach(Path path in paths)
            {
                if (path.IsCrossing(p1, p2)) return true;
            }

            return false;
        }

        protected struct Path
        {
            public Vector2Int start;
            public Vector2Int end;

            public Path(Vector2Int currentPosition, Vector2Int nextPosition) : this()
            {
                this.start = currentPosition;
                this.end = nextPosition;
            }

            public bool IsCrossing(Vector2Int p1, Vector2Int p2)
            {
                if (p1.y == start.y && end.y == p2.y)
                {
                    if (p1.x == end.x && p2.x == start.x && p2 != end)
                    {
                        return true;
                    }
                }
                return false;
            }

            public override string ToString()
            {
                return start.ToString() + " -> " + end.ToString();
            }
        }
    }
}