using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardController : MonoBehaviour
    {
        private ICardRender m_cardRender;
        private ICardAnim m_anim;
        [SerializeField] private Card m_card;

        private void Awake()
        {
            if (m_cardRender == null)
            {
                m_cardRender = GetComponent<ICardRender>();
            }

            if (m_anim == null)
            {
                m_anim = GetComponent<ICardAnim>();
            }

            if (m_card == null)
            {
                m_card = GetComponent<Card>();
            }
        }

        public void SetVisual(Card.CardName name, Card.CardSuit suit)
        {
            m_cardRender.Render(name, suit);
        }

        public void UnlockCard()
        {
            if (m_card.State == Card.CardState.LOCK)
            {
                m_card.State = Card.CardState.UNLOCK;
                m_anim.OnUnlock();
            }
        }

        public void SetNewPos(Vector2 newpos)
        {
            SetNewPos(new Vector3(newpos.x, newpos.y, transform.position.z));
        }

        public void SetNewPos(Vector3 newpos)
        {
            m_anim.OnNewPos(newpos);
        }
    }
}
