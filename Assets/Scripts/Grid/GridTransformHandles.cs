using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;
using static UnityEngine.GraphicsBuffer;
using Color = UnityEngine.Color;

namespace CardGame
{
    [CustomEditor(typeof(GridTransform))]
    public class GridTransformHandles : Editor
    {
        private const int arrowSize = 2;
        private const float CenterOffset = 3f;

        private int leftId, rightId, forwardId, backId;
        private Transform trans;
        private bool cached = false;

        private Vector3 selectedDirection = Vector3.zero;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var t = target as GridTransform;

            // Render a field for get+set position accessors:
            var positionInput = EditorGUILayout.Vector3IntField("Grid Position", t.position);
            SetPosition(positionInput);
        }

        // https://gamedev.stackexchange.com/questions/149514/use-unity-handles-for-interaction-in-the-scene-view
        public void OnSceneGUI()
        {
            if (!cached)
            {
                var t = target as GridTransform;
                trans = t.transform;

                leftId = GUIUtility.GetControlID(FocusType.Passive);
                rightId = GUIUtility.GetControlID(FocusType.Passive);
                forwardId = GUIUtility.GetControlID(FocusType.Passive);
                backId = GUIUtility.GetControlID(FocusType.Passive);

                cached = true;
            }


            if (Event.current.type == EventType.Repaint)
            {
                {
                    Vector3 pos = trans.position + Vector3.left * CenterOffset;
                    Handles.color = selectedDirection == Vector3.left ? Color.magenta : Color.yellow;
                    Handles.ArrowHandleCap(leftId, pos, Quaternion.LookRotation(Vector3.left), arrowSize, EventType.Repaint);
                }

                {
                    Vector3 pos = trans.position + Vector3.right * CenterOffset;
                    Handles.color = selectedDirection == Vector3.right ? Color.magenta : Color.red;
                    Handles.ArrowHandleCap(rightId, pos, Quaternion.LookRotation(Vector3.right), arrowSize, EventType.Repaint);
                }

                {
                    Vector3 pos = trans.position + Vector3.up * CenterOffset;
                    Handles.color = selectedDirection == Vector3.up ? Color.magenta : Color.blue;
                    Handles.ArrowHandleCap(forwardId, pos, Quaternion.LookRotation(Vector3.up), arrowSize, EventType.Repaint);
                }

                {
                    Vector3 pos = trans.position + Vector3.down * CenterOffset;
                    Handles.color = selectedDirection == Vector3.down ? Color.magenta : Color.cyan;
                    Handles.ArrowHandleCap(backId, pos, Quaternion.LookRotation(Vector3.down), arrowSize, EventType.Repaint);
                }
            }
            else if (Event.current.type == EventType.Layout)
            {
                {
                    Vector3 pos = trans.position + Vector3.left * CenterOffset;
                    Handles.ArrowHandleCap(leftId, pos, Quaternion.LookRotation(Vector3.left), arrowSize, EventType.Layout);
                }

                {
                    Vector3 pos = trans.position + Vector3.right * CenterOffset;
                    Handles.ArrowHandleCap(rightId, pos, Quaternion.LookRotation(Vector3.right), arrowSize, EventType.Layout);
                }

                {
                    Vector3 pos = trans.position + Vector3.up * CenterOffset;
                    Handles.ArrowHandleCap(forwardId, pos, Quaternion.LookRotation(Vector3.up), arrowSize, EventType.Layout);
                }

                {
                    Vector3 pos = trans.position + Vector3.down * CenterOffset;
                    Handles.ArrowHandleCap(backId, pos, Quaternion.LookRotation(Vector3.down), arrowSize, EventType.Layout);
                }
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                int id = HandleUtility.nearestControl;

                if (id == leftId) selectedDirection = Vector3.left;
                else if (id == rightId) selectedDirection = Vector3.right;
                else if (id == forwardId) selectedDirection = Vector3.up;
                else if (id == backId) selectedDirection = Vector3.down;
                else selectedDirection = Vector3.zero;
            }
        }

        // Set the position of GridTransform.
        protected void SetPosition(Vector3Int nextPosition)
        {
            var t = target as GridTransform;
            // Stop if nextPosition is the same as the current position
            if (nextPosition == t.position) return;

            // Record the change to the object. Allows the user to undo their change and marks scene dirty after the change.
            Undo.RecordObject(target, "Moved " + t.gameObject.name);

            t.position = nextPosition;
        }
    }
}