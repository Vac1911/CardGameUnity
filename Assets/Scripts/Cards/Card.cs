using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame.Effects;
using System;
using UnityEditor;
using Tools.UI.Card;

namespace CardGame {
    [CreateAssetMenu(menuName = "Create Card")]
    [Serializable]
    public class Card : ScriptableObject
    {
        [SerializeField]
        public int cost;

        [SerializeReference]
        public List<IEffect> effects = new List<IEffect>();

        [SerializeField]
        public string cardName;

        [SerializeField]
        public Sprite cardArt;
    }
}