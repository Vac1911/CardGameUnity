using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CardGame.Effects;
using Tools;
using System;
using System.Linq;

namespace CardGame
{
    [CustomEditor(typeof(Card))]
    public class CardEditor : Editor
    {
        SerializedProperty effects;

        private Type[] effectTypes;
        private static string[] options;
        private int _lastSelected;

        void OnEnable()
        {
            effectTypes = Utils.GetImplementingClasses(typeof(IEffect));
            options = effectTypes.Select(t => t.ToString()).Prepend("-- Type --").ToArray();

            effects = serializedObject.FindProperty("effects");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            /*
            EditorGUILayout.PropertyField(effects);
            serializedObject.ApplyModifiedProperties();*/


            EditorGUI.BeginChangeCheck(); //we want to see if the popup had a selection made
            _lastSelected = EditorGUILayout.Popup("Add Effect", _lastSelected, options);
            if (EditorGUI.EndChangeCheck() && _lastSelected > 0)
            {
                Type ObjectType = effectTypes[_lastSelected - 1];
                IEffect newEffect = Activator.CreateInstance(ObjectType) as IEffect;
                var card = this.target as Card;
                card.effects.Add(newEffect);
                _lastSelected = 0;
            }
        }
    }
}
