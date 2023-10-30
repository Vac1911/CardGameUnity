using CardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools.UI.Card
{
    //------------------------------------------------------------------------------------------------------------------

    /// <summary>
    ///     The Player Hand.
    /// </summary>
    public class UiPlayerHand : UiCardPile, IUiPlayerHand
    {
        //--------------------------------------------------------------------------------------------------------------

        #region Properties

        /// <summary>
        ///     Card currently selected by the player.
        /// </summary>
        public IUiCard SelectedCard { get; private set; }

        public event Action<IUiCard>  OnCardSelected = card => { };

        public event Action<IUiCard> OnCardPlayed = card => { };

        public event Action<IUiCard> OnCardDiscard = card => { };

        [SerializeField]
        [Tooltip("Character")]
        public Character character;

        [SerializeField]
        [Tooltip("Game view transform")]
        Transform gameView;

        [SerializeField]
        [Tooltip("World point where the deck is positioned")]
        Transform deckPosition;

        [SerializeField]
        [Tooltip("Prefab of the Card C#")]
        GameObject cardPrefabCs;
        int Count { get; set; }

        /// <summary>
        ///     UI Event raised when a card is played.
        /// </summary>
        Action<IUiCard> IUiPlayerHand.OnCardPlayed
        {
            get => OnCardPlayed;
            set => OnCardPlayed = value;
        }

        /// <summary>
        ///     UI Event raised when a card is discarded.
        /// </summary>
        Action<IUiCard> IUiPlayerHand.OnCardDiscard
        {
            get => OnCardDiscard;
            set => OnCardDiscard = value;
        }

        /// <summary>
        ///     UI Event raised when a card is selected.
        /// </summary>
        Action<IUiCard> IUiPlayerHand.OnCardSelected
        {
            get => OnCardSelected;
            set => OnCardSelected = value;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Unitycallbacks

        protected virtual void Start()
        {
            // Bind to CharacterManager Events
            character.OnCardDraw += DrawCard;

            character.OnCardDiscard += Discard;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Operations

        void DrawCard(CardGame.Card cardData)
        {
            // Spawn UI Card
            var cardGo = Instantiate(cardPrefabCs, gameView);
            cardGo.name = "Card_" + Count;
            Count++;
            var uiCard = cardGo.GetComponent<IUiCard>();
            uiCard.transform.position = deckPosition.position;
            uiCard.SetData(cardData);

            // Add UI Card to this pile
            AddCard(uiCard);
        }

        /// <summary>
        ///     Select the card in the parameter.
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(IUiCard card)
        {
            SelectedCard = card ?? throw new ArgumentNullException("Null is not a valid argument.");

            //disable all cards
            DisableCards();
            NotifyCardSelected();
        }

        /// <summary>
        ///     Play the card which is currently selected. Nothing happens if current is null.
        /// </summary>
        /// <param name="card"></param>
        public void PlaySelected()
        {
            if (SelectedCard == null)
                return;

            PlayCard(SelectedCard);
        }

        /// <summary>
        ///     Play the card in the parameter.
        /// </summary>
        /// <param name="card"></param>
        public void PlayCard(IUiCard card)
        {
            if (card == null)
                throw new ArgumentNullException("Null is not a valid argument.");

            character.PlayCard(card.GetData());

            /*RemoveCard(card);*/
            OnCardPlayed?.Invoke(card);
            EnableCards();
            NotifyPileChange();
        }

        /// <summary>
        ///     Unselect the card in the parameter.
        /// </summary>
        /// <param name="card"></param>
        public void UnselectCard(IUiCard card)
        {
            if (card == null)
                return;

            SelectedCard = null;
            card.Unselect();
            NotifyPileChange();
            EnableCards();
        }

        /// <summary>
        ///     Unselect the card which is currently selected. Nothing happens if current is null.
        /// </summary>
        public void Unselect() => UnselectCard(SelectedCard);

        /// <summary>
        ///     Disables input for all cards.
        /// </summary>
        public void DisableCards()
        {
            foreach (var otherCard in Cards)
                otherCard.Disable();
        }

        /// <summary>
        ///     Enables input for all cards.
        /// </summary>
        public void EnableCards()
        {
            foreach (var otherCard in Cards)
                otherCard.Enable();
        }

        /// <summary>
        ///     Discard a card.
        /// </summary>
        public void Discard(IUiCard card)
        {
            OnCardDiscard?.Invoke(card);
            RemoveCard(card);
        }

        public void Discard(CardGame.Card cardData)
        {
            Func<IUiCard, bool> match = (IUiCard c) => c.GetData() == cardData;

            if(SelectedCard != null && match(SelectedCard))
            {
                Discard(SelectedCard);
                SelectedCard = null;
            }
            else
            {
                Discard(Cards.First((c) => c.GetData() == cardData));
            }
        }

        /// <summary>
        ///     Discard all cards.
        /// </summary>
        [Button]
        public void DiscardAll()
        {
            while(Cards.Count > 0)
            {
                var card = Cards[0];
                Discard(card);

            }
        }

        [Button]
        void NotifyCardSelected() => OnCardSelected?.Invoke(SelectedCard);

        #endregion
    }
}