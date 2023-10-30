using CardGame;
using CardGame.Effects;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Tools.UI.Card;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

namespace CardGame
{
    public enum Team
    {
        None,
        Friendly,
        Enemy,
    }

    public abstract class Character : MonoBehaviour
    {
        public EncounterGrid encounterGrid;
        public GridTransform gridTransform;

        public Team team;
        public int maxHealth;
        public int health;
        public int baseEnergy = 1;
        public int energy = 0;
        public int baseHandSize = 4;

        public List<Card> deckList = new List<Card>();
        [HideInInspector]
        public List<Card> drawPile = new List<Card>();
        [HideInInspector]
        public List<Card> hand = new List<Card>();
        [HideInInspector]
        public List<Card> discardPile = new List<Card>();

        public event Action<Card> OnCardDraw = card => { };
        public event Action<Card> OnCardDiscard = card => { };
        public event Action<Character> OnTurnStart = character => { };
        public event Action<Character> OnTurnEnd = character => { };
        public event Action<Character> OnTakeDamage = character => { };
        public event Action<Character> OnDeathEvent = character => { };

        protected readonly Vector3Int[] neighbourPositions =
        {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    
        // diagonal neighbours
        Vector3Int.up + Vector3Int.right,
        Vector3Int.up + Vector3Int.left,
        Vector3Int.down + Vector3Int.right,
        Vector3Int.down + Vector3Int.left
    };

        public Tilemap tilemap
        {
            get { return encounterGrid.tilemap; }
        }

        public Grid grid
        {
            get { return encounterGrid.layoutGrid; }
        }

        // Start is called before the first frame update
        protected void Start()
        {
            health = maxHealth;
            gridTransform = GetComponent<GridTransform>();
            drawPile = new List<Card>(deckList);
            drawPile.Shuffle();
        }

        public virtual void StartTurn()
        {
            DrawCards(baseHandSize);
            energy = baseEnergy;
        }

        public virtual void EndTurn()
        {
            while (hand.Count > 0)
            {
                var card = hand[0];
                DiscardCard(card);
            }
        }

        // Called when a card is played
        public async Task PlayCard(Card card)
        {
            // TODO: Make sure player has enough energy to play the card
            if (energy < card.cost)
            {
                return;
            }
            energy -= card.cost;

            DiscardCard(card);

            foreach (var effect in card.effects)
            {
                await effect.DoEffect(this);
            }
        }

        // Get the target position for an effect
        public abstract Task<Vector3Int> GetEffectTarget(IEffect effect, List<Vector3Int> positions);

        // Draws a card
        public void DrawCard()
        {
            if (drawPile.Count == 0)
            {
                /*Debug.Log("Hydrate");*/
                if (!HydrateDrawPile())
                {
                    /*Debug.Log("Could not hydrate");*/
                    return;
                }
            }

            var card = drawPile[0];
            drawPile.Remove(card);
            hand.Add(card);
            OnCardDraw(card);
        }

        // Draw `x` cards
        public void DrawCards(int x)
        {
            for (int i = 0; i < x; i++)
            {
                DrawCard();
            }
        }

        // Put card into the `discardPile` from `hand`
        public void DiscardCard(Card card)
        {
            hand.Remove(card);
            discardPile.Add(card);
            OnCardDiscard(card);
        }

        /// <summary>
        ///     Shuffle the discard pile back into the deck
        /// </summary>
        /// <returns> Was the deck sucesfully hydrated? </returns>
        [Button]
        public bool HydrateDrawPile()
        {
            if (discardPile.Count == 0)
            {
                return false;
            }

            while (discardPile.Count > 0)
            {
                var card = discardPile[0];
                discardPile.Remove(card);
                drawPile.Add(card);
            }

            return true;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
            OnTakeDamage(this);

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            OnDeathEvent(this);
            Debug.Log("OnDestroy1");
        }

        public Vector3Int GetCellPosition()
        {
            return grid.WorldToCell(transform.position);
        }

        public void SetCellPosition(Vector3Int cellPosition)
        {
            gridTransform.position = cellPosition;
        }

        public List<Vector3Int> GetAdjacentPositions(bool withDiagonal = true)
        {
            List<Vector3Int> positions = new List<Vector3Int>();
            var neighbors = withDiagonal ? neighbourPositions : neighbourPositions.Take(4);
            foreach (var neighbour in neighbors)
            {
                Vector3Int position = GetCellPosition() + neighbour;
                positions.Add(position);
            }
            return positions;
        }

        protected bool IsCurrentTurn()
        {
            return EncounterManager.Instance.GetCurrentCharacter() == this;
        }
    }
}