using UnityEditor;
using UnityEngine;

namespace TranslucentUI
{
    [CustomEditor(typeof(TranslucentUICamera))]
    [ExecuteInEditMode]
    public class TranslucentUICameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
        }
    }
}