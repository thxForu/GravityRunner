using UnityEditor;
using UnityEngine;

namespace TranslucentUI
{
    [CustomEditor(typeof(TranslucentUICameraMobile))]
    [ExecuteInEditMode]
    public class TranslucentUICameraMobileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
        }
    }
}