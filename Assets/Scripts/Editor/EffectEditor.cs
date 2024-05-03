using CardGame.Effects;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

namespace CardGame
{
    /*[CustomPropertyDrawer(typeof(AbstractEffect), true)]
    public class EffectEditor : PropertyDrawer
    {

        private Type[] effectTypes;
        private static string[] options;
        AbstractEffect effect;

        void OnEnable()
        {
            effectTypes = Utils.GetImplementingClasses(typeof(AbstractEffect));
            options = effectTypes.Select(t => t.ToString()).Prepend("-- Type --").ToArray();
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            effect = (AbstractEffect)property.serializedObject;

            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            // Create drawer UI using C#.
            var popup = new UnityEngine.UIElements.PopupWindow();
            popup.text = effect.GetType().ToString();

            popup.Add(new PropertyField(property.FindPropertyRelative("value"), "Value"));
            container.Add(popup);

            // Return the finished UI.
            return container;
        }
    }*/
}
