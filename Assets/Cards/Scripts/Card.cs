using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        public enum CardActive
        {
            TRUE,
            FALSE

        };

        public enum CardState
        {
            UNLOCK,
            LOCK
        };

        public enum CardSuit
        {
            NONE,
            HEARTS,
            DIAMONDS,
            CLUBS,
            SPADES
        };

        public enum CardColor
        {
            NONE,
            RED,
            BLACK
        }

        public enum CardName
        {
            NONE,
            NUM2,
            NUM3,
            NUM4,
            NUM5,
            NUM6,
            NUM7,
            NUM8,
            NUM9,
            NUM10,
            JACK,
            QUEEN,
            KING,
            ACE,
            JOCKER
        }

        private CardState m_state = CardState.LOCK;
        private CardSuit m_suit = CardSuit.NONE;
        private CardName m_name = CardName.NONE;

        /// <summary>
        /// Red or black color card. Set by card suit.
        /// </summary>
        public CardColor Color
        {
            get => GetCardColor();
        }

        public CardState State
        {
            get => m_state;
            set => m_state = value;
        }

        public CardSuit Suit
        {
            get => m_suit;
            set
            {
                if (value != CardSuit.NONE)
                {
                    m_suit = value;
                }
                else
                {
                    Debug.LogError("Set None");
                }
            }
        }

        public CardName Name
        {
            get => m_name;
            set
            {
                if (value != CardName.NONE)
                {
                    m_name = value;
                }
                else
                {
                    Debug.LogError("Set None");
                }
            }
        }

        private CardColor GetCardColor()
        {
            if (m_suit == CardSuit.HEARTS || m_suit == CardSuit.DIAMONDS)
            {
                return CardColor.RED;
            }

            if (m_suit == CardSuit.SPADES || m_suit == CardSuit.CLUBS)
            {
                return CardColor.BLACK;
            }

            return CardColor.NONE;
        }

        /// <summary>
        /// Makes the card a Joker
        /// </summary>
        /// <param name="jockerColor">Set Red or Black jocker color</param>

        public void SetJocker(CardColor jockerColor)
        {
            m_name = CardName.JOCKER;

            if (jockerColor == CardColor.BLACK)
            {
                m_suit = CardSuit.SPADES;
            }

            if (jockerColor == CardColor.RED)
            {
                m_suit = CardSuit.HEARTS;
            }
        }    

        public void SetCard(CardSuit suit, CardName name)
        {
            m_name = name;
            m_suit = suit;
        }
    }
}
