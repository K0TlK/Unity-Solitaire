using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardRenderType1 : MonoBehaviour, ICardRender
    {
        public bool isRenderComplete => m_isRenderComplete;

        private bool m_isRenderComplete = false;
        private CardSpriteManager m_cardSpriteManager;

        [SerializeField] private Card m_card;

        [SerializeField] SpriteRenderer m_topSuit;
        [SerializeField] SpriteRenderer m_topName;
        [SerializeField] SpriteRenderer m_downSuit;
        [SerializeField] SpriteRenderer m_downName;

        private void setCard()
        {
            if (m_card == null)
            {
                m_card = GetComponent<Card>();
            }
        }

        public void setCardSpriteManager()
        {
            if (m_cardSpriteManager == null)
            {
                m_cardSpriteManager = GetComponentInParent<CardSpriteManager>();
                if (m_cardSpriteManager == null)
                {
                    Debug.LogError("m_cardSpriteManager is null");
                }
            }
        }

        public void Render(Card.CardName name, Card.CardSuit suit, Card.CardState state = Card.CardState.LOCK)
        {
            setCard();
            m_card.Name = name;
            m_card.Suit = suit;
            m_card.State = state;
            SetVisual();
        }

        public void Render()
        {
            setCard();
            SetVisual();
        }

        private void SetVisual()
        {
            setCardSpriteManager();
            m_isRenderComplete = true;

            m_topName.sprite = m_cardSpriteManager.GetNames(((int)m_card.Name));
            m_downName.sprite = m_cardSpriteManager.GetNames(((int)m_card.Name));
            m_topName.color = m_cardSpriteManager.GetColor(m_card.Color);
            m_downName.color = m_cardSpriteManager.GetColor(m_card.Color);

            m_topSuit.sprite = m_cardSpriteManager.GetSuits(((int)m_card.Suit));
            m_downSuit.sprite = m_cardSpriteManager.GetSuits(((int)m_card.Suit));

        }    
    }
}
