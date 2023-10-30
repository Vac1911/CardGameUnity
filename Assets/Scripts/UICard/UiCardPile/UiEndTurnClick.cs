using CardGame;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI.Card
{
    [RequireComponent(typeof(IMouseInput))]
    public class UiEndTurnClick : MonoBehaviour
    {
        public Character character;
        IMouseInput Input { get; set; }

        void Awake()
        {
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += EndTurn;
        }

        void EndTurn(PointerEventData obj) => EncounterManager.Instance.AdvanceTurn();
    }
}