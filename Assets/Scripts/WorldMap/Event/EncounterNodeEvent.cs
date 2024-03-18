using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class EncounterNodeEvent : NodeEvent
    {
        public int seed;
        public override Sprite GetSprite()
        {
            if(sprite == null) {
                sprite = Resources.Load<Sprite>("Map/NodeBase");
            }

            return sprite;
        }

        public override void OnVisit()
        {
            throw new System.NotImplementedException();
        }
    }
}
