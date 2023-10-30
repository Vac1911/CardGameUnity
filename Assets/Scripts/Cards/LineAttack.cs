using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardGame.Effects
{
    [Serializable]
    public class LineAttack : IEffect
    {
        public string label = "LineAttack";

        public int damage;
        public int range;

        string IEffect.text { get => string.Format("Line {0}: DMG {1}", range, damage); }

        protected readonly Vector3Int[] neighbourPositions =
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left,
        };

        public async Task DoEffect(Character character)
        {
            List<Vector3Int> positions = character.GetAdjacentPositions(false);
            Vector3Int targetPosition = await character.GetEffectTarget(this, GetPossibleTarget(character));
            Debug.Log("LineAttack " + targetPosition.ToString());
            var direction = targetPosition - character.GetCellPosition();

            for (int r = 1; r <= range; r++ )
            {
                var cellPosition = direction * r + character.GetCellPosition();

                Character attackedCharacter = character.encounterGrid.GetCharacterAtCell(cellPosition);
                if (attackedCharacter != null)
                {
                    Debug.Log("Hit");
                    attackedCharacter.TakeDamage(damage);
                }
            }

            await Task.Yield();
        }
        public List<Vector3Int> GetPossibleTarget(Character character)
        {
            List<Vector3Int> positions = new List<Vector3Int>();
            foreach (var neighbour in neighbourPositions)
            {
                for (int r = 1; r <= range; r++)
                {
                    Vector3Int position = neighbour * r + character.GetCellPosition();
                    positions.Add(position);
                }
            }
            return positions;
        }

        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition)
        {
            List<Vector3Int> area = new List<Vector3Int>();

            var direction = (targetPosition - character.GetCellPosition()).GetSignedVector();
            for (int r = 1; r <= range; r++)
            {
                var cellPosition = direction * r + character.GetCellPosition();

                area.Add(cellPosition);
            }
            return area;
        }
    }
}