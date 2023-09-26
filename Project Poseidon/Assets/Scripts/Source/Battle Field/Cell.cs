namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell
    {
        private bool _isOpened;
        private bool _hasShip;
    
        public bool GetOpenStatus()
        {
            return _isOpened;
        }

        public bool GetShipStatus()
        {
            return _hasShip;
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
            if (_hasShip) return false;
            PlaceShip();
            return true;
        }
        
        private void PlaceShip()
        {
            _hasShip = true;
        }

        public void Clear()
        {
            _hasShip = false;
            _isOpened = false;
        }
    }
}
