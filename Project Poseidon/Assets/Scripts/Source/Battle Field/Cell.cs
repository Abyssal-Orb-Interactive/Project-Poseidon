using Source.Ships;

namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell
    {
        private bool _isOpened;
        private ShipLogicalRepresentation _ship;
    
        public bool GetOpenStatus()
        {
            return _isOpened;
        }

        public bool GetShipStatus()
        {
            return _ship != null;
        }

        public bool TryClose()
        {
            if (!_isOpened) return false;
            Close();
            return true;
        }
        
        private void Close()
        {
            _isOpened = false;
        }
        
        public bool TryOpen()
        {
            if (_isOpened) return false;
            Open();
            return true;
        }
        
        private void Open()
        {
            _isOpened = true;
        }

        public bool TryPlaceShip(ShipLogicalRepresentation ship)
        {
            if (GetShipStatus()) return false;
            PlaceShip(ship);
            return true;
        }
        
        private void PlaceShip(ShipLogicalRepresentation ship)
        {
            _ship = ship;
        }

        public void Clear()
        {
            _ship = null;
            _isOpened = false;
        }

        public ShipLogicalRepresentation GetShip()
        {
            return _ship;
        }
    }
}
