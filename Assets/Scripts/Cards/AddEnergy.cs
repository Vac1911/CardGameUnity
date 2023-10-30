using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace CardGame.Effects
{
    [Serializable]
    public class AddEnergy : IEffect
    {
        public string label = "Add Energy";
        string IEffect.text { get => string.Format("+{0} Energy", value); }

        public int value;

        public async Task DoEffect(Character character)
        {
            character.energy += value;
            await Task.Yield();
        }

        public List<Vector3Int> GetPossibleTarget(Character character)
        {
            return new List<Vector3Int>();
        }

        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition)
        {
            List<Vector3Int> area = new List<Vector3Int>();
            return area;
        }
    }
}