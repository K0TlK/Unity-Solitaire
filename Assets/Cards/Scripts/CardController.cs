using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardController : MonoBehaviour
    {
        private ICardRender cardRender;

        private void Awake()
        {
            cardRender = GetComponent<ICardRender>();
        }

        public void SetVisual(Card.CardName name, Card.CardSuit suit)
        {
            cardRender.Render(name, suit);
        }
    }
}
