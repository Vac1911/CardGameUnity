using CardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CardGame.Map
{

    [Serializable]
    public class WorldMap : MonoBehaviour
    {
        public List<Node> nodes = new List<Node>();

        public GameObject nodePrefab;
        public GameObject linePrefab;

        public Node currentNode;

        public Vector3 nodeScale = Vector3.one;

        protected List<NodeView> nodeViews = new List<NodeView>();

        protected MapGenerator generator;

        // Start is called before the first frame update
        void Start()
        {
            generator = GetComponent<MapGenerator>();
            generator.Generate();
            this.nodes = generator.GetNodes();

            Render();
        }

        void Render()
        {
            foreach (var node in nodes)
            {
                node.Setup();
                GameObject nodeObj = Instantiate(nodePrefab, Vector3.Scale(node.position, nodeScale), Quaternion.identity, transform);
                NodeView nodeView = nodeObj.GetComponent<NodeView>();
                nodeView.map = this;
                nodeView.node = node;
                nodeViews.Add(nodeView);
            }

            // Because ConnectionView uses on the position of NodeView, they must be initialized after
            foreach (var node in nodes)
            {
                foreach (var connection in node.connections)
                {
                    GameObject lineObj = Instantiate(linePrefab, transform);
                    lineObj.transform.parent = this.transform;
                    var connectionView = lineObj.GetComponent<LineView>();
                    connectionView.map = this;
                    connectionView.connection = connection;
                }
            }

            currentNode = nodes[0];
            currentNode.state = NodeState.Current;
        }

        public NodeView GetNodeView(Node node)
        {
            foreach(var nodeView in nodeViews)
            {
                if(nodeView.node == node) return nodeView;
            }

            return null;
        }

        public void TravelTo(Node node)
        {
            if (currentNode == null)
            {
                Debug.Log("No current node");
                return;
            }

            if (!currentNode.HasConnectionTo(this, node))
            {
                Debug.Log("Cannot travel to unconnected node");
                return;
            }

            if (node.state != NodeState.Unvisited)
            {
                Debug.Log("Can only travel to connected nodes");
            }

            currentNode.state = NodeState.Visited;
            node.state = NodeState.Current;

            node.nodeEvent.OnVisit();
        }
    }
}