using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Source.Ships;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Grid : ReadonlyGrid
    {
        private IReadOnlyDictionary<Vector2Int, Cell> _cells;
        private ShipExplosion _currentExplosionZone;
        private bool _isExplosionReadyToFire;

        public event ExplosionContext ShipExplosion;

        public Grid(IDictionary<Vector2Int, Cell> cells)
        {
            _cells = new ReadOnlyDictionary<Vector2Int, Cell>(cells);
            _isExplosionReadyToFire = false;

            if (CellsIsEmpty())
            {
                MinCoord = new Vector2Int(0, 0);
                MaxCoord = new Vector2Int(0, 0);
            }
            else
            {
                FindMinMaxCoordinates();
            }
        }

        private bool CellsIsEmpty()
        {
            return _cells.Count == 0;
        }
        
        private void FindMinMaxCoordinates()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;
            
            foreach (var coord in _cells.Keys)
            {
                var x = coord.x;
                var y = coord.y;

                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
            }

            MinCoord = new Vector2Int(minX, minY);
            MaxCoord = new Vector2Int(maxX, maxY);
        }
        
        public override IReadonlyOpener GetExplosion()
        {
            if (!_isExplosionReadyToFire) throw new InvalidOperationException("Explosion info is not actual.");

            _isExplosionReadyToFire = false;
            return _currentExplosionZone;
        }
        
        public IEnumerable<IReadonlyCell> Cells => _cells.Values;
        public IEnumerable<Vector2Int> Coords => _cells.Keys;

        public override IEnumerable<Vector2Int> GetCoords()
        {
            return Coords;
        }

        public override Vector2Int MaxCoord { get; protected set; }

        public override Vector2Int MinCoord { get; protected set; }

        public bool TryOpenCells(IOpener opener)
        {
            var hasOpened = false;
            
            foreach (var coord in opener.GetOpenInformation().ToList())
            {
                if (!_cells.TryGetValue(coord, out var cell) || !cell!.TryOpen())
                {
                    opener.RemoveCoord(coord);
                    continue;
                }

                if (cell!.HasShip && cell.Ship is ShipLogicalRepresentation ship)
                {
                    ship.TakeHit();
                }
                
                hasOpened = true;
            }

            return hasOpened;
        }
        
        public bool TryPlaceShip(IEnumerable<Vector2Int> coords, ShipLogicalRepresentation ship)
        {
            if (DoesShipHaveSegmentsOutOfGrid(ship))
            {
                return false;
            }

            var vector2Ints = coords as Vector2Int[] ?? coords.ToArray();
            if (!TryPlaceShipSegments(vector2Ints, ship))
            {
                ClearCellsAndUnsubscribeFromExplosion(vector2Ints, ship);
                return false;
            }
                
                
            SubscribeToExplosion(ship);
            return true;
        }

        private bool DoesShipHaveSegmentsOutOfGrid(in IReadonlyLogicalRepresentation ship)
        {
            return ship.SegmentsCoords.Any(segment => !_cells.ContainsKey(segment));
        }
        
        private bool TryPlaceShipSegments(in IEnumerable<Vector2Int> coords, IReadonlyLogicalRepresentation ship)
        {
            return coords.All(coord => IsPlacementAllowed(coord, ship));
        }
        
        private bool IsPlacementAllowed(Vector2Int coord, IReadonlyLogicalRepresentation ship)
        {
            return _cells[coord].TryPlaceShip(ship);
        }
        private void ClearCellsAndUnsubscribeFromExplosion(IEnumerable<Vector2Int> coords, IReadonlyLogicalRepresentation ship)
        {
            ClearCells(coords);
            UnsubscribeToExplosion(ship);
        }
        
        private void ClearCells(IEnumerable<Vector2Int> coords)
        {
            foreach (var coord in coords)
            {
                _cells[coord].Clear();
            }
        }
        
        private void SubscribeToExplosion(IReadonlyLogicalRepresentation ship)
        {
            ship.Explosion += () => { HandleExplosion(ship); };
        }

        private void UnsubscribeToExplosion(IReadonlyLogicalRepresentation ship)
        {
            ship.Explosion -= () => { HandleExplosion(ship); };
        }
        
        private void HandleExplosion(IReadonlyLogicalRepresentation ship)
        {
            _currentExplosionZone = ship.GetExplosionZoneOpener();
            _isExplosionReadyToFire = true;
            
            ShipExplosion!.Invoke();

        }
        
        public override bool HasShip(Vector2Int coord)
        {
            return _cells.ContainsKey(coord) && _cells[coord].HasShip;
        }

        public override IEnumerable<IReadonlyCell> GetCells()
        {
            return Cells;
        }

        public void Clear()
        {
            _currentExplosionZone = null;
            ShipExplosion = null;
            _isExplosionReadyToFire = false;
            _cells = null;
        }

        public override void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }
    }
}
