using GameField;
using Cards;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private FieldController m_field;
    [SerializeField] private CardDeck m_deck;

    [SerializeField] private CardPlace[] cardPlaces;
    [SerializeField] private CardPlace m_bankPlace;
    [SerializeField] private CardPlace m_activePlace;

    private CardController[] m_cards;
    private int[] m_cardIndex = new int[1];

    public void StartGame()
    {
        Clear();
        m_deck.CreateNewDeck(CardDeck.CardDeckType.SOLITAIRE50);
        m_cards = m_field.GetComponentsInChildren<CardController>();
        CreateRandomIndex();
        SetParents();
        OpenLastCards();
    }

    private void CreateRandomIndex()
    {
        m_cardIndex = new int[m_cards.Length];

        for (int i = 0; i < m_cardIndex.Length; i++)
            m_cardIndex[i] = i;

        for (int i = 0; i < m_cardIndex.Length; i++)
        {
            int newIndex = Random.Range(0, m_cards.Length);
            int swap = m_cardIndex[newIndex];
            m_cardIndex[newIndex] = m_cardIndex[i];
            m_cardIndex[i] = swap;
        }
    }

    private void ShowCards()
    {
        const float m_screenBorderOffset = 1.2f;
        const float m_rowsOffset = -1f;
        const int m_rows = 4;

        Vector2 screen = GetScreen();
        screen.x -= m_screenBorderOffset;

        float cardOnRow = m_cards.Length / m_rows;
        float screenStep = (screen.x * 2) / (cardOnRow - 1);

        for (int i = 0; i < m_cards.Length; i++)
        {
            float posX = screenStep * (i % cardOnRow) - screen.x;
            float posY = (Mathf.Floor(i / cardOnRow) + 1) * m_rowsOffset;
            m_cards[m_cardIndex[i]].SetNewPos(new Vector3(posX, posY, i * (-0.1f)));
        }
    }

    private void SetParents()
    {
        for (int i = 0; i < m_cards.Length - 1; i++)
        {
            if (m_cards[m_cardIndex[i]].additionalInformation == "bank")
            {
                m_bankPlace.setChild(m_cards[m_cardIndex[i]]);
                continue;
            }
            else
            {
                int cardPlaceIndex = Convert.ToInt16(m_cards[m_cardIndex[i]].additionalInformation);
                cardPlaces[cardPlaceIndex].setChild(m_cards[m_cardIndex[i]]);
            }
        }

        m_activePlace.setChild(m_cards[m_cardIndex[m_cards.Length - 1]]);
        m_cards[m_cardIndex[m_cards.Length - 1]].additionalInformation = "active";
    }

    private void OpenLastCards()
    {
        foreach (CardPlace cardPlace in cardPlaces)
        {
            cardPlace.OpenLast();
        }
        m_activePlace.OpenLast();
    }

    private void Clear()
    {
        if (m_cards == null)
        {
            return;
        }

        foreach (CardController card in m_cards)
        {
            Destroy(card.gameObject);
        }

        foreach (CardPlace cardPlace in cardPlaces)
        {
            cardPlace.Clear();
        }

        m_bankPlace.Clear();
        m_activePlace.Clear();

        m_cards = null;
    }

    private Vector2 GetScreen()
    {
        Camera camera = UnityEngine.Camera.main;

        return new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);
    }

    public int GetSizeOfCardPlace()
    {
        return cardPlaces.Length;
    }

    public bool isCardOnActivePlace(CardController cardController)
    {
        return cardController.place == m_activePlace;
    }

    public bool isCardOnBankPlace(CardController cardController)
    {
        return cardController.place == m_bankPlace;
    }

    public bool isCardsCanMerging(CardController cardController1, CardController cardController2)
    {
        if (cardController1 == null || cardController2 == null)
        {
            return false;
        }

        if (isCardOnActivePlace(cardController1) && isCardOnActivePlace(cardController2))
        {
            return false;
        }

        if (!(isCardOnActivePlace(cardController1) || isCardOnActivePlace(cardController2)))
        {
            return false;
        }

        if (isCardOnBankPlace(cardController1) || isCardOnBankPlace(cardController2))
        {
            return true;
        }
        
        return Mathf.Abs(cardController1.Name - cardController2.Name) == 1;
    }

    public void MergingCards(CardController cardController1, CardController cardController2)
    {
        if (isCardsCanMerging(cardController1, cardController2))
        {
            if (isCardOnActivePlace(cardController1))
            {
                m_activePlace.SetNewPlace(cardController2);
            }
            else
            {
                m_activePlace.SetNewPlace(cardController1);
            }
            cardController1.PutCard();
            cardController2.PutCard();
            WinTest();
        }
    }

    private void WinTest()
    {
        bool flag = true;

        foreach (CardPlace cardPlace in cardPlaces)
        {
            if (cardPlace.isComplete)
            {
                flag = false;
            }
        }

        if (flag)
        {
            Debug.Log("You WIN!)");
        }
    }
}
