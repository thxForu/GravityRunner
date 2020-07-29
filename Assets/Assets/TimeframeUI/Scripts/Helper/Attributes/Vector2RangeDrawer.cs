#if UNITY_EDITOR
namespace Termway.Helper
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(Vector2RangeAttribute))]
    public class Vector2RangeAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = fieldInfo.GetTooltip();

            Vector2RangeAttribute vector2Range = attribute as Vector2RangeAttribute;
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                EditorGUI.BeginChangeCheck();
                
                Vector2 vector2Value = EditorGUI.Vector2Field(position, label, property.vector2Value);
                if (EditorGUI.EndChangeCheck())
                    property.vector2Value = vector2Range.Clamp(vector2Value);

            }
            else
                Debug.LogError(typeof(Vector2RangeAttribute).Name + " must be only use for " + typeof(Vector2).Name);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {   
            return Screen.width < 333 ? (16f + 18f) : 16f; 
        }
    }

    [CustomPropertyDrawer(typeof(Vector2IntRangeAttribute))]
    public class Vector2IntRangeAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = fieldInfo.GetTooltip();
            Vector2IntRangeAttribute vector2IntRange = attribute as Vector2IntRangeAttribute;
#if UNITY_2017_2_OR_NEWER
            if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                EditorGUI.BeginChangeCheck();
                Vector2Int vector2IntValue = EditorGUI.Vector2IntField(position, label, property.vector2IntValue);
                if (EditorGUI.EndChangeCheck())
                    property.vector2IntValue = vector2IntRange.Clamp(vector2IntValue);
            }
#else
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
                string[] tooltips = vector2IntRange.ToStrings();

                for (int i = 0; i < 2; i++)
                {
                    EditorGUI.BeginProperty(remainingPosition, new GUIContent(labels[i], tooltips[i]), propertiesChildren[i]);
                    EditorGUI.BeginChangeCheck();
                    int value = EditorGUI.IntField(remainingPosition, new GUIContent(labels[i], tooltips[i]), propertiesChildren[i].intValue);
                    if (EditorGUI.EndChangeCheck())
                        propertiesChildren[i].intValue = vector2IntRange.Clamp(i, value);
                    EditorGUI.EndProperty();
                    remainingPosition.x += halfRemainingPositionWidth;
                }           
            }
#endif
            else
                Debug.Log(typeof(Vector2IntRangeAttribute).Name + " must be only use for " + typeof(Vector2Int).Name);
        }

        // https://catlikecoding.com/unity/tutorials/editor/custom-list/ for multiline ident
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Screen.width < 333 ? (16f + 18f) : 16f;
        }
    }

}
#endif
