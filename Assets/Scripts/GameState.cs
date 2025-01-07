using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame.Map;
using System;
using Patterns;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CardGame
{
    [Serializable]
    public class GameState : Singleton<GameState>
    {
        public uint seed;
        public WorldMap map;
        public List<Card> deckList = new List<Card>();

        public Node currentNode
        {
            get { return map.currentNode; }
        }
    }
}