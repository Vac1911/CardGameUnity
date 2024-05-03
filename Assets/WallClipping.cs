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
        GridTransform gridTransform;
        SpriteRenderer spriteRenderer;
        SpriteMask spriteMask;
        SpriteRenderer shadowRenderer;
        GameObject shadow;

        // Start is called before the first frame update
        void Start()
        {
            gridTransform = GetComponent<GridTransform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteMask = GetComponent<SpriteMask>();

            shadow = new GameObject();
            shadow.transform.SetParent(transform, false);
            shadow.transform.localPosition = new Vector3(0,0,1.5f);
            shadowRenderer = shadow.AddComponent<SpriteRenderer>();
            UpdateShadow();         
        }

        void UpdateShadow()
        {
            TileBase tile = gridTransform.grid.GetWallAtCell(gridTransform.position);
            Debug.Log(tile);

        }
    }
}
