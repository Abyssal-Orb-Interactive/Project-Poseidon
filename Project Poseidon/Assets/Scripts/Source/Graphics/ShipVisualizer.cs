using System.Collections.Generic;
using Source.Ships;
using UnityEngine;

namespace Source.Graphics
{
    public class ShipVisualizer : GameObjectsVisualizer<Dictionary<Vector2Int, GameObject>>
    {
        [SerializeField] private const int DEFEATED_SHIPS_LAYER = 8;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);
        [SerializeField] private Vector2 _cellSize = new(10f, 10f);
        private Dictionary<Vector2Int, GameObject> _shipsSegmentsVisuals;
        
        public void VisualizeShips(IEnumerable<IReadonlyLogicalRepresentation> ships)
        {
            _shipsSegmentsVisuals ??= new Dictionary<Vector2Int, GameObject>();
            
            foreach (var ship in ships)
            {
                var prefab = ship.GetGraphicsRepresentation();
                foreach (var coord in ship.SegmentsCoords)
                {
                    _shipsSegmentsVisuals.Add(coord, Instantiate(prefab, new Vector3(coord.x * _cellSize.x + _fieldOffset.x, 0, coord.y * _cellSize.y + _fieldOffset.y),
                        Quaternion.identity, _container));
                }
            }
        }

        public void VisualizeHit(Vector2Int coord)
        {
            if(!_shipsSegmentsVisuals.ContainsKey(coord)) return;
            _shipsSegmentsVisuals[coord].layer = DEFEATED_SHIPS_LAYER;
        }

        public override void CleanVisual()
        {
            if(_shipsSegmentsVisuals == null) return;
            
            foreach (var segment in _shipsSegmentsVisuals.Values)
            {
                segment.SetActive(false);
                Destroy(segment);
            }

            _shipsSegmentsVisuals = null;
        }
    }
}
