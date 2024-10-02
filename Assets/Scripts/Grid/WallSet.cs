using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Create Wall Set")]
    public class WallSet : ScriptableObject
    {
        public TileBase WallB;
        public TileBase WallBL;
        public TileBase WallBR;
        public TileBase WallF;
        public TileBase WallFL;
        public TileBase WallFR;
        public TileBase WallL;
        public TileBase WallR;
    }
}
