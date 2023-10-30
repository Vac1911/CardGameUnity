using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [ExecuteInEditMode]
    public class GridTransform : MonoBehaviour
    {
        public EncounterGrid grid;
        public Vector3Int position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                UpdateWorldTransform();
            }
        }

        [SerializeField]
        private Vector3Int _position;

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
        }
    }
}