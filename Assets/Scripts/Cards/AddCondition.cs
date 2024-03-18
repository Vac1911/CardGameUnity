using CardGame.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.Effects
{
    [Serializable]
    public class AddCondition : IEffect
    {

        public string label = "Add Condition";
        string IEffect.text { get => string.Format("Add ", condition.text); }

        [SerializeReference]
        public Condition condition;

        public async Task DoEffect(Character character)
        {
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