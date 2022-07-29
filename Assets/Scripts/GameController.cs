using GameField;
using Cards;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private FieldController m_field;
    [SerializeField] private CardDeck m_deck;
    [SerializeField] private CardController[] m_cards;
    private float m_screenBorderOffset = 1.2f;
    private float m_rowsOffset = -1f;
    private int m_rows = 4;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        m_deck.CreateNewDeck(CardDeck.CardDeckType.TYPE52);
        m_cards = m_field.GetComponentsInChildren<CardController>();

        int[] index = new int[m_cards.Length];

        for (int i = 0; i < index.Length; i++)
            index[i] = i;


        int swap = 0;

        for (int i = 0; i < index.Length; i++)
        {
            int newIndex = Random.Range(0, m_cards.Length);
            swap = index[newIndex];
            index[newIndex] = index[i];
            index[i] = swap;
        }


        Vector2 screen = GetScreen();
        screen.x -= m_screenBorderOffset;

        float cardOnRow = m_cards.Length / m_rows;
        float screenStep = (screen.x * 2) / (cardOnRow - 1);

        for (int i = 0; i < m_cards.Length; i++)
        {
            float posX = screenStep * (i % cardOnRow) - screen.x;
            float posY = (Mathf.Floor(i / cardOnRow) + 1) * m_rowsOffset;
            m_cards[index[i]].SetNewPos(new Vector3(posX, posY, i * (-0.1f)));
        }
    }

    private Vector2 GetScreen()
    {
        Camera camera = UnityEngine.Camera.main;

        return new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);
    }
}
