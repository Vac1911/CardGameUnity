using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Conditions
{
    [Serializable]
    public abstract class Condition
    {
        public int value = 1;
        public abstract string text { get; }

        public abstract string description { get; }

        public abstract ConditionLength length { get; }
    }

    public enum ConditionLength
    {
        Turn,
        Combat
    }
}
