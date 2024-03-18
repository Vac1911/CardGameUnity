using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralNoise;
using Color = UnityEngine.Color;

namespace CardGame
{
    using Extensions;
    using System.Linq;
    using Unity.Mathematics;
    using UnityEngine.TextCore.Text;
    using UnityEngine.UIElements;

    public class EncounterGenerator : MonoBehaviour
    {

        public int width = 64;
        public int height = 64;
        public int octaves = 4;
        public float frequency = 20f;

        public uint seed = 0;
        Random rng;
        
        /*public float amplitude = 1f;*/

        public int levels = 3;
        public AnimationCurve curve;

        private void Start()
        {
            if (seed == 0)
            {
                seed = (uint)(UnityEngine.Random.value * 1000000);
            }

            rng = new Random(seed);

            float[,] map = GenerateArray();

            var buildings = GenerateBuildings();

            var texture = CreateVoronoiTexture(buildings.Select(b => b.center).ToList());
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width-1, height-1), new Vector2(0.5f, 0.5f), 8f);
            GameObject newObj = new GameObject();
            var renderer = newObj.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }

        public Texture2D CreateTexture(float[,] map, Color[] colors)
        {
            var texture = new Texture2D(map.GetUpperBound(0), map.GetUpperBound(1), TextureFormat.RGBA32, false);

            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    var colorIndex = map[x, y];
                    texture.SetPixel(x,y, new Color(colorIndex, colorIndex, colorIndex));
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
        }

        public Texture2D CreateVoronoiTexture(List<Vector2> points)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 currentPoint = new Vector2(x, y);
                    Vector2 closestPoint = points.MinBy(p => Vector2.Distance(currentPoint, p));
                    var colorIndex = (float)points.IndexOf(closestPoint) / points.Count;
                    texture.SetPixel(x, y, new Color(colorIndex, colorIndex, colorIndex));
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;

        }

        public float[,] GenerateArray()
        {
            float[,] map = new float[width, height];
            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    map[x, y] = 1f;
                }
            }

            return map;
        }

        public float[,] GenerateArrayNoise()
        {
            float[,] map = new float[width, height];
            var noise = new VoronoiNoise((int)seed, 20);
            /*noise.Amplitude = amplitude;
            noise.Frequency = frequency;*/
            FractalNoise fractal = new FractalNoise(noise, octaves, frequency);

            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    float fx = x / (width - 1.0f);
                    float fy = y / (height - 1.0f);
                    float value = fractal.Sample2D(fx, fy);

                    map[x, y] = value;
                }
            }

            NormalizeArray(map);

            Posterize(map);

            return map;
        }

        protected List<RectInt> GenerateBuildings(int number = 5)
        {
            List<RectInt> buildings = new List<RectInt>();
            for(int i = 0; i < number; i++)
            {
                var nextBuilding = GenerateBuilding();
                if(buildings.All(b => !b.Overlaps(nextBuilding))) {
                    buildings.Add(nextBuilding);
                }
            }

            return buildings;
        }

        protected RectInt GenerateBuilding(int minSize = 4, int maxSize = 8)
        {
            Vector2Int size = new Vector2Int(rng.NextInt(minSize, maxSize + 1), rng.NextInt(minSize, maxSize + 1));
            Vector2Int center = new Vector2Int(rng.NextInt(0, width - size.x), rng.NextInt(0, height - size.y));

            return new RectInt(center, size);
        }

        protected float[,] RenderBuilding(float[,] arr, RectInt building)
        {
            // Make Floor
            for (int x = building.xMin; x <= building.xMax; x++)
            {
                for (int y = building.yMin; y <= building.yMax; y++)
                {
                    arr[x, y] = 0.5f;
                }
            }

            // Make Walls
            for (int x = building.xMin; x <= building.xMax; x++)
            {
                arr[x, building.yMin] = 1;
                arr[x, building.yMax] = 1;
            }
            for (int y = building.yMin; y <= building.yMax; y++)
            {
                arr[building.xMin, y] = 1;
                arr[building.xMax, y] = 1;
            }
            return arr;
        }

        protected float[,] Blockify(float[,] arr, int size)
        {
            int xBlocks = (int)Math.Ceiling(arr.GetLength(0) / (float)size);
            int yBlocks = (int)Math.Ceiling(arr.GetLength(1) / (float)size);
            float[,] map = new float[xBlocks, yBlocks];

            for (int x = 0; x <= arr.GetUpperBound(0); x += size)
            {
                for (int y = 0; y <= arr.GetUpperBound(1); y += size)
                {
                    var slice = Slice2D(arr, x, x + size, y, y + size);
                    var mode = Flatten(slice).GroupBy(item => item)
                       .OrderByDescending(group => group.Count())
                       .First().Key;
                    Debug.Log(mode);
                    map[x / size, y / size] = mode;
                }
            }

            return map;
        }

        protected float[,] Slice2D(float[,] arr, int xStart, int xEnd, int yStart, int yEnd)
        {
            float[,] slice = new float[xEnd - xStart, yEnd - yStart];

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    if(x < arr.GetLength(0) && y < arr.GetLength(1))
                    slice[x - xStart, y - yStart] = arr[x,y];
                }
            }

            return slice;
        }

        protected void Posterize(float[,] arr)
        {
            NormalizeArray(arr);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float value = arr[x, y];
                    arr[x, y] = 1 - curve.Evaluate(Mathf.Floor(value * levels) / ((float)levels - 1));
                }
            }
        }

        protected void NormalizeArray(float[,] arr)
        {

            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    float v = arr[x, y];
                    if (v < min) min = v;
                    if (v > max) max = v;

                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float v = arr[x, y];
                    arr[x, y] = (v - min) / (max - min);
                }
            }
        }

        public static T[] Flatten<T>(T[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            T[] flattened = new T[width * height];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    flattened[j * width + i] = input[i, j];

                }
            }

            return flattened;
        }

    }
}
