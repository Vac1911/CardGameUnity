using Patterns.StateMachine;

namespace Tools.UI.Card
{
    /// <summary>
    ///     A complete UI card.
    /// </summary>
    public interface IUiCard : IStateMachineHandler, IUiCardComponents, IUiCardTransform
    {
        IUiPlayerHand Hand { get; }
        bool IsDragging { get; }
        bool IsHovering { get; }
        bool IsDisabled { get; }
        bool IsPlayer { get; }
        CardGame.Card GetData();
        void SetData(CardGame.Card card);
        void Disable();
        void Enable();
        void Select();
        void Unselect();
        void Hover();
        void Draw();
        void Discard();
        void Reshuffle();
    }
}