using System.Collections.Generic;
using System.Linq;
using Base.Graphics;
using Source.Ships;
using UnityEngine;

namespace Source.Graphics
{
    public class ShipVisualizer : DictionaryGameObjectsVisualizer<Dictionary<Vector2Int, GameObject>>
    {
        private const int DEFEATED_SHIPS_LAYER = 8;
        private const int FRIENDLY_SHIPS_LAYER = 7;
        private const int ENEMY_SHIPS_LAYER = 6;
        
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);
        [SerializeField] private Vector2 _cellSize = new(10f, 10f);

        private List<ShipData> _shipsData = new();
        private List<GameObject> _defeatedShipVisuals = new();
        public void VisualizeHit(Vector2Int coord)
        {
            if(!Visual.ContainsKey(coord)) return;
            Visual[coord].layer = DEFEATED_SHIPS_LAYER;
            _defeatedShipVisuals.Add(Visual[coord]);
        }

        public void MakeVisible()
        {
            foreach (var visual in Visual.Values.Where(visual => !_defeatedShipVisuals.Contains(visual)))
            {
                visual.layer = FRIENDLY_SHIPS_LAYER;
            }
        }
        
        public void MakeInvisible()
        {
            foreach (var visual in Visual.Values.Where(visual => !_defeatedShipVisuals.Contains(visual)))
            {
                visual.layer = ENEMY_SHIPS_LAYER;
            }
        }

        public void ClearVisual()
        {
            if (Visual == null) return;
            
            foreach (var value in Visual.Values)
            {
                Destroy(value);
            }
            
            Visual.Clear();
        }

        private void AddShip(IReadonlyLogicalRepresentation ship)
        {
            _shipsData.Add(new ShipData(ship));
        }
        
        public void AddShips(IEnumerable<IReadonlyLogicalRepresentation> ships)
        {
            Visual ??= new Dictionary<Vector2Int, GameObject>();
            
            foreach (var ship in ships)
            {
                AddShip(ship);
            }
        }

        private void VisualizeShip(ShipData data)
        {
            foreach (var coord in data.SegmentsCoords)
            {
                if(Visual.ContainsKey(coord)) continue;
                
                var prefab = data.Prefab;
                var position = new Vector3(coord.x * _cellSize.x + _fieldOffset.x, 0, coord.y * _cellSize.y + _fieldOffset.y);
                var shipSegment = Instantiate(prefab, position, Quaternion.identity, _container);

                Visual.Add(coord, shipSegment);
            }
        }
        
        public override void Visualize()
        {
            foreach (var data in _shipsData)
            {
                VisualizeShip(data);
            }
            
            _shipsData.Clear();
        }

        public void DeleteSegmentVisual(Vector2Int coord)
        {
            if(!Visual.ContainsKey(coord)) return;
            
            Destroy(Visual[coord]);
            Visual.Remove(coord);
        }

        protected override void Dispose()
        {
           _fieldOffset = Vector2.zero;
           _cellSize = Vector2.zero;
           _shipsData = null;
           base.Dispose();
        }
        
        private struct ShipData
        {
            public GameObject Prefab;
            public List<Vector2Int> SegmentsCoords;

            public ShipData(IReadonlyLogicalRepresentation ship)
            {
                Prefab = ship.GetGraphicsRepresentation();
                SegmentsCoords = ship.SegmentsCoords.ToList();
            }
        }
    }
}
