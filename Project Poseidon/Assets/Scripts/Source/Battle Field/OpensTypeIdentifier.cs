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
    }
}
