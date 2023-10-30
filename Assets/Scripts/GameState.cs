using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame.Map;
using System;

namespace CardGame
{
    [Serializable]
    public class GameState : MonoBehaviour
    {
        public WorldMap map;
        public List<Card> deckList = new List<Card>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}