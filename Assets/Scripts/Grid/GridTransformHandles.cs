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
        private const float arrowSize = 1f;

        private int forwardId, rightId;
        private Vector3 directionForward, directionRight, gridCenter;
        private Transform trans;
        private GridTransform gridTransform;
        private bool cached = false;

        private Vector3 selectedDirection = Vector3.zero;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            gridTransform = target as GridTransform;

            // Render a field for get+set position accessors:
            var positionInput = EditorGUILayout.Vector3IntField("Grid Position", gridTransform.position);
            SetPosition(positionInput);
        }

        // https://gamedev.stackexchange.com/questions/149514/use-unity-handles-for-interaction-in-the-scene-view
        public void OnSceneGUI()
        {
            if (!cached)
            {
                gridTransform = target as GridTransform;
                trans = gridTransform.transform;

                forwardId = GUIUtility.GetControlID(FocusType.Passive);
                rightId = GUIUtility.GetControlID(FocusType.Passive);

                gridCenter = gridTransform.grid.tilemap.GetCellCenterWorld(Vector3Int.zero);
                directionForward = gridTransform.grid.tilemap.GetCellCenterWorld(Vector3Int.up) - gridCenter;
                directionRight = gridTransform.grid.tilemap.GetCellCenterWorld(Vector3Int.right) - gridCenter;

                cached = true;
            }

            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                Vector3 pos = trans.position;

                {
                    if(Event.current.type == EventType.Repaint) Handles.color = selectedDirection == Vector3.up ? Color.yellow : Color.blue;
                    Handles.ArrowHandleCap(forwardId, pos, Quaternion.LookRotation(directionForward.normalized), arrowSize, Event.current.type);
                }

                {
                    if (Event.current.type == EventType.Repaint) Handles.color = selectedDirection == Vector3.right ? Color.yellow : Color.red;
                    Handles.ArrowHandleCap(rightId, pos, Quaternion.LookRotation(directionRight.normalized), arrowSize, Event.current.type);
                }
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                int id = HandleUtility.nearestControl;

                if (id == forwardId) selectedDirection = Vector3.up;
                else if (id == rightId) selectedDirection = Vector3.right;
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