//For Vector2Int backward compatibility. Before 2017.2.

#if !UNITY_2017_2_OR_NEWER
namespace Termway.Helper
{
    using UnityEngine; 

    [System.Serializable]
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Int(Vector2 v)
        {
            x = Mathf.RoundToInt(v.x);
            y = Mathf.RoundToInt(v.y);
        }

        public static implicit operator Vector2(Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }
    }
}

#if UNITY_EDITOR
namespace Termway.Helper
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(Vector2Int))]
    public class Vector2IntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                Rect remainingPosition = EditorGUI.PrefixLabel(position, label);
                List<SerializedProperty> propertiesChildren = property.Children();

                if (position.height > 16f)
                {
                    position.height = 16f;
                    EditorGUI.indentLevel += 1;
                    remainingPosition = EditorGUI.IndentedRect(position);
                    remainingPosition.y += 18f;
                }

                float halfRemainingPositionWidth = remainingPosition.width / 2;
                EditorGUIUtility.labelWidth = 14f;
                remainingPosition.width /= 2;
                EditorGUI.indentLevel = 0;

                string[] labels = { "X", "Y" };

                for (int i = 0; i < 2; i++)
                {
                    EditorGUI.BeginProperty(remainingPosition, new GUIContent(labels[i]), propertiesChildren[i]);
                    propertiesChildren[i].intValue = EditorGUI.IntField(remainingPosition, new GUIContent(labels[i]), propertiesChildren[i].intValue);
                    EditorGUI.EndProperty();
                    remainingPosition.x += halfRemainingPositionWidth;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Screen.width < 333 ? (16f + 18f) : 16f;
        }
    }
}
#endif

#endif