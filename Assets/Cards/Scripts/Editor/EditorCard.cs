using UnityEngine;
using UnityEditor;
using Cards;

namespace Cards
{
    [CustomEditor(typeof(Card))]
    public class EditorCard : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Card card = (Card)target;

            GUILayout.Label($"{card.Name} - {card.Suit}\n" +
                $"{card.Color} - {card.State}");

            if (GUILayout.Button("Refresh"))
            {
                Repaint();
            }
        }
    }
}
