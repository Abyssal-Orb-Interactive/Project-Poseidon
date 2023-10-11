using System;
using Source.Ships;

namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell, IDisposable
    {
        public bool IsOpened { get; private set; }
        public IReadonlyLogicalRepresentation Ship { get; private set; }

        public bool HasShip => Ship != null;

        public bool TryOpen()
        {
            if (IsOpened) return false;
            Open();
            return true;
        }
        
        private void Open()
        {
            IsOpened = true;
        }

        public bool TryPlaceShip(IReadonlyLogicalRepresentation ship)
        {
            if (HasShip) return false;
            PlaceShip(ship);
            return true;
        }
        
        private void PlaceShip(IReadonlyLogicalRepresentation ship)
        {
            Ship = ship;
        }

        public void Clear()
        {
            Ship = null;
            IsOpened = false;
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
