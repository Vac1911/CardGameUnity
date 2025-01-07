using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
                return prevPosition;
            }
            set
            {
                prevPosition = value;
                UpdateWorldTransform();
            }
        }

        [ReadOnly]
        public Vector3Int prevPosition;

        protected void Awake()
        {
            Debug.Log(gameObject.name + " awake");
            if (grid == null)
                return;

            grid.transforms.Add(this);
            UpdateWorldTransform();
        }

        protected void OnEnable()
        {
            if(grid == null)
                return;

            grid.transforms.Add(this);
            UpdateWorldTransform();
        }

        protected void OnDestroy()
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