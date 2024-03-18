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
        public WorldMap map;
        public List<Card> deckList = new List<Card>();
    }
}