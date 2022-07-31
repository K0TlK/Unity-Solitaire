using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GameField;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardDeck : MonoBehaviour
    {

        const int cardsMinCombination = 2;
        const int cardsMaxCombination = 7;
        const int cardsCount = 40;
        const int cardPlace = 4;

        const int cardUpChance = 65;
        const int cardDownChance = 35;
        const int cardUpAndDownChance = 15;

        const Card.CardName minNameIndex = Card.CardName.NUM2;
        const Card.CardName maxNameIndex = Card.CardName.ACE;

        private int m_placeID = 0;

        [SerializeField] private FieldController m_fieldController;
        private StringBuilder m_logOutPut = new StringBuilder();
        private List<int> m_cardsCombination = new List<int>();
        private int m_cardsCounter = 0;

        private void Awake()
        {
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
            TYPE24,
            SOLITAIRE50
        };

        public void CreateNewDeck(CardDeckType type, int seed = 0)
        {
            if (type == CardDeckType.TYPE52)
            {
                NewSimpleDeck();
                return;
            }

            if (type == CardDeckType.SOLITAIRE50)
            {
                NewSolitaireDeck();
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
            for (Card.CardSuit cardSuit = Card.CardSuit.HEARTS; cardSuit <= Card.CardSuit.SPADES; cardSuit++)
            {
                for (Card.CardName cardName = start; cardName <= end; cardName++)
                {
                    m_fieldController.AddCard(cardName, cardSuit);
                }
            }
        }

        private void NewSolitaireDeck()
        {
            var startTime = DateTime.Now;
            m_logOutPut = new StringBuilder($"[{startTime}] Cards on solitaire deck:\n");
            m_cardsCounter = 0;
            m_cardsCombination = new List<int>();

            GenRandomCombination();
            FixCombinationSize();
            GenByCombinationCards();


            TimeSpan timer = DateTime.Now.Subtract(startTime);
            int time = (timer.Hours * 60 * 60) + (timer.Minutes * 60) + timer.Seconds;
            m_logOutPut.AppendLine($"[{DateTime.Now}] Time: {time}");
            Debug.Log(m_logOutPut.ToString());
        }

        private void GenRandomCombination()
        {
            m_logOutPut.AppendLine("----------Start random gen----------");
            do
            {
                int combination = Random.Range(cardsMinCombination, cardsMaxCombination + 1);
                m_cardsCombination.Add(combination);
                m_cardsCounter += combination;
                m_logOutPut.AppendLine($"{m_cardsCombination.Count}) combination len: {m_cardsCombination[m_cardsCombination.Count - 1]}");

            } while (m_cardsCounter <= cardsCount);
            m_logOutPut.AppendLine("----------End random gen----------");
            m_logOutPut.AppendLine();
        }

        private void FixCombinationSize()
        {
            m_logOutPut.AppendLine("----------Start clear----------");
            while (m_cardsCounter > cardsCount)
            {
                int randomIndex = Random.Range(0, m_cardsCombination.Count);

                if (m_cardsCombination[randomIndex] > cardsMinCombination + 1)
                {
                    m_cardsCombination[randomIndex]--;
                    m_cardsCounter--;
                    m_logOutPut.AppendLine($"Remove on: {randomIndex}, now: {m_cardsCombination[randomIndex]}");
                }
                else
                {
                    m_logOutPut.AppendLine($"Removal not successful on: {randomIndex}, now: {m_cardsCombination[randomIndex]}");
                }
            }
            m_logOutPut.AppendLine("----------Clear complete----------");
            m_logOutPut.AppendLine("");
        }

        private void GenByCombinationCards()
        {
            m_logOutPut.AppendLine("----------Start gen main cards----------");

            for (int i = 0; i < m_cardsCombination.Count; i++)
            {
                int typeCombination = Random.Range(0, cardUpChance + cardDownChance + cardUpAndDownChance + 1);
                m_logOutPut.Append($"Combination: {i} have {m_cardsCombination[i]} cards. Type [{typeCombination}]:");

                if (typeCombination > cardUpAndDownChance)
                {
                    int tmp = Random.Range((int)minNameIndex, (int)(maxNameIndex - m_cardsCombination[i] + 1));
                    Card.CardName cardName = (Card.CardName)tmp;

                    if (typeCombination < cardUpChance + cardUpAndDownChance)
                    {
                        m_logOutPut.AppendLine("Up");
                        CreateBankCard(cardName);
                        CreateCardsCombination(cardName, m_cardsCombination[i]);
                    }
                    else
                    {
                        m_logOutPut.AppendLine("Down");
                        CreateBankCard(cardName);
                        CreateCardsCombination(cardName + m_cardsCombination[i], m_cardsCombination[i], false);
                    }
                }
                else
                {
                    int uniqueNames = Mathf.CeilToInt(m_cardsCombination[i] / 2.0f);
                    if (m_cardsCombination[i] % 2 == 0) uniqueNames++;

                    m_logOutPut.AppendLine("UpAndDown uniqueNames: " + uniqueNames.ToString());
                    int tmp = Random.Range((int)minNameIndex, (int)(maxNameIndex - uniqueNames + 1));

                    Card.CardName cardName = (Card.CardName)tmp;
                    CreateBankCard(cardName);
                    CreateCardsCombination(cardName, uniqueNames);
                    CreateCardsCombination(cardName + uniqueNames - 1, uniqueNames - ((m_cardsCombination[i] + 1) % 2) - 1, false);
                }
            }

            m_logOutPut.AppendLine("----------Gen main cards complete----------");
            m_logOutPut.AppendLine($"Card Counter: {m_cardsCounter}");
            m_logOutPut.AppendLine($"Combination Counter: {m_cardsCombination.Count}");
        }

        private void CreateBankCard(Card.CardName startedName)
        {
            Card.CardSuit cardSuit = TakeRandomSuit();
            m_logOutPut.AppendLine($"[bk]Addcard: {startedName}_{cardSuit}");
            m_fieldController.AddCard(startedName, cardSuit, "bank");
        }

        private void SolitaireAddCard(Card.CardName name, string info = "")
        {
            Card.CardSuit cardSuit = TakeRandomSuit();
            m_logOutPut.AppendLine($"Addcard: {name}_{cardSuit}");

            if (info == "") info = m_placeID.ToString();
            m_fieldController.AddCard(name, cardSuit, info);

            m_placeID++;
            if (m_placeID % cardPlace == 0) m_placeID = 0;
        }

        private void CreateCardsCombination(Card.CardName startedName, int count, bool isUp = true)
        {
            if (isUp)
            {
                startedName++;
                for (Card.CardName i = startedName; i < startedName + count - 1; i++)
                {
                    SolitaireAddCard(i);
                }
            }
            else
            {
                startedName--;
                for (Card.CardName i = startedName; (int)i > (int)startedName - count; i--)
                {
                    SolitaireAddCard(i);
                }
            }
        }

        private Card.CardSuit TakeRandomSuit()
        {
            int buffer = Random.Range((int)Card.CardSuit.HEARTS, (int)Card.CardSuit.SPADES + 1);
            return (Card.CardSuit)buffer;
        }
    }
}
