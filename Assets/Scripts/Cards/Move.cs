using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace CardGame.Effects
{
    [Serializable]
    public class Move : IEffect
    {
        public string label = "Move";
        string IEffect.text { get => string.Format("Move {0}", value); }
        /*public override string text => "Move";*/

        public int value;

        public async Task DoEffect(Character character)
        {

            Vector3Int cellPosition = await character.GetEffectTarget(this, this.GetPossibleTarget(character));
            character.SetCellPosition(cellPosition);
            return;
        }

        public void DrawFx(Character character)
        {

        }

        public List<Vector3Int> GetPossibleTarget(Character character)
        {
            return character.encounterGrid.GetMovementPositions(character.GetCellPosition(), value);
        }

        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition)
        {
            List<Vector3Int> area = new List<Vector3Int>();
            area.Add(targetPosition);
            return area;
        }
    }
}