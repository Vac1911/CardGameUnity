using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.UI
{
    public class CharacterInfoManager : MonoBehaviour
    {
        public GameObject characterInfoPrefab;
        public UIEnergyCount energyCount;

        public void Init(Character[] characters)
        {
            foreach (Character c in characters)
            {
                InitCharacter(c);
            }
            energyCount.character = characters[0];
        }

        protected void InitCharacter(Character character)
        {
            var infoObject = Instantiate(characterInfoPrefab, this.transform);
            var info = infoObject.GetComponent<UICharacterInfo>();
            info.character = character;
        }
    }
}