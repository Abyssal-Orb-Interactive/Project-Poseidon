using System;
using System.Collections.Generic;
using Source.Ships;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Grid
    {
        private readonly Dictionary<Vector2Int, Cell> _cells;
        private ShipExplosion _explosion;
        private bool _isExplosionReady;

        public Grid(IDictionary<Vector2Int, Cell> cells)
        {
            _cells = new Dictionary<Vector2Int, Cell>(cells);
            _isExplosionReady = false;
        }

        public event Explosion ShipExplose;

        public IOpener GetExplosion()
        {
            if (!_isExplosionReady) throw new InvalidOperationException("Explosion info is not actual.");

            _isExplosionReady = false;
            return _explosion;
        }
        
        public IEnumerable<IReadonlyCell> Cells => _cells.Values;
        public IEnumerable<Vector2Int> Coords => _cells.Keys;

        public bool TryOpenCells(IOpener opener)
        {
            foreach (var coord in opener.GetOpenInformation())
            {

                if (_cells.ContainsKey(coord) && _cells[coord].TryOpen())
                {
                    if (_cells[coord].GetShipStatus()) _cells[coord].GetShip().TakeHit(opener);
                    Debug.Log($"Coord {coord.x}-{coord.y}");
                    continue;
                }
                
                foreach (var coord1 in opener.GetOpenInformation())
                {
                    if (coord == coord1) continue;
                    
                    _cells[coord1].TryClose();
                }
                return false;
            }

            return true;
        }

        public bool TryPlaceShip(IEnumerable<Vector2Int> coords, ShipLogicalRepresentation ship)
        {
            var result = true;
            foreach (var coord in coords)
            {
                result = _cells[coord].TryPlaceShip(ship);
                if (result)
                {
                    ship.OnExplosion += () => { Explosion(ship); };
                    continue;
                }
                
                foreach (var coord1 in coords)
                {
                    ship.OnExplosion -= () => { Explosion(ship); };
                    _cells[coord1].Clear();
                }
                    
                break;
            }

            
            
            return result;
        }

        private void Explosion(IReadonlyLogicalRepresentation ship)
        {
            _explosion = ship.GetExplosionZoneOpener();
            _isExplosionReady = true;
            ShipExplose!.Invoke();

        }
        
        public bool HasShip(Vector2Int coord)
        {
            return _cells.ContainsKey(coord) && _cells[coord].GetShipStatus();
        }
    }
}
