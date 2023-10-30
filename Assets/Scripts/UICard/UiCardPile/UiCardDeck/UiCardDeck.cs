using Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Tools.UI.Card
{
    //------------------------------------------------------------------------------------------------------------------

    /// <summary>
    ///     Card deck holds a register with cards the player can draw.
    /// </summary>
    /// 
    [Obsolete("Logic regarding cards being transfered from one pile to another has been decoupled from the UI. See CharacterManager", false)]
    public class UiCardDeck : UiCardPile
    {

        //--------------------------------------------------------------------------------------------------------------

        #region Fields

        [SerializeField]
        [Tooltip("World point where the deck is positioned")]
        public Transform deckPosition;

        public TextMeshPro deckText;

        public UiPlayerHand PlayerHand;

        public UiCardGraveyard Graveyard;

        public event Action<IUiCard> OnCardDraw = card => { };

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Unitycallbacks

        protected override void Awake()
        {
            base.Awake();
            deckText ??= GetComponentInChildren<TextMeshPro>();
            /*PlayerHand = transform.parent.GetComponentInChildren<UiPlayerHand>();
            Graveyard = transform.parent.GetComponentInChildren<UiCardGraveyard>();*/
            OnPileChanged += UpdateText;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Operations

        [Button]
        public void DrawCard()
        {
            if(Cards.Count == 0) {
                if (!Hydrate()) {
                    Debug.Log("Could not hydrate");
                    return;
                }
            }

            var card = Cards[0];
            RemoveCard(card);
            /*card.SetActive(true);*/
            PlayerHand.AddCard(card);
        }

        /// <summary>
        ///     Shuffle the graveyard back into the deck
        /// </summary>
        /// <returns> Was the deck sucesfully hydrated? </returns>
        [Button]
        public bool Hydrate()
        {
            if (Graveyard.Cards.Count == 0)
            {
                return false;
            }


            while(Graveyard.Cards.Count > 0)
            {
                var card = Graveyard.Cards[0];
                Graveyard.RemoveCard(card);
                card.Reshuffle();
                AddCard(card);
            }

            return true;
        }

        /// <summary>
        ///     Adds a card to the draw pile.
        /// </summary>
        /// <param name="card"></param>
        public override void AddCard(IUiCard card)
        {
            if (card == null)
                throw new ArgumentNullException("Null is not a valid argument.");

            Cards.Add(card);
            card.transform.SetParent(deckPosition);

            /*card.SetActive(false);*/
            NotifyPileChange();
        }

        /// <summary>
        ///     Shuffles the cards in draw pile
        /// </summary>
        public void Shuffle()
        {
            Cards.Shuffle();
            NotifyPileChange();
        }


        /// <summary>
        ///     Removes a card from the draw pile.
        /// </summary>
        /// <param name="card"></param>
        public override void RemoveCard(IUiCard card)
        {
            if (card == null)
                throw new ArgumentNullException("Null is not a valid argument.");

            Cards.Remove(card);
            NotifyPileChange();
        }

        public void UpdateText(IUiCard[] cards)
        {
            if(deckText != null)
            {
                deckText.SetText(string.Format("Deck ({0})", cards.Length));
            }
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------
    }
}