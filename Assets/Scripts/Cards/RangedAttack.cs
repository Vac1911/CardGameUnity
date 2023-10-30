using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.Effects
{
    [Serializable]
    public class RangedAttack : IEffect
    {
        public string label = "Ranged Attack";
        string IEffect.text { get => string.Format("Range {0}: DMG {1}", range, damage); }
        /*public override string text => string.Format("Beam {0}: DMG {1}", range, damage);*/


        public int damage;
        public int range;

        public async Task DoEffect(Character character)

        {
            Vector3Int targetPosition = await character.GetEffectTarget(this, GetPossibleTarget(character));
            Debug.Log("RangedAttack " + targetPosition.ToString());
            Character attackedCharacter = character.encounterGrid.GetCharacterAtCell(targetPosition);
            if(attackedCharacter != null)
            {
                Debug.Log("Hit");
                attackedCharacter.TakeDamage(damage);
            }
            await Task.Yield();
        }
        public List<Vector3Int> GetPossibleTarget(Character character)
        {
            return character.encounterGrid.GetCircularCellPositionsFilled(character.GetCellPosition(), range);
        }

        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition)
        {
            List<Vector3Int> area = new List<Vector3Int>();
            area.Add(targetPosition);
            return area;
        }
    }
}