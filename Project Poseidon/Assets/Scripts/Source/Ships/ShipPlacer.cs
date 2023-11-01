using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;
using Random = UnityEngine.Random;

namespace Source.Ships
{
    public class ShipPlacer : IDisposable
    {
        private List<ShipLogicalRepresentation> _ships;
        private Grid _grid;
        private Fleet _fleet;

        public ShipPlacer(Grid grid, Fleet fleet)
        {
            _ships = new List<ShipLogicalRepresentation>();
            _grid = grid;
            _fleet = fleet;
        }

        public bool TryPlaceShips()
        {
            const int MAX_TRIES = 100;
    
            foreach (var ship in _fleet.Ships)
            {
                for (var triesCounter = 0; triesCounter < MAX_TRIES; triesCounter++)
                {
                    var randomOrientation = (Orientation)Random.Range(0, Enum.GetValues(typeof(Orientation)).Length);

                    if (!TryPlaceShipRandomly(randomOrientation, ship)) continue;
                    
                    break;
                }
            }
    
            return true;
        }
        
        private Vector2Int RandomCoordForOrientation(Orientation orientation, int shipSize)
        {
            var xRange = orientation == Orientation.HorizontalReversed
                ? _grid.MaxCoord.x - shipSize + 1
                : _grid.MaxCoord.x - (orientation == Orientation.Horizontal ? shipSize - 1 : 0);

            var yRange = orientation == Orientation.VerticalReversed
                ? _grid.MaxCoord.y - shipSize + 1
                : _grid.MaxCoord.y - (orientation == Orientation.Vertical ? shipSize - 1 : 0);

            var x = Random.Range(_grid.MinCoord.x, xRange);
            var y = Random.Range(_grid.MinCoord.y, yRange);

            return new Vector2Int(x, y);
        }
        
        public bool TryPlaceShipRandomly(Orientation orientation, Ship ship)
        {
            var shipLogical = ShipFabric.Create(ship.Type);
            
            var bowCoord = RandomCoordForOrientation(orientation, ship.Size);
            shipLogical.SetPosition(bowCoord);
            shipLogical.Orientation = orientation;

            if (HasShipEntersInAnotherShip(shipLogical) || HasShipEntersAnyRestrictedArea(shipLogical))
            {
                return false;
            }

            var result = _grid.TryPlaceShip(shipLogical.SegmentsCoords as IReadOnlyCollection<Vector2Int>, shipLogical);
            if(result) _ships.Add(shipLogical);
            return result;
        }

        public void DeleteShip(IReadonlyLogicalRepresentation ship)
        {
            _ships.Remove((ShipLogicalRepresentation)ship);
        }
        
        public bool TryPlaceShip(Orientation orientation, Ship ship, Vector2Int bowCoord)
        {
            var shipLogical = ShipFabric.Create(ship.Type);
            
            shipLogical.SetPosition(bowCoord);
            shipLogical.Orientation = orientation;

            if (HasShipEntersInAnotherShip(shipLogical) || HasShipEntersAnyRestrictedArea(shipLogical))
            {
                shipLogical.Dispose();
                return false;
            }

            var result = _grid.TryPlaceShip(shipLogical.SegmentsCoords as IReadOnlyCollection<Vector2Int>, shipLogical);
            if(result) _ships.Add(shipLogical);
            return result;
        }

        private bool HasShipEntersAnyRestrictedArea(in IReadonlyLogicalRepresentation ship)
        {
            var representation = ship;
            return _ships.Exists(ship1 =>
                representation.SegmentsCoords.Any(segment => ship1.RestrictedAreaCoords.Contains(segment)));
        }

        private bool HasShipEntersInAnotherShip(in IReadonlyLogicalRepresentation ship)
        {
            var representation = ship;
            return _ships.Exists(anotherShip =>
                anotherShip.SegmentsCoords.Any(segment => representation.SegmentsCoords.Contains(segment)));

        }
        
        public IEnumerable<IReadonlyLogicalRepresentation> GetAllShips()
        {
            return _ships;
        }

        public void DisposeAllShips()
        {
            foreach (var ship in _ships)
            {
                ship.Dispose();
            }

            _ships = new List<ShipLogicalRepresentation>();
        }

        public void Dispose()
        {
            DisposeAllShips();
            _grid.Dispose();
            _ships = null;
            _grid = null;
            _fleet = null;
            GC.SuppressFinalize(this);
        }
    }
}
