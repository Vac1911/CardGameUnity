using System;
using TMPro;
using UnityEngine;

namespace Tools.UI.Card
{
    //------------------------------------------------------------------------------------------------------------------

    /// <summary>
    ///     Card graveyard holds a register with cards played by the player.
    /// </summary>
    public class UiCardGraveyard : UiCardPile
    {
        [SerializeField] [Tooltip("World point where the graveyard is positioned")]
        Transform graveyardPosition;

        public TextMeshPro graveyardText;

        //--------------------------------------------------------------------------------------------------------------

        IUiPlayerHand PlayerHand { get; set; }


        //--------------------------------------------------------------------------------------------------------------

        #region Unitycallbacks

        protected override void Awake()
        {
            base.Awake();
            graveyardText ??= GetComponentInChildren<TextMeshPro>();
            PlayerHand = transform.parent.GetComponentInChildren<IUiPlayerHand>();
            PlayerHand.OnCardPlayed += AddCard;
            PlayerHand.OnCardDiscard += AddCard;
            OnPileChanged += UpdateText;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Operations

        /// <summary>
        ///     Adds a card to the graveyard or discard pile.
        /// </summary>
        /// <param name="card"></param>
        public override void AddCard(IUiCard card)
        {
            if (card == null)
                throw new ArgumentNullException("Null is not a valid argument.");

            Cards.Add(card);
            card.transform.SetParent(graveyardPosition);
            card.Discard();
            NotifyPileChange();
        }


        /// <summary>
        ///     Removes a card from the graveyard or discard pile.
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
            if (graveyardText != null)
            {
                graveyardText.SetText(string.Format("Discard ({0})", cards.Length));
            }
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------
    }
}