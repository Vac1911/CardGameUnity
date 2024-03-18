using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Tools.UI.Card;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGame.UI
{

    public class Reward : MonoBehaviour
    {
        public Card[] cardsData;
        public IUiCard[] cards;
        public UiCardParameters cardParameters;

        void Start()
        {
            cards = GetComponentsInChildren<IUiCard>();
            for (int i = 0; i < cards.Length; i++)
            {
                IUiCard cardUi = cards[i];
                UiCardComponent cardComponent = cardUi.gameObject.GetComponent<UiCardComponent>();
                cardComponent.cardConfigsParameters = cardParameters;

                cardUi.SetData(cardsData[i]);
                cardUi.Enable();

                cardUi.Input.OnPointerEnter += (eventData) => Debug.Log("here");
            }
        }

        void Update()
        {
            /*for (int i = 0; i < cards.Length; i++)
            {
                IUiCard cardUi = cards[i];

                foreach (var renderer in cardUi.Renderers)
                {
                    var myColor = renderer.color;
                    myColor.a = cardUi.IsHovering ? 0.25f : 1f;
                    renderer.color = myColor;
                }

            }*/
        }
    }
}
