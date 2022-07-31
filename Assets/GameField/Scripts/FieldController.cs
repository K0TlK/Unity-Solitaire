using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

namespace GameField
{
    public class FieldController : MonoBehaviour
    {
        public const string NonAdditionalInformation = "";

        [SerializeField] private CardController m_cardPrefab;
        [SerializeField] private Transform m_creationPoint;
        [SerializeField] private CardSpriteManager m_cardSpriteManager;
        private CardDeck m_deck;
        private List<CardController> m_cards = new List<CardController>();

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

        public CardController AddCard(Card.CardName name, Card.CardSuit suit, string additionalInformation = NonAdditionalInformation)
        {
            m_cards.Add(Instantiate(m_cardPrefab, m_creationPoint));
            m_cards[m_cards.Count - 1].SetVisual(name, suit);
            m_cards[m_cards.Count - 1].additionalInformation = additionalInformation;

            return m_cards[m_cards.Count - 1];
        }

        public void Clear()
        {
            foreach (CardController card in m_cards)
            {
                Destroy(card);
            }
            m_cards.Clear();
        }
    }
}

