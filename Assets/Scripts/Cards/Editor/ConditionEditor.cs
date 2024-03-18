using CardGame.Conditions;
using CardGame.Effects;
using System;
using System.Diagnostics;
using System.Linq;
using Tools;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CardGame
{
    [CustomPropertyDrawer(typeof(Condition), true)]
    public class ConditionEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var valueField = new PropertyField(property.FindPropertyRelative("value"));

            // Add fields to the container.
            container.Add(valueField);

            return container;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            /*// Calculate rects
            var valueRect = new Rect(position.x, position.y, 30, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);*/

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    /*public class ConditionEditor : Editor
    {
        SerializedProperty effects;

        private Type[] conditionTypes;
        private static string[] options;
        private int _lastSelected;

        void OnEnable()
        {
            conditionTypes = Utils.GetImplementingClasses(typeof(Condition));
            options = conditionTypes.Select(t => t.ToString()).Prepend("-- Type --").ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("(Condition Editor)");
            serializedObject.ApplyModifiedProperties();
        }
    }*/
}