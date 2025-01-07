using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ProceduralNoise;

namespace CardGame
{
    using CardGame.UI;
    using Extensions;
    using System.Collections;
    using System.Linq;
    using Tools.UI.Card;
    using Unity.Mathematics;
    using UnityEngine.Tilemaps;

    public class EncounterGenerator : MonoBehaviour
    {

        public int width = 32;
        public int height = 32;
        /*public int octaves = 4;
        public float frequency = 20f;
        public AnimationCurve curve;*/
        public int levels = 3;
        public int buffer = 4;
        public int difficulty = 4;

        public uint seed = 0;
        Random rng;


        public CharacterInfoManager characterInfoManager;
        public UiPlayerHand playerHand;

        public TileBase tile;
        public TileBase roadTile;
        public WallSet wallSet;

        public GameObject gridPrefab;
        public GameObject roofPrefab;

        public GameObject playerPrefab;
        public List<GameObject> enemyPrefabs;

        protected GameObject encounterObj;
        protected EncounterManager encounterManager;
        protected EncounterGrid encounter;

        private void Awake()
        {
            seed = GameState.Instance.seed;
            if (seed == 0)
            {
                throw new Exception("no seed");
                //seed = (uint)(UnityEngine.Random.value * 1000000);
            }

            rng = new Random(seed);
            encounterObj = new GameObject("Encounter");
            encounterManager = encounterObj.AddComponent<EncounterManager>();
            float[,] map = GenerateArray();

            var buildings = CreatedSlicedRects();
            var texture = CreateSlicedTexture(buildings);
            var grid = GenerateGrid(buildings);
            encounterManager.grid = grid;
            SpawnCharacters();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 8f);
            GameObject newObj = new GameObject();
            var renderer = newObj.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;

            InitCamera();
            encounterManager.characterInfoManager = characterInfoManager;
            encounterManager.StartEncounter();
        }

        protected EncounterGrid GenerateGrid(List<RectInt> rects)
        {
            GameObject gridObj = Instantiate(gridPrefab, new Vector3(0, 0, 0), Quaternion.identity, encounterObj.transform);
            encounter = gridObj.GetComponent<EncounterGrid>();

            var floorLayer = encounter.tilemap;
            floorLayer.ClearAllTiles();
            floorLayer.origin = Vector3Int.zero;
            floorLayer.size = new Vector3Int(width, height);

            foreach (RectInt rect in rects)
            {
                // Box fill floors
                floorLayer.BoxFill(new Vector3Int(rect.xMin + 1, rect.yMin + 1, 0), tile, rect.min.x + 1, rect.min.y + 1, rect.max.x - 1, rect.max.y - 1);

                // Create surrounding roads
                for (int x = 0; x <= rect.width; x++)
                {
                    floorLayer.SetTile(new Vector3Int(x + rect.xMin, rect.yMin), roadTile);
                    floorLayer.SetTile(new Vector3Int(x + rect.xMin, rect.yMax), roadTile);
                }
                for (int y = 0; y <= rect.height; y++)
                {
                    floorLayer.SetTile(new Vector3Int(rect.xMin, y + rect.yMin), roadTile);
                    floorLayer.SetTile(new Vector3Int(rect.xMax, y + rect.yMin), roadTile);
                }
            }

            encounter.wallmap.ClearAllTiles();

            foreach (var rect in rects)
            {
                var trimRect = TrimRect(rect);
                float ratio = (float)(trimRect.width + 1) / (trimRect.height + 1);
                if (ratio > 0.5f && ratio < 2f)
                {
                    GenerateBuilding(trimRect);
                }
            }

            floorLayer.RefreshAllTiles();

            return encounter;
        }

        protected RectInt TrimRect(RectInt rect)
        {
            return new RectInt(rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2);
        }

        protected void GenerateBuilding(RectInt rect)
        {
            var wallLayer = encounter.wallmap;

            for (int x = 0; x <= rect.width; x++)
            {
                wallLayer.SetTile(new Vector3Int(x + rect.xMin, rect.yMin), wallSet.WallFR);
                wallLayer.SetTile(new Vector3Int(x + rect.xMin, rect.yMax), wallSet.WallBL);
            }
            for (int y = 0; y <= rect.height; y++)
            {
                wallLayer.SetTile(new Vector3Int(rect.xMin, y + rect.yMin), wallSet.WallFL);
                wallLayer.SetTile(new Vector3Int(rect.xMax, y + rect.yMin), wallSet.WallBR);
            }

            wallLayer.SetTile(new Vector3Int(rect.xMin, rect.yMin), wallSet.WallF);
            wallLayer.SetTile(new Vector3Int(rect.xMax, rect.yMin), wallSet.WallR);
            wallLayer.SetTile(new Vector3Int(rect.xMin, rect.yMax), wallSet.WallL);
            wallLayer.SetTile(new Vector3Int(rect.xMax, rect.yMax), wallSet.WallB);

            wallLayer.RefreshAllTiles();

            GenerateRoof(rect);
        }

        protected void GenerateRoof(RectInt rect)
        {
            GameObject roofObj = Instantiate(roofPrefab, new Vector3(0, 0, 0), Quaternion.identity, encounterObj.transform);
            var roof = roofObj.GetComponent<ProceduralRoof>();
            var gridTransform = roofObj.GetComponent<GridTransform>();
            gridTransform.grid = encounter;
            roof.SetRect(rect);
        }

        public List<RectInt> CreatedSlicedRects()
        {
            var baseRect = new RectInt(0, 0, width - 1, height - 1);
            List<RectInt> rects = new() { baseRect };
            for (int i = 0; i < levels; i++)
            {
                var index = rng.NextInt(rects.Count - 1);
                var rect = rects[index];
                rects.Remove(rect);
                rects.AddRange(SliceRectangle(rect));
            }
            return rects;
        }

        public Texture2D CreateSlicedTexture(List<RectInt> rects)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            var colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.grey, Color.black, new Color(1f, 0.5f, 0) };

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, new UnityEngine.Color(1f, 1f, 1f));
                }
            }

            foreach (var item in rects.Select((x, i) => new { Value = x, Index = i }))
            {
                var rect = item.Value;
                var color = colors[item.Index];
                for (int x = 0; x <= rect.width; x++)
                {
                    texture.SetPixel(x + rect.xMin, rect.yMin, color);
                    texture.SetPixel(x + rect.xMin, rect.yMax, color);
                }
                for (int y = 0; y <= rect.height; y++)
                {
                    texture.SetPixel(rect.xMin, y + rect.yMin, color);
                    texture.SetPixel(rect.xMax, y + rect.yMin, color);
                }

                texture.SetPixel((int)rect.center.x, (int)rect.center.y, color);
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;
        }

        protected List<RectInt> SliceRectangle(RectInt baseRect)
        {
            List<RectInt> rects = new List<RectInt>();
            /*bool IsVertical = rng.NextBool();

            // If we dont have enough room to accomidate our buffer, try slicing the other direction
            if(IsVertical && baseRect.width < buffer * 2)
            {
                IsVertical = false;
            }
            else if (!IsVertical && baseRect.height < buffer * 2)
            {
                IsVertical = true;
            }*/

            bool IsVertical = baseRect.width > baseRect.height;

            int max = IsVertical ? baseRect.width : baseRect.height;

            // If we still dont have enough room, return the original rectangle0
            if (max <= buffer * 2) {
                rects.Add(baseRect);
                return rects;
            }

            int position = rng.NextInt(buffer, max - buffer);

            if (IsVertical)
            {
                position = position + baseRect.xMin;
                RectInt rect1 = new();
                RectInt rect2 = new();
                rect1.SetMinMax(baseRect.min, new Vector2Int(position, baseRect.yMax));
                rect2.SetMinMax(new Vector2Int(position + 1, baseRect.yMin), baseRect.max);

                rects.Add(rect1);
                rects.Add(rect2);
            }
            else
            {
                position = position + baseRect.yMin;
                RectInt rect1 = new();
                RectInt rect2 = new();
                rect1.SetMinMax(baseRect.min, new Vector2Int(baseRect.xMax, position));
                rect2.SetMinMax(new Vector2Int(baseRect.xMin, position + 1), baseRect.max);

                rects.Add(rect1);
                rects.Add(rect2);
            }

            return rects;
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

        public void SpawnCharacters()
        {
            GameObject playerObj = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity, encounterObj.transform);
            Player player = playerObj.GetComponent<Player>();
            player.encounterGrid = encounter;
            player.gridTransform.grid = encounter;
            encounterManager.characters.Add(player);
            playerHand.character = player;

            player.gridTransform.position = new Vector3Int(rng.NextInt(1, width - 1), rng.NextInt(1, height - 1));

            int d = 0;
            while(d < difficulty)
            {
                int index = rng.NextInt(enemyPrefabs.Count - 1);
                GameObject prefab = enemyPrefabs[index];
                encounterManager.characters.Add(SpawnEnemy(prefab));

                // TODO: assign difficulty to enemies and increment `d` based on enemy difficulty
                d++;
            }
        }

        public void InitCamera()
        {
            CameraController cc = Camera.main.GetComponent<CameraController>();
            cc.encounter = encounter.transform.parent.gameObject;
        }

        public Enemy SpawnEnemy(GameObject prefab)
        {
            GameObject enemyObj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, encounterObj.transform);
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.encounterGrid = encounter;
            enemy.gridTransform.grid = encounter;

            enemy.gridTransform.position = new Vector3Int(rng.NextInt(1, width- 1), rng.NextInt(1, height - 1));

            return enemy;
        }
    }
}
