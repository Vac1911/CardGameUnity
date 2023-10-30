using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace CardGame.Effects
{
    [Serializable]
    public class MeleeAttack : IEffect
    {
        public string label = "Melee Attack";
        public int damage;

        string IEffect.text { get => string.Format("Melee {0}", damage); }

        public async Task DoEffect(Character character)

        {
            List<Vector3Int> positions = character.GetAdjacentPositions();
            Vector3Int targetPosition = await character.GetEffectTarget(this, positions);
            Debug.Log("MeleeAttack " + targetPosition.ToString());
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
            return character.encounterGrid.GetAdjacentPositions(character.GetCellPosition());
        }

        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition)
        {
            List<Vector3Int> area = new List<Vector3Int>();
            area.Add(targetPosition);
            return area;
        }
    }
}