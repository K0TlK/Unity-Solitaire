using UnityEngine;

namespace Cards
{
    interface ICardRender
    {
        /// <summary>
        /// Checking if the drawing of the card is complete.
        /// </summary>
        bool isRenderComplete { get; }


        /// <summary>
        /// Set new visual with update card.
        /// </summary>
        /// <param name="name">Card name</param>
        /// <param name="suit">Card suit</param>
        /// <param name="state">Card state</param>
        void Render
            (Card.CardName name,
            Card.CardSuit suit,
            Card.CardState state = Card.CardState.LOCK);

        /// <summary>
        /// Update card visual.
        /// </summary>
        void Render();
    }
}
