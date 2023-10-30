using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

namespace CardGame.UI
{
    public class UICharacterInfo : MonoBehaviour
    {
        public Character character;
        public float OffsetYScreenFraction = 1f;

        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 uiOffset;

        // Start is called before the first frame update
        void Start()
        {

            this.rectTransform = GetComponent<RectTransform>();
            this.canvas = transform.root.GetComponent<Canvas>();
            character.OnDeathEvent += OnCharacterDeath;
        }

        private void Update()
        {
            FollowCharacterTransform();
        }

        public void FollowCharacterTransform()
        {
            Vector3 screenPoint = WorldToScreenSpace(character.transform.position, Camera.main, this.rectTransform);
            this.rectTransform.anchoredPosition = screenPoint;
        }

        public static Vector2 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;

            /*Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
            {
                return screenPos;
            }*/

            return new Vector2(screenPoint.x, screenPoint.y);
        }

        private void OnDestroy()
        {
            character.OnDeathEvent -= OnCharacterDeath;
        }

        public void OnCharacterDeath(Character character)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}