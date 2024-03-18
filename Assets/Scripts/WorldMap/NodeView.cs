using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGame.Map
{
    public class NodeView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public WorldMap map;
        public Node node;

        public Color defaultColor;
        public Color hoverColor;
        public Color activeColor;

        [HideInInspector]
        public SpriteRenderer spriteRenderer;

        protected bool isHovered = false;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Render();
        }

        // Update is called once per frame
        void Update()
        {
            if (map.currentNode == node)
            {
                spriteRenderer.color = activeColor;
            }
            else
            {
                spriteRenderer.color = isHovered ? hoverColor : defaultColor;
            }
        }

        public void Render()
        {
            if (node == null)
            {
                Debug.Log("cannot render node: null");
                return;
            }

            if (node.nodeEvent == null)
            {
                Debug.Log("cannot render node.nodeEvent: null");
                return;
            }

            if (node.nodeEvent.GetSprite() == null)
            {
                Debug.Log("cannot render node.nodeEvent.GetSprite(): null");
                return;
            }

            spriteRenderer.sprite = node.nodeEvent.GetSprite();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            map.TravelTo(node);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
        }
    }
} 