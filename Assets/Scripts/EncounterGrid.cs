using CardGame;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace CardGame
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Grid))]
    public class EncounterGrid : MonoBehaviour
    {
        [HideInInspector]
        public List<GridTransform> transforms;

        [HideInInspector]
        public Tilemap tilemap;

        public Grid layoutGrid
        {
            get { return tilemap.layoutGrid; }
        }

        protected readonly Vector3Int[] neighbourPositions =
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left,
    
            // diagonal neighbours
            Vector3Int.up + Vector3Int.right,
            Vector3Int.up + Vector3Int.left,
            Vector3Int.down + Vector3Int.right,
            Vector3Int.down + Vector3Int.left
        };

        void Awake()
        {
            tilemap = GetComponentInChildren<Tilemap>();
            tilemap.CompressBounds();
        }

        public Vector3Int WorldToCellPosition(Vector3 worldPosition)
        {
            return layoutGrid.WorldToCell(worldPosition);
        }

        public Vector3 CellToWorldPosition(Vector3Int cellPosition)
        {
            return layoutGrid.GetCellCenterWorld(cellPosition);
        }

        public GameObject GetObjectAtCell(Vector3Int cell)
        {
            foreach (var gridTransform in transforms)
            {
                if (gridTransform.position == cell)
                {
                    return gridTransform.gameObject;
                }
            }

            return null;
        }

        public Character GetCharacterAtCell(Vector3Int cell)
        {
            var objAtCell = GetObjectAtCell(cell);
            if (objAtCell == null)
                return null;
            return objAtCell.GetComponent<Character>();
        }

        public List<Character> GetCharacters()
        {
            return transforms.Where(t => t.HasComponent<Character>()).Select(t => t.GetComponent<Character>()).ToList();
        }

        public List<Character> GetCharacters(Team team)
        {
            return GetCharacters().Where(c => c.team.HasFlag(team)).ToList();
        }

        public List<Vector3Int> GetAdjacentPositions(Vector3Int basePosition, bool withDiagonal = true)
        {
            List<Vector3Int> positions = new List<Vector3Int>();

            var neighbors = withDiagonal ? neighbourPositions : neighbourPositions.Take(4);
            foreach (var neighbour in neighbors)
            {
                positions.Add(basePosition + neighbour);
            }

            return positions;
        }

        // Breadth-first search for all possible positions that could be moved to
        public List<Vector3Int> GetMovementPositions(Vector3Int start, int distance)
        {
            HashSet<Vector3Int> positions = new HashSet<Vector3Int>(GetNeighborCellPositions(start));
            for (int d = 1; d < distance; d++)
            {
                var neighbourPositions = positions.SelectMany(p => GetNeighborCellPositions(p))
                    .Where(p => !IsOccupied(p))
                    .ToList();

                positions.UnionWith(neighbourPositions);
            }

            return new List<Vector3Int>(positions);
        }

        public List<Vector3Int> GetNeighborCellPositions(Vector3Int center)
        {
            List<Vector3Int> neighborTiles = new List<Vector3Int>();
            foreach (var neighbourPosition in neighbourPositions)
            {
                Vector3Int position = center + neighbourPosition;

                if (HasCell(position))
                {
                    neighborTiles.Add(position);
                }
            }

            return neighborTiles;
        }

        public bool HasCell(Vector3Int position)
        {
            return tilemap.HasTile(position);
        }

        public bool IsOccupied(Vector3Int position)
        {
            return GetCharacterAtCell(position) != null;
        }



        // see: https://www.geeksforgeeks.org/mid-point-circle-drawing-algorithm/
        public List<Vector3Int> GetCircularCellPositions(Vector3Int center, int r)
        {
            List<Vector3Int> positions = new List<Vector3Int>();
            print(center);
            int x_centre = center.x;
            int y_centre = center.y;

            int x = r;
            int y = 0;

            // Printing the initial point on the
            // axes after translation
            positions.Add(new Vector3Int((x + x_centre), (y + y_centre), 0));

            // When radius is zero only a single
            // point will be printed
            if (r > 0)
            {

                positions.Add(new Vector3Int((-x + x_centre), (-y + y_centre), 0));
                positions.Add(new Vector3Int((y + x_centre), (x + y_centre), 0));
                positions.Add(new Vector3Int((-y + x_centre), (-x + y_centre), 0));
            }

            // Initialising the value of P
            int P = 1 - r;
            while (x > y)
            {
                y++;

                // Mid-point is inside or on the perimeter
                if (P <= 0)
                    P = P + 2 * y + 1;

                // Mid-point is outside the perimeter
                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                // All the perimeter points have already
                // been printed
                if (x < y)
                    break;

                // Printing the generated point and its
                // reflection in the other octants after
                // translation
                positions.Add(new Vector3Int((x + x_centre), (y + y_centre), 0));
                positions.Add(new Vector3Int((-x + x_centre), (y + y_centre), 0));
                positions.Add(new Vector3Int((x + x_centre), (-y + y_centre), 0));
                positions.Add(new Vector3Int((-x + x_centre), (-y + y_centre), 0));
                // If the generated point is on the
                // line x = y then the perimeter points
                // have already been printed
                if (x != y)
                {
                    positions.Add(new Vector3Int((y + x_centre), (x + y_centre), 0));
                    positions.Add(new Vector3Int((-y + x_centre), (x + y_centre), 0));
                    positions.Add(new Vector3Int((y + x_centre), (-x + y_centre), 0));
                    positions.Add(new Vector3Int((-y + x_centre), (-x + y_centre), 0));
                }
            }


            return positions;
        }

        public List<Vector3Int> GetCircularCellPositionsFilled(Vector3Int center, int r)
        {
            List<Vector3Int> positions = new List<Vector3Int>();

            for (int y = -r; y <= r; y++)
            {
                for (int x = -r; x <= r; x++)
                {
                    if (x * x + y * y <= r * r)
                        positions.Add(new Vector3Int((x + center.x), (y + center.y), 0));
                }
            }

            return positions;
        }

        public Dictionary<Vector3Int,PathNode> GetNavGraph()
        {
            Dictionary<Vector3Int, PathNode> graph = new Dictionary<Vector3Int, PathNode>();

            // Iterate through each point in the tilemap bounds
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(pos);
                PathNode node;

                // TODO: Make sure tile is unoccupied
                if (tile)
                {
                    node = new PathNode(pos);
                    graph[pos] = node;
                }
            }

            return graph;
        }
    }
}