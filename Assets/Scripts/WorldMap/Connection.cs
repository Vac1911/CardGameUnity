using OneLine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Map
{

    [Serializable]
    public class Connection
    {
        public int destinationIndex;

        [NonSerialized]
        private Node parent;

        public Connection(int destinationIndex)
        {
            this.destinationIndex = destinationIndex;
        }

        public Node GetParent()
        {
            return parent;
        }

        /*
         * Alternate parameters to match syntax of GetDestination()
         */
        public Node GetParent(WorldMap map)
        {
            return GetParent();
        }

        public void SetParent(Node _parent)
        {
            parent = _parent;
        }

        public Node GetDestination(WorldMap map)
        {
            return map.nodes[destinationIndex];
        }
    }
}