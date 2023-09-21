namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell
    {
        private bool _isOpened;
        private bool HasShip { get; set; }
    
        public bool GetOpenStatus()
        {
            return _isOpened;
        }

        public bool GetShipStatus()
        {
            return HasShip;
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

        public bool TryPlaceShip()
        {
            if (HasShip) return false;
            PlaceShip();
            return true;
        }
        
        private void PlaceShip()
        {
            HasShip = true;
        }
    }
}
