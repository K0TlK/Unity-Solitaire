using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlace : MonoBehaviour
{
    [SerializeField] private string m_info = "";
    List<CardController> m_cards = new List<CardController>();
    public int indexLastCard
    {
        get { return m_cards.Count - 1; }
    }
    public void setChild(CardController card)
    {
        AddCard(card);
        m_cards[indexLastCard].PutCard(this);
    }

    public void Clear()
    {
        m_cards = new List<CardController>();
    }
     
    public virtual void OnClick()
    {
        Debug.Log("CardPlace");
    }

    public void AddCard(CardController card)
    {
        m_cards.Add(card);
        m_cards[indexLastCard].transform.SetParent(transform);
        m_cards[indexLastCard].additionalInformation = m_info;
    }

    public void SetNewPlace(CardController card)
    {
        card.place.RemoveLast();
        card.place.OpenLast();
        m_cards[indexLastCard].LockCard();
        AddCard(card);
        card.PutCard(this);
    }

    private void RemoveLast()
    {
        m_cards.RemoveAt(indexLastCard);
    }

    public virtual void OpenLast()
    {
        m_cards[indexLastCard].UnlockCard();
    }
}