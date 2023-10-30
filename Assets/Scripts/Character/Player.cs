using CardGame.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CardGame
{
    public class Player : Character
    {
        public GameObject positionMarkerPrefab;

        protected List<MovementMarker> positionMarkers = new List<MovementMarker>();

        // Get the target position for an effect
        public override Task<Vector3Int> GetEffectTarget(IEffect effect, List<Vector3Int> positions)
        {
            TaskCompletionSource<Vector3Int> tcs1 = new TaskCompletionSource<Vector3Int>();
            Task<Vector3Int> t1 = tcs1.Task;

            CreateMarkers(positions);

            foreach (var marker in positionMarkers)
            {
                marker.SetResponse(() =>
                {
                    ClearMarkers();
                    tcs1.SetResult(marker.position);
                    return true;
                });

                marker.SetOnHover(() =>
                {
                    ShowEffectArea(effect, marker.position);
                    return true;
                });
            }

            return t1;
        }

        public void CreateMarkers(List<Vector3Int> positions)
        {
            foreach (var position in positions)
            {
                GameObject gameObject = Instantiate(positionMarkerPrefab, grid.GetCellCenterWorld(position) - Vector3Int.forward, Quaternion.identity);
                MovementMarker marker = gameObject.GetComponent<MovementMarker>();
                marker.position = position;
                positionMarkers.Add(marker);
            }
        }

        protected void ShowEffectArea(IEffect effect, Vector3Int position)
        {
            var area = effect.GetAreaOfEffect(this, position);

            foreach (var marker in positionMarkers)
            {
                if (area.Contains(marker.position))
                {
                    marker.ShowSelected();
                }
                else
                {
                    marker.ShowDefault();
                }
            }
        }

        protected void ClearMarkers()
        {
            foreach (var marker in positionMarkers)
            {
                Destroy(marker.gameObject);
            }
            positionMarkers.Clear();
        }
    }
}