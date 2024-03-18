using Patterns;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGame
{
    public class EncounterManager : SingletonMB<EncounterManager>
    {
        public EncounterGrid grid;

        // All character mangers in the encounter
        public List<Character> characters;

        // Where we are in the turn order (-1 if we have not started the turn order)
        public int turnIndex = -1;

        // Start is called before the first frame update
        void Start()
        {
            foreach(var character in characters)
            {
                character.OnDeathEvent += HandleCharacterDeath;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(turnIndex == -1) AdvanceTurn();
        }

        public void AdvanceTurn()
        {
            // End the active character's turn
            if(turnIndex != -1)
            {
                characters[turnIndex].EndTurn();
            }

            // Advance the turnIndex
            turnIndex++;
            if(turnIndex >= characters.Count)
                turnIndex = 0;

            // Start the active character's turn
            characters[turnIndex].StartTurn();
        }

        public void HandleCharacterDeath(Character chatacter)
        {
            chatacter.OnDeathEvent -= HandleCharacterDeath;

            int characterIndex = characters.IndexOf(chatacter);

            if(characterIndex > turnIndex)
                turnIndex -= 1;
            characters.Remove(chatacter);
        }

        public bool IsEncounterOver()
        {
            if (characters.All(c => c.team != Team.Enemy))
            {
                return true;
            }

            return false;
        }

        public Character GetCurrentCharacter()
        {
            return characters[turnIndex];
        }
    }
}