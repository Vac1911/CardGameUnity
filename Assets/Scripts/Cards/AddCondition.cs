using CardGame.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using UnityEngine;

namespace CardGame.Effects
{
    [Serializable]
    public class AddCondition : IEffect
    {
        public enum ConditionClasses
        {
            Power,
        }

        public string label = "Add Condition";
        string IEffect.text { get => string.Format("Add {0}", GetCondition().text); }

        // Using a string to match to classname because of issue with unity inspector ui
        public ConditionClasses conditionName;

        public int value;

        public Condition GetCondition()
        {
            /*var conditionTypes = Utils.GetImplementingClasses(0typeof(Condition));
            var conditionType = Array.Find(conditionTypes, t => t.ToString() == conditionName);
            var condition = (Condition)Activator.CreateInstance(conditionType);*/

            switch(conditionName)
            {
                case (ConditionClasses.Power):
                    return new Power(value);
                default:
                    throw new Exception(string.Format("Invalid Conditon Name {0}", conditionName));
            }
            
        }

        public async Task DoEffect(Character character)
        {
            character.AddCondition(GetCondition());
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