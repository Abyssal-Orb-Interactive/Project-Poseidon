using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Ships
{
    public class ShipVisualizer : MonoBehaviour
    {
        [SerializeField] private const int DEFEATED_SHIPS_LAYER = 8;
        [SerializeField] private Transform _shipHolder;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);
        [SerializeField] private Vector2 _cellSize = new(10f, 10f);
        private Dictionary<Vector2Int, GameObject> _shipsSegmentsVisuals;
        

        private void OnValidate()
        {
            _shipHolder ??= GetComponent<Transform>();
        }
        
        public void VisualizeShips(IEnumerable<IReadonlyLogicalRepresentation> ships)
        {
            _shipsSegmentsVisuals ??= new Dictionary<Vector2Int, GameObject>();
            
            foreach (var ship in ships)
            {
                var prefab = ship.GetGraphicsRepresentation();
                foreach (var coord in ship.SegmentsCoords)
                {
                    _shipsSegmentsVisuals.Add(coord, Instantiate(prefab, new Vector3(coord.x * _cellSize.x + _fieldOffset.x, 0, coord.y * _cellSize.y + _fieldOffset.y),
                        Quaternion.identity, _shipHolder));
                }
            }
        }

        public void VisualizeHit(Vector2Int coord)
        {
            if(!_shipsSegmentsVisuals.ContainsKey(coord)) return;
            _shipsSegmentsVisuals[coord].layer = DEFEATED_SHIPS_LAYER;
        }

        public void CleanVisual()
        {
            foreach (var segment in _shipsSegmentsVisuals.Values)
            {
                segment.SetActive(false);
                Destroy(segment);
            }

            _shipsSegmentsVisuals = null;
        }

        private void OnDestroy()
        {
            CleanVisual();
            _shipHolder = null;
        }
    }
}
