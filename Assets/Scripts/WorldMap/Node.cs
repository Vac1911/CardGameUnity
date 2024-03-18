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
        public NodeState state = NodeState.Unvisited;
        [SerializeField]
        public NodeEvent nodeEvent;
        public Vector3 position;

        [OneLine]
        public List<Connection> connections = new List<Connection>();
        private Vector2Int start;

        public Node(Vector3 _position, NodeEvent _nodeEvent)
        {
            this.position = _position;
            this.nodeEvent = _nodeEvent;
        }

        public Node(Vector2Int _position, NodeEvent _nodeEvent)
        {
            this.position = ((Vector3Int)_position);
            this.nodeEvent = _nodeEvent;
        }

        public void AddConnection(Connection connection)
        {
            connections.Add(connection);
        }

        public void AddConnection(int DestinationIndex)
        {
            AddConnection(new Connection(DestinationIndex));
        }

        public bool HasConnectionTo(WorldMap map, Node node)
        {
            foreach(Connection connection in connections)
            {
                if(connection.GetDestination(map) == node) return true;
            }

            return false;
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