using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

namespace GameField
{
    public class FieldController : MonoBehaviour
    {
        [SerializeField] private CardController m_cardPrefab;
        [SerializeField] private Transform m_creationPoint;
        [SerializeField] private CardSpriteManager m_cardSpriteManager;
        private CardDeck m_deck;
        private Stack<CardController> m_cards = new Stack<CardController>();

        void Start()
        {
            if (m_creationPoint == null)
            {
                m_creationPoint = gameObject.transform;
            }

            if (m_deck == null)
            {
                m_deck = gameObject.GetComponent<CardDeck>();
            }
        }

        public void AddCard(Card.CardName name, Card.CardSuit suit)
        {
            m_cards.Push(Instantiate(m_cardPrefab, m_creationPoint));
            m_cards.Pop().SetVisual(name, suit);
        }

        public void Clear()
        {
            foreach (CardController card in m_cards)
            {
                Destroy(card);
            }
            m_cards = new Stack<CardController>();
        }
    }
}

