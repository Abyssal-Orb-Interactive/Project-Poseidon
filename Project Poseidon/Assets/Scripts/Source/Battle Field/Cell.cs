namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell
    {
         private bool IsOpened { get; set; }
         private bool HasShip { get; set; }
    
        public bool GetOpenStatus()
        {
            return IsOpened;
        }

        public bool GetShipStatus()
        {
            return HasShip;
        }
        
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
