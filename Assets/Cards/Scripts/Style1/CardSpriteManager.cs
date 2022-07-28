using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpriteManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> m_cardSuits = new List<Sprite>();
    [SerializeField] private List<Sprite> m_cardNames = new List<Sprite>();


    [SerializeField] private Color m_blackColor = Color.black;
    [SerializeField] private Color m_redColor = Color.red;

    public Sprite GetSuits(int index)
    {
        index--;
        if (index < 0 || index >= m_cardSuits.Count)
        {
            Debug.LogError($"GetSuits. Out of range: Size={m_cardSuits.Count} > yourIndex: {index + 1} fixIndex: {index}> 0");
        }

        return m_cardSuits[index];
    }

    public Sprite GetNames(int index)
    {
        index--;
        if (index < 0 || index >= m_cardNames.Count)
        {
            Debug.LogError($"GetNames. Out of range: Size={m_cardSuits.Count} > yourIndex: {index + 1} fixIndex: {index} > 0");
        }

        return m_cardNames[index];
    }

    public Color GetColor(Card.CardColor cardColor)
    {
        if (cardColor == Card.CardColor.BLACK)
        {
            return m_blackColor;
        }
        
        if (cardColor == Card.CardColor.RED)
        {
            return m_redColor;
        }

        Debug.LogError("GetColor: cardColor is None");
        return Color.white;
    }
}
