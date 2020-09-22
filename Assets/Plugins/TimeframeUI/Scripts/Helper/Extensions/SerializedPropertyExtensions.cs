#if UNITY_EDITOR
namespace Termway.Helper
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class SerializedPropertyExtensions
    {
        public static object Value(this SerializedProperty sp)
        {
            switch (sp.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.ArraySize:          return sp.intValue;
                case SerializedPropertyType.Boolean:            return sp.boolValue;
                case SerializedPropertyType.Float:              return sp.floatValue;
                case SerializedPropertyType.String:             return sp.stringValue;
                case SerializedPropertyType.Color:              return sp.colorValue;
                case SerializedPropertyType.ObjectReference:    return sp.objectReferenceValue;
                case SerializedPropertyType.Enum:               return new KeyValuePair<int, string>(sp.enumValueIndex, sp.enumNames[sp.enumValueIndex]);
                case SerializedPropertyType.Vector2:            return sp.vector2Value;
                case SerializedPropertyType.Vector3:            return sp.vector3Value;
                case SerializedPropertyType.Vector4:            return sp.vector4Value;
                case SerializedPropertyType.Rect:               return sp.rectValue;
                case SerializedPropertyType.Character:          return (char)sp.intValue;
                case SerializedPropertyType.AnimationCurve:     return sp.animationCurveValue;
                case SerializedPropertyType.Bounds:             return sp.boundsValue;
                case SerializedPropertyType.Gradient:           return sp.GradientValue();
                case SerializedPropertyType.Quaternion:         return sp.quaternionValue;
                case SerializedPropertyType.ExposedReference:   return sp.exposedReferenceValue;
#if UNITY_2017_2_OR_NEWER
                case SerializedPropertyType.FixedBufferSize:    return sp.fixedBufferSize;
                case SerializedPropertyType.Vector2Int:         return sp.vector2IntValue;
                case SerializedPropertyType.Vector3Int:         return sp.vector3IntValue;
                case SerializedPropertyType.RectInt:            return sp.rectIntValue;
                case SerializedPropertyType.BoundsInt:          return sp.boundsIntValue;
#endif
                case SerializedPropertyType.Generic:
                default:
                    return GenericValue(sp);
            }
        }

        /// <summary>
        /// Get the value for a Generic SerializedProperty. The SerializedProperty must be an array and have children.
        /// </summary>
        /// <param name="objectReference">if we accept objectReference element.</param>
        /// <returns>The list of objects from the children or elements of the array.</returns>
        static object GenericValue(SerializedProperty serializedProperty, bool objectReference = false)
        {
            if (serializedProperty.isArray)
            {
                List<object> list = new List<object>();
                for (int i = 0; i < serializedProperty.arraySize; i++)
                {
                    SerializedProperty sp = serializedProperty.GetArrayElementAtIndex(i);
                    if (objectReference || sp.propertyType != SerializedPropertyType.ObjectReference)
                        list.Add(sp.Value());
                }
                return list;
            }
            else if (serializedProperty.hasChildren)
            {
                List<object> list = new List<object>();
                List<SerializedProperty> serializedPropertyChildren = serializedProperty.Children();
                foreach (SerializedProperty sp in serializedPropertyChildren)
                    if (objectReference || sp.propertyType != SerializedPropertyType.ObjectReference)
                        list.Add(sp.Value());

                return list;
            }
            else
                return serializedProperty.GetReflectionValue();
        }

        public static object GetReflectionValue(this SerializedProperty serializedProperty)
        {
            MemberInfo[] membersInfo = serializedProperty.serializedObject.targetObject.GetType().GetMember(serializedProperty.name);
            if (membersInfo.Length == 0)
                return null;

            FieldInfo fieldInfo = membersInfo.First() as FieldInfo;
            return fieldInfo.GetValue(serializedProperty.serializedObject.targetObject);
        }


        /// <summary>
        /// Get all the children of a SerializedProperty. Use SerializeProperty iterator method to do so.
        /// </summary>
        public static List<SerializedProperty> Children(this SerializedProperty serializeProperty)
        {
            List<SerializedProperty> spChildren = new List<SerializedProperty>();
            SerializedProperty serializedPropertySibling = serializeProperty.Copy();
            SerializedProperty serializedPropertyChild = serializeProperty.Copy();

            serializedPropertySibling.NextVisible(false);
            //Stop if there is not more element or when the next element is a sibling and not a child.
            while (serializedPropertyChild.NextVisible(true) && !SerializedProperty.EqualContents(serializedPropertySibling, serializedPropertyChild))
                spChildren.Add(serializedPropertyChild.Copy());

            return spChildren;
        }

        /// <summary>
        /// Test equality for SerializedProperty deep copy value. 
        /// Because SerializedProperty.EqualContents only work on SerializedProperty and SerializedProperty.Copy() do not perform a deep copy of the value but serve as an iterator storage.
        /// </summary>
        public static bool IsEquals(this SerializedProperty serializedProperty, object obj)
        {
            object value = serializedProperty.Value();
            return IsEquals(value, obj);
        }

        static bool IsEquals(object objectA, object objectB)
        {
            if (objectA is List<object> && objectB is List<object>)
            {
                List<object> list = objectA as List<object>;
                List<object> list2 = objectB as List<object>;
                if (list.Count != list2.Count)
                    return false;

                for (int i = 0; i < list.Count; i++)
                    if (!IsEquals(list[i], list2[i]))
                        return false;

                return true;
            }
            else
                return objectA.Equals(objectB);
        }

        /// <summary>
        /// When Unity forgot to put a property a public to a method. ("internal Gradient gradientValue")
        /// </summary>
        public static Gradient GradientValue(this SerializedProperty sp)
        {
            PropertyInfo propertyInfo = typeof(SerializedProperty).GetProperty(
                "gradientValue",
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                typeof(Gradient),
                new System.Type[0],
                null
            );
            if (propertyInfo == null)
                return null;

            Gradient gradientValue = propertyInfo.GetValue(sp, null) as Gradient;
            return gradientValue;
        }
    }
}
#endif