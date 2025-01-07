using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class EncounterNodeEvent : NodeEvent
    {
        public uint seed;
        public override Sprite GetSprite()
        {
            if(sprite == null) {
                sprite = Resources.Load<Sprite>("Map/NodeBase");
            }

            return sprite;
        }

        public override void OnVisit()
        {
            GameState.Instance.seed = seed;
            SceneManager.LoadScene("Encounter2");
        }
    }
}
