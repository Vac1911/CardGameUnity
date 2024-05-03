using CardGame.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class UIStatus : MonoBehaviour
    {
        public UICharacterInfo characterInfo;
        public TextMeshProUGUI text;

        public Character character
        {
            get { return characterInfo.character; }
        }

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            UpdateStatuses();
            character.OnConditionChange += c => UpdateStatuses();
        }

        void Update()
        {
        }

        public void UpdateStatuses()
        {
            foreach (var condition in character.conditions)
            { 
                Debug.Log(condition.text);
                text.text = condition.text;
            }
        }
    }
}