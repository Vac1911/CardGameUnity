using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.UI
{
    public class CharacterInfoManager : MonoBehaviour
    {
        public GameObject characterInfoPrefab;

        void Start()
        {
            Character[] characters = Resources.FindObjectsOfTypeAll<Character>();
            foreach (Character c in characters)
            {
                InitCharacter(c);
            }
        }

        public void InitCharacter(Character character)
        {
            var infoObject = Instantiate(characterInfoPrefab, this.transform);
            var info = infoObject.GetComponent<UICharacterInfo>();
            info.character = character;
        }
    }
}