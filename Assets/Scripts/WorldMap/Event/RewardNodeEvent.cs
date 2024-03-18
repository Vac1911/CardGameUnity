using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class RewardNodeEvent : NodeEvent
    {
        public Scene eventScene;
        public List<Card> choices;

        public override Sprite GetSprite()
        {
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>("Map/NodeBase");
            }

            return sprite;
        }

        public override void OnVisit()
        {
            SceneManager.LoadScene(eventScene.path);
        }
    }
}