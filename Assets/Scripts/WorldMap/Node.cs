using OneLine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Map
{
    public enum NodeState
    {
        Unvisited,
        Visited,
        Current,
    }

    [Serializable]
    public class Node
    {
        public NodeType type;
        public NodeState state = NodeState.Unvisited;
        public Vector3 position;

        [OneLine]
        public List<Connection> connections = new List<Connection>();
        private Vector2Int start;

        public Node(Vector3 position, NodeType type)
        {
            this.position = position;
            this.type = type;
        }

        public Node(Vector2Int position, NodeType type)
        {
            this.position = ((Vector3Int)position);
            this.type = type;
        }

        public void AddConnection(Connection connection)
        {
            connections.Add(connection);
        }

        public void AddConnection(int DestinationIndex)
        {
            AddConnection(new Connection(DestinationIndex));
        }

        public void Setup()
        {
            foreach (Connection conn in connections)
            {
                conn.SetParent(this);
            }
        }
    }
}