using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameField
{
    [CustomEditor(typeof(FieldController))]
    public class EditorFieldController : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            FieldController field = (FieldController)target;

            if (GUILayout.Button("empty"))
            {
                
            }
        }
    }
}
