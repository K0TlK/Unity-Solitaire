using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardController : MonoBehaviour
    {
        private ICardRender m_cardRender;
        private ICardAnim m_anim;
        [SerializeField] private string m_additionalInformation;
        [SerializeField] private Card m_card;
        [SerializeField] private BoxCollider2D m_collider2D;
        private bool m_dragable = false;
        private CardPlace m_place;
        private int m_index = 0;

        public CardPlace place
        {
            get { return m_place; }
        }
        public int index
        {
            get { return m_index; }
        }

        public Card.CardName Name
        {
            get => m_card.Name;
        }

        public string additionalInformation
        {
            get { return m_additionalInformation; }
            set { m_additionalInformation = value; }
        }

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

            if (m_collider2D == null)
            {
                m_collider2D = GetComponent<BoxCollider2D>();
            }
        }

        public void SetVisual(Card.CardName name, Card.CardSuit suit)
        {
            m_cardRender.Render(name, suit);
        }

        public void LockCard()
        {
            Debug.LogWarning($"{gameObject.name} lock");
            if (m_card.State == Card.CardState.UNLOCK)
            {
                m_card.State = Card.CardState.LOCK;
                m_anim.OnLock();
            }
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

        public void TakeCard()
        {
            m_anim.StopAnim();
            m_collider2D.enabled = false;
        }

        public void PutCard()
        {
            m_anim.OnPut(m_index);
            m_collider2D.enabled = true;
        }

        public void PutCard(CardPlace newPlace)
        {
            m_place = newPlace;
            m_index = m_place.indexLastCard;
            m_anim.OnPut(m_place.indexLastCard);
        }
    }
}
