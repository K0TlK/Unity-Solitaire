using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cards
{
    [CustomEditor(typeof(CardAnimController))]
    public class EditorCardAnimController : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CardAnimController anim = (CardAnimController)target;

            if (GUILayout.Button("Unlock"))
            {
                anim.OnUnlock();
            }

            if (GUILayout.Button("Lock"))
            {
                anim.OnLock();
            }
        }
    }
}

