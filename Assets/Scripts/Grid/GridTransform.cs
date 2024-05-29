using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [ExecuteInEditMode]
    public class GridTransform : MonoBehaviour
    {
        public EncounterGrid grid;
        public event Action<GridTransform> OnMove = GridTransform => { };
        public Vector3Int position
        {
            get
            {
                return _privatePosition;
            }
            set
            {
                _privatePosition = value;
                UpdateWorldTransform();
            }
        }

        [ReadOnly]
        private Vector3Int _privatePosition;

        void OnEnable()
        {
            grid.transforms.Add(this);
        }

        void OnDestroy()
        {
            grid.transforms.Remove(this);
        }

        protected void UpdateWorldTransform()
        {
            transform.position = grid.CellToWorldPosition(position);
            OnMove(this);
        }
    }
}