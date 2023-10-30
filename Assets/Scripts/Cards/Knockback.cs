using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.Effects
{
    [Serializable]
    public class Knockback : IEffect
    {
        public string label = "Knockback";
        string IEffect.text { get => string.Format("Knockback {0}", spaces); }
        public int spaces;

        public async Task DoEffect(Character character)
        {
            List<Vector3Int> positions = character.GetAdjacentPositions();
            Vector3Int targetPosition = await character.GetEffectTarget(this, positions);
            Debug.Log("MeleeAttack " + targetPosition.ToString());
            Character attackedCharacter = character.encounterGrid.GetCharacterAtCell(targetPosition);
            if(attackedCharacter != null)
            {
                Debug.Log("Hit");
                var direction = targetPosition - character.GetCellPosition();
                attackedCharacter.SetCellPosition(attackedCharacter.GetCellPosition() + direction * spaces);
            }
            await Task.Yield();
        }

        public List<Vector3Int> GetPossibleTarget(Character character)
        {
            return new List<Vector3Int>();
        }

        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition)
        {
            List<Vector3Int> area = new List<Vector3Int>();
            area.Add(targetPosition);
            return area;
        }
    }
}