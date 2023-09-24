using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source.Ships
{
    public class ShipPlacer
    {
        private Dictionary<ShipType, int> _shipsLimits;
        private readonly List<ShipLogicalRepresentation> _ships;
        private Grid _grid; 

        public ShipPlacer(Grid grid)
        {
            _ships = new List<ShipLogicalRepresentation>();
            _shipsLimits = new Dictionary<ShipType, int>();
            _grid = grid;
        }

        public bool TryPlaceShip(Vector2Int coord, ShipType type)
        {
            if (_ships.Any(ship1 =>
                    ship1.RestrictedAreaCoords.Contains(coord))) return false;
            
            var ship = ShipCreator.Create(type);
            if(!_shipsLimits.ContainsKey(type)) _shipsLimits.Add(type, ship.GetMaxNumberOfShips());
            if (_shipsLimits[type] == 0)
            {
                ship.Destruct();
                return false;
            }
            
            _ships.Add(ship);

            _shipsLimits[type]--;

            var result = _grid.TryPlaceShip(coord);

            if (result) ship.SetPosition(coord);
            else
            {
                _shipsLimits[type]++;
                _ships.Remove(ship);
                ship.Destruct();
            }

            return result;
        }

        public IEnumerable<IReadonlyLogicalRepresentation> GetAllShips()
        {
            return _ships;
        }
    }
}
