using CardGame.Effects;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;
using static UnityEngine.UI.Image;

namespace CardGame
{
    public class Enemy : Character
    {
        public Card moveCard;
        public Card attackCard;

        protected Vector3Int target;
        protected Pathfinder pathfinder;

        protected new void Start()
        {
            deckList = new List<Card> { moveCard, attackCard };
            pathfinder = new Pathfinder(encounterGrid);
            base.Start();
        }

        public override void StartTurn()
        {
            base.StartTurn();
            DoTurn();
        }

        public void DoTurn()
        {
            // Get Characters that can be attacked
            var cellTargets = attackCard.effects[0].GetPossibleTarget(this);
            List<Character> characterTargets = cellTargets.Select(cell => encounterGrid.GetCharacterAtCell(cell)).Where(character => character != null && character.team == Team.Friendly).ToList();

            if (characterTargets.Count != 0)
            {
                target = characterTargets[0].gridTransform.position;
                PlayCard(attackCard);
            }
            else
            {
                // TODO: play `moveCard` and move to the player
                List<Character> characters = encounterGrid.GetCharacters(Team.Friendly);
                var closestOpposingCharacter = characters.MinBy(c => Vector3Int.Distance(this.gridTransform.position, c.gridTransform.position));

                // TODO: if no path can be found, try moving to a different opposing character
                var path = pathfinder.FindPath(gridTransform.position, closestOpposingCharacter.gridTransform.position);
                var moveLimit = 3;
                var pathLimit = path.SkipLast(1).Take(Math.Min(path.Count - 1, moveLimit)).ToList();

                for (var i = 1; i < pathLimit.Count; i++)
                {

                    var start = encounterGrid.CellToWorldPosition(path[i - 1]);
                    var end = encounterGrid.CellToWorldPosition(path[i]);
                    Debug.DrawLine(start, end, Color.cyan, 5f);
                }

                PlayCard(moveCard);
            }

            EncounterManager.Instance.AdvanceTurn();
        }

        public override Task<Vector3Int> GetEffectTarget(IEffect effect, List<Vector3Int> positions)
        {
            Debug.Log("GetEffectTarget");
            TaskCompletionSource<Vector3Int> tcs1 = new TaskCompletionSource<Vector3Int>();
            Task<Vector3Int> t1 = tcs1.Task;


            tcs1.SetResult(target);

            return t1;
        }
    }
}