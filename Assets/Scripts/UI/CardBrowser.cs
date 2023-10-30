using CardGame;
using System.Collections;
using System.Collections.Generic;
using Tools.UI.Card;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardBrowser : MonoBehaviour
{

    public List<Card> cards = new List<Card>();

    [SerializeField]
    [Tooltip("Prefab of the Card C#")]
    GameObject cardPrefabCs;

    private RectTransform rectTransform;

    void Start()
    {
        this.rectTransform = GetComponent<RectTransform>();
        Draw();
    }

    void Draw()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];

            var cardGo = Instantiate(cardPrefabCs, this.transform);
            cardGo.GetComponent<RectTransform>().SetParent(rectTransform);
            var uiCard = cardGo.GetComponent<IUiCard>();
            if(uiCard != null) {
                uiCard.SetData(card);
            }
        }
    }
}
