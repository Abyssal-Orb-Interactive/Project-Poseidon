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

        public OpenType GetType(Vector2Int coord, IOpener opener)
        {
            if (opener is ShipExplosion) return OpenType.ShipExplosion;

            return _grid.HasShip(coord) ? OpenType.Hit : OpenType.Miss;
        }

        public IEnumerable<OpenType> GetTypes(IEnumerable<Vector2Int> coords, IOpener opener)
        {
            return coords.Select(coord => GetType(coord, opener));
        }

        public void Destroy()
        {
            _grid = null;
        }
    }
}
