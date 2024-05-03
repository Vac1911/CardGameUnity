using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Conditions
{
    [Serializable]
    public class Defense : Condition
    {
        public Defense(int value) : base(value)
        {
        }
        public override string text { get => string.Format("{0} Defense", value); }
        public override string description => throw new System.NotImplementedException();
        public override ConditionLength length { get => ConditionLength.Combat; }
    }
}
