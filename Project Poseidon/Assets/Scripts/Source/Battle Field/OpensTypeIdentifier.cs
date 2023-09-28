using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Battle_Field
{
    public class OpensTypeIdentifier
    {
        private Grid _grid;

        public OpensTypeIdentifier(Grid grid)
        {
            _grid = grid;
        }

        public TypeOfOpens GetType(Vector2Int coord, IOpener opener)
        {
            if (opener is ShipExplosion) return TypeOfOpens.ShipExplosion;

            return _grid.HasShip(coord) ? TypeOfOpens.Hit : TypeOfOpens.Miss;
        }

        public IEnumerable<TypeOfOpens> GetTypes(IEnumerable<Vector2Int> coords, IOpener opener)
        {
            return coords.Select(coord => GetType(coord, opener)).ToList();
        }
    }
}
