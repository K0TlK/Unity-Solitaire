using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GameField;

namespace Cards
{
    public class CardDeck : MonoBehaviour
    {
        [SerializeField] private FieldController m_fieldController;

        private void Start()
        {
            CreateNewDeck(CardDeckType.TYPE52);

            if (m_fieldController == null)
            {
                m_fieldController.GetComponent<FieldController>();
            }
        }

        public enum CardDeckType
        {
            TYPE52,
            TYPE36,
            TYPE32,
            TYPE24
        };

        public void CreateNewDeck(CardDeckType type, int seed = 0)
        {
            if (type == CardDeckType.TYPE52)
            {
                NewSimpleDeck();
            }
            else
            {
                Debug.Log("Error");
            }
        }

        private void NewSimpleDeck
            (Card.CardName start = Card.CardName.NUM2,
            Card.CardName end = Card.CardName.ACE)
        {
            List<Card.CardName> names = new List<Card.CardName>();

            StringBuilder logOutput = new StringBuilder();


            int CardCounter = 0;

            for (Card.CardSuit cardSuit = Card.CardSuit.HEARTS; cardSuit <= Card.CardSuit.SPADES; cardSuit++)
            {
                for (Card.CardName cardName = start; cardName <= end; cardName++)
                {
                    CardCounter++;
                    m_fieldController.AddCard(cardName, cardSuit);
                    logOutput.AppendLine($"{CardCounter}) New card: {cardSuit} of {cardName}");
                }
            }

            Debug.Log(logOutput.ToString());
        }
    }
}
