using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Conditions
{
    [Serializable]
    public class Power : Condition
    {
        [SerializeField]
        public int value = 1;

        public override string text { get => string.Format("{0} Power", value); }
        public override string description => throw new System.NotImplementedException();
        public override ConditionLength length { get => ConditionLength.Combat; }
    }
}
