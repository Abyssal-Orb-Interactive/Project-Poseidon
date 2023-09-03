using System;

namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell
    {
         private bool IsOpened { get; set; }
    
        public bool GetStatus()
        {
            return IsOpened;
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
    }
}
