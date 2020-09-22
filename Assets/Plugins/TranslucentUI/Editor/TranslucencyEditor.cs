using UnityEditor;
using UnityEngine;

namespace TranslucentUI
{
    [CustomEditor(typeof(Translucency))]
    [ExecuteInEditMode]
    public class TranslucencyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
        }
    }
}