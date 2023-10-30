using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.Effects
{
    public interface IEffect
    {
        public string text { get; }
        public Task DoEffect(Character character);
        public List<Vector3Int> GetPossibleTarget(Character character);
        public List<Vector3Int> GetAreaOfEffect(Character character, Vector3Int targetPosition);

    }
}