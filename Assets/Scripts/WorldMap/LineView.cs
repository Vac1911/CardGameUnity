using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Map
{
    public class LineView : MonoBehaviour
    {
        public WorldMap map;
        public Connection connection;

        public float offsetFromNodes = 0.1f;
        public int linePointsCount = 2;

        [HideInInspector]
        public LineRenderer lineRenderer;

        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            Render();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Render()
        {
            if (connection == null)
            {
                Debug.Log("cannot render node: null");
                return;
            }

            NodeView from = map.GetNodeView(connection.GetParent(map));
            NodeView to = map.GetNodeView(connection.GetDestination(map));

            var fromPoint = from.transform.position +
                            (to.transform.position - from.transform.position).normalized * offsetFromNodes;

            var toPoint = to.transform.position +
                          (from.transform.position - to.transform.position).normalized * offsetFromNodes;

            // drawing lines in local space:
            this.transform.position = fromPoint;
            lineRenderer.useWorldSpace = false;

            // line renderer with 2 points only does not handle transparency properly:
            lineRenderer.positionCount = linePointsCount;
            for (var i = 0; i < linePointsCount; i++)
            {
                lineRenderer.SetPosition(i,
                    Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float)i / (linePointsCount - 1)));
            }

            /*var dottedLine = this.GetComponent<DottedLineRenderer>();
            if (dottedLine != null) dottedLine.ScaleMaterial();*/
        }
    }
}