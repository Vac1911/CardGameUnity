using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame.UI
{
    public class UIEnergyCount : MonoBehaviour
    {
        public Character character;
        public TextMeshProUGUI text;

        void Update()
        {
            if(character != null) text.text = character.energy.ToString();
        }
    }
}