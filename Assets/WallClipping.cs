using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CardGame
{
    [RequireComponent(typeof(GridTransform))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(SpriteMask))]
    public class WallClipping : MonoBehaviour
    {
        public Material shadowMaterial;
        public Sprite maskF;
        public Sprite maskR;
        public Sprite maskR2;
        public Sprite maskL;
        public Sprite maskL2;
        GridTransform gridTransform;
        SpriteRenderer spriteRenderer;
        SpriteMask spriteMask;

        GameObject shadow;
        SpriteRenderer shadowRenderer;
        SpriteMask shadowMask;

        // Start is called before the first frame update
        void Start()
        {
            gridTransform = GetComponent<GridTransform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteMask = GetComponent<SpriteMask>();

            shadow = new GameObject("Shadow");
            shadow.transform.SetParent(transform, false);
            shadow.transform.localPosition = Vector3.zero;
            shadowRenderer = shadow.AddComponent<SpriteRenderer>();
            shadowMask = shadow.AddComponent<SpriteMask>();

            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            shadowRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            shadowRenderer.sprite = spriteRenderer.sprite;
            shadowRenderer.material = shadowMaterial;

            UpdateShadow();

            gridTransform.OnMove += (GridTransform obj) => UpdateShadow();
        }

        void UpdateShadow()
        {

            var walls = GetAdjaentWalls();

            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            shadowRenderer.enabled = true;

            // Determine appropriate mask
            if (walls[0] && walls[3])
            {
                // Use wallF
                spriteMask.sprite = maskF;
                shadowMask.sprite = maskF;
            }
            else if (walls[0])
            {
                if(walls[2])
                {
                    // Use wallR2
                    spriteMask.sprite = maskR2;
                    shadowMask.sprite = maskR2;
                }
                else
                {
                    // Use wallR
                    spriteMask.sprite = maskR;
                    shadowMask.sprite = maskR;
                }
            }
            else if (walls[3])
            {
                if (walls[1])
                {
                    // Use wallL2
                    spriteMask.sprite = maskL2;
                    shadowMask.sprite = maskL2;
                }
                else
                {
                    // Use wallL
                    spriteMask.sprite = maskL;
                    shadowMask.sprite = maskL;
                }
            }
            else
            {
                spriteMask.sprite = null;
                shadowMask.sprite = null;

                spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                shadowRenderer.enabled = false;
            }
        }

        bool[] GetAdjaentWalls()
        {
            bool wall0 = false;
            bool wall1 = false;
            bool wall2 = false;
            bool wall3 = false;

            // Current Tile
            Tile tile = (Tile)gridTransform.grid.GetWallAtCell(gridTransform.position);
            if (tile != null)
            {
                var wall = new Wall(tile.name);
                if (wall.down)
                {
                    wall0 = true;
                }
                if (wall.left)
                {
                    wall3 = true;
                }
            }

            // Tile below and right
            Tile tile1 = (Tile)gridTransform.grid.GetWallAtCell(gridTransform.position + Vector3Int.down);
            if (tile1 != null)
            {
                var wall = new Wall(tile1.name);
                if (wall.up)
                {
                    wall0 = true;
                }
                if (wall.left)
                {
                    wall1 = true;
                }
            }

            // Tile below
            Tile tile2 = (Tile)gridTransform.grid.GetWallAtCell(gridTransform.position + Vector3Int.left + Vector3Int.down);
            if (tile2 != null)
            {
                var wall = new Wall(tile2.name);
                if (wall.up)
                {
                    wall2 = true;
                }
                if (wall.right)
                {
                    wall1 = true;
                }
            }

            // Tile below and left
            Tile tile3 = (Tile)gridTransform.grid.GetWallAtCell(gridTransform.position + Vector3Int.left);
            if (tile3 != null)
            {
                var wall = new Wall(tile3.name);
                if (wall.down)
                {
                    wall2 = true;
                }
                if (wall.right)
                {
                    wall3 = true;
                }
            }

            return new bool[] { wall0, wall1, wall2, wall3 };
        }
    }
}
