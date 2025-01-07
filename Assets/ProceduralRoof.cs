using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tools.UI.Card;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace CardGame
{
    [RequireComponent(typeof(GridTransform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ProceduralRoof : MonoBehaviour
    {
        public int pixelsPerUnit = 64;
        public Vector2Int tileSize = new Vector2Int(64, 32);
        protected int overhang = 0;
        protected RectInt rect;
        protected GridTransform gridTransform;
        protected SpriteRenderer spriteRenderer;
        protected Texture2D texture;
        private Vector2Int closeVertex;
        private Vector2Int rightVertex;
        private Vector2Int leftVertex;
        private Vector2Int farVertex;

        // Start is called before the first frame update
        void Start()
        {
            gridTransform = GetComponent<GridTransform>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            Draw();
        }

        public void SetRect(RectInt building)
        {
            this.rect = building;
        }

        public void Draw()
        {
            this.gridTransform.position = new Vector3Int(rect.min.x, rect.min.y, 0);
            this.transform.position -= Vector3.forward * 2f;

            int textureX = (Mathf.Max(rect.width, rect.height) + 2) * tileSize.x;
            int textureY = textureX;

            texture = new Texture2D(textureX, textureY, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;

            Color[] cols = texture.GetPixels();
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i] = Color.clear;
            }
            texture.SetPixels(cols);

            closeVertex = new Vector2Int(texture.width / 2 - 1, tileSize.y / 2);
            rightVertex = closeVertex + tileSize / 2 * (rect.width + 1);
            leftVertex = new Vector2Int(closeVertex.x - tileSize.x / 2 * (rect.height + 1) + 1, closeVertex.y + tileSize.y / 2 * (rect.height + 1));
            farVertex = leftVertex + tileSize / 2 * (rect.width + 1);

            // Add overhang
            closeVertex += Vector2Int.down * overhang;
            rightVertex += Vector2Int.right * overhang * 2;
            leftVertex += Vector2Int.left * overhang * 2;
            farVertex += Vector2Int.up * overhang;

            DrawLine(leftVertex + Vector2Int.one, farVertex);
            DrawLine(closeVertex, leftVertex);
            DrawLine(closeVertex, rightVertex);

            DrawGable();

            texture.Apply();
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, textureX, textureY), new Vector2(0.5f, 0f), pixelsPerUnit);
        }

        public void DrawGable()
        {
            Vector2Int midPoint = (leftVertex + closeVertex) / 2;
            Vector2Int midPointFar = (rightVertex + farVertex) / 2;

            Vector2Int offset = Vector2Int.up * tileSize.y / 4 * (rect.height + 1);
            Vector2Int ridgeStart = midPoint + offset;
            Vector2Int ridgeEnd = midPointFar + offset;

            DrawLine(ridgeStart, leftVertex);
            DrawLine(ridgeStart, closeVertex);
            DrawLine(ridgeStart, ridgeEnd);
            DrawLine(ridgeEnd, rightVertex);
            DrawLine(ridgeEnd, farVertex);

            FloodFill(closeVertex + Vector2Int.up * 2, Color.grey);
            FloodFill(ridgeStart + Vector2Int.up * 2, Color.grey);
            /*FloodFill(ridgeStart + Vector2Int.down * 2, Color.white);*/
        }

        public void DrawHip()
        {
            Vector2Int midPoint = (leftVertex + closeVertex) / 2;
            Vector2Int midPointFar = (rightVertex + farVertex) / 2;

            Vector2Int offset = Vector2Int.up * tileSize.y / 4 * (rect.height + 1);
            Vector2Int ridgeStart = midPoint + offset;
            Vector2Int ridgeEnd = midPointFar + offset;


            DrawLine(ridgeStart, leftVertex);
            DrawLine(ridgeStart, closeVertex);
            DrawLine(ridgeStart, ridgeEnd);
            DrawLine(ridgeEnd, rightVertex);
            DrawLine(ridgeEnd, farVertex);
        }

        public void DrawLine(Vector2Int begin, Vector2Int end)
        {
            var points = GetLine(begin, end);
            foreach(var point in points)
            {
                texture.SetPixel(point.x, point.y, Color.black);
            }
        }

        public List<Vector2Int> GetLine(Vector2Int begin, Vector2Int end)
        {
            var points = new List<Vector2Int> { };
            int x0 = begin.x;
            int y0 = begin.y;
            int x1 = end.x;
            int y1 = end.y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                var point = new Vector2Int(x0, y0);
                points.Add(point);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            return points;
        }

        public void FloodFill(Vector2Int pt, Color color)
        {
            Stack<Vector2Int> pixels = new Stack<Vector2Int>();
            var targetColor = texture.GetPixel(pt.x, pt.y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Vector2Int a = pixels.Pop();
                if (a.x < texture.width && a.x > -1 && a.y < texture.height && a.y > -1)
                {
                    if (texture.GetPixel(a.x, a.y) == targetColor)
                    {
                        texture.SetPixel(a.x, a.y, color);
                        pixels.Push(new Vector2Int(a.x - 1, a.y));
                        pixels.Push(new Vector2Int(a.x + 1, a.y));
                        pixels.Push(new Vector2Int(a.x, a.y - 1));
                        pixels.Push(new Vector2Int(a.x, a.y + 1));
                    }
                }
            }

        }

    }
}
