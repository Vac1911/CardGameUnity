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
using Debug = UnityEngine.Debug;

namespace CardGame
{
   /* [CustomPropertyDrawer(typeof(Condition), true)]
    public class ConditionEditor : PropertyDrawer
    {

        private Type[] conditionTypes;
        private static string[] options;

        void OnEnable()
        {
            conditionTypes = Utils.GetImplementingClasses(typeof(Condition));
            options = conditionTypes.Select(t => t.ToString()).Prepend("-- Type --").ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            *//*Debug.Log(property.FindPropertyRelative("value"));*//*

            var valueRect = new Rect(position.x, position.y, 30, position.height);

            EditorGUI.IntField(valueRect, 1);

            EditorGUI.EndProperty();
        }
    }*/

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