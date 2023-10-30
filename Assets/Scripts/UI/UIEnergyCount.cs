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
            text.text = character.energy.ToString();
        }
    }
}