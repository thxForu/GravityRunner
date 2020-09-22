#if UNITY_EDITOR
namespace Termway.Helper
{
    using System;
    using System.Collections.Generic;

    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Simplify model (updateAction) and view (editorAction) actions when there is a value modification for a serialized property of a MonoBehaviour.
    /// It give more flexibility than a OnValueChanged attribute but is more verbose.
    /// 
    /// Example :
    /// 
    /// [CustomEditor(typeof(MonoBehaviourExampleEditor))]
    /// public class MonoBehaviourExampleEditor : Editor
    /// {
    ///    SerializedPropertyManager pm;
    ///    void OnEnable()
    ///    {
    ///         MonoBehaviourExample monoBehaviourExample = (MonoBehaviourExample) target;
    ///         floatProperty = serializedObject.FindProperty(Name.Of(() => monoBehaviourExample.floatProperty));
    ///         pm = new SerializedPropertyManager(serializedObject);
    ///         pm.Add(floatProperty,
    ///             () => monoBehaviourExample.FloatAction(),
    ///             () => floatProperty.floatValue = EditorGUILayout.DelayedFloatField("Target Framerate", floatProperty.floatValue));
    ///    }
    ///    public override void OnInspectorGUI()
    ///    {
    ///         pm.Do();
    ///    }
    ///}   
    /// </summary>
    public class SerializedPropertyManager
    {
        List<PropertyBehaviour> propertiesBehaviour;
        HashSet<Action> postAppliedModifiedActions;
        SerializedObject serializedObject;

        public SerializedPropertyManager(SerializedObject serializedObject)
        {
            postAppliedModifiedActions = new HashSet<Action>();
            propertiesBehaviour = new List<PropertyBehaviour>();
            this.serializedObject = serializedObject;
        }

        public void Add(SerializedProperty serializedProperty,
                Action updateAction = null,
                Action editorAction = null)
        {
            propertiesBehaviour.Add(new PropertyBehaviour(serializedProperty, updateAction, editorAction));
        }

        public void Add(SerializedProperty serializedProperty,
              string displayName,
              Action updateAction = null,
              Action editorAction = null)
        {
            propertiesBehaviour.Add(new PropertyBehaviour(serializedProperty, displayName, updateAction, editorAction));
        }

        public void Do()
        {
            postAppliedModifiedActions.Clear();
            serializedObject.Update();

            foreach (PropertyBehaviour propertyBehaviour in propertiesBehaviour)
            {
                EditorGUI.BeginChangeCheck();

                if (propertyBehaviour.EditorAction == null && propertyBehaviour.SerializedProperty != null)
                    EditorGUILayout.PropertyField(propertyBehaviour.SerializedProperty,
                        new GUIContent(propertyBehaviour.DisplayName), true);

                else if (propertyBehaviour.EditorAction != null)
                    propertyBehaviour.EditorAction.Invoke(); // cant use ?.Invoke due to compatibility issue.

                if ((EditorGUI.EndChangeCheck() || propertyBehaviour.HasBeenUpdated) &&
                        propertyBehaviour.UpdateAction != null &&
                        !postAppliedModifiedActions.Contains(propertyBehaviour.UpdateAction))
                {
                    postAppliedModifiedActions.Add(propertyBehaviour.UpdateAction);
                    propertyBehaviour.CurrentValue = propertyBehaviour.SerializedProperty.Value();        
                } 
            }

            serializedObject.ApplyModifiedProperties();

            foreach (Action postAppliedModifiedAction in postAppliedModifiedActions)
                postAppliedModifiedAction();
        }


        class PropertyBehaviour
        {
            public SerializedProperty SerializedProperty { get; set; }
            public Action EditorAction { get; set; }
            public Action UpdateAction { get; set; }
            public string DisplayName { get; set; }
            public object CurrentValue { get; set; }

            public PropertyBehaviour(SerializedProperty serializedProperty,
                Action updateAction = null,
                Action editorAction = null)

            {
                SerializedProperty = serializedProperty;
                UpdateAction = updateAction;
                EditorAction = editorAction;
                DisplayName = serializedProperty.displayName;
                CurrentValue = SerializedProperty.Value();
            }

            public PropertyBehaviour(SerializedProperty serializedProperty,
                string displayName,
                Action updateAction = null,
                Action editorAction = null) : this(serializedProperty, updateAction, editorAction)
            {
                DisplayName = displayName;
            }

            public bool HasBeenUpdated
            {
                get
                {
                    if (CurrentValue == null)               
                        throw new ArgumentNullException("CurrentValue is null " + SerializedProperty.name + ":" + SerializedProperty.propertyType);

                    if (SerializedProperty.propertyType == SerializedPropertyType.Generic)
                        return !SerializedProperty.IsEquals(CurrentValue);
                    
                    return !CurrentValue.Equals(SerializedProperty.Value());
                }
            }

        }
    }

}



#endif
