using Patterns.StateMachine;
using UnityEngine;

namespace Tools.UI.Card
{
    /// <summary>
    ///     State when a card is in the deck.
    /// </summary>
    public class UiCardInitial : UiBaseCardState
    {
        public UiCardInitial(IUiCard handler, BaseStateMachine fsm, UiCardParameters parameters) : base(handler, fsm,
            parameters)
        {
        }

        Vector3 StartScale { get; set; }

        //--------------------------------------------------------------------------------------------------------------

        #region Operations

        public override void OnEnterState()
        {
            Disable();
            SetScale();
            SetRotation();
        }

        void SetScale()
        {
            var finalScale = Handler.transform.localScale * Parameters.StartSizeWhenDraw;
            Handler.ScaleTo(finalScale, Parameters.ScaleSpeed);
        }

        void SetRotation()
        {
            var speed = Handler.IsPlayer ? Parameters.RotationSpeed : Parameters.RotationSpeedP2;
            Handler.RotateTo(Vector3.zero, speed);
        }

        #endregion
    }
}