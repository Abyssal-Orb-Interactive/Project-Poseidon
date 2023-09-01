namespace Source.Battle_Field
{
    public class Cell : IReadonlyCell
    {
         private bool IsOpened { get; set; }
    
        public bool GetStatus()
        {
            return IsOpened;
        }

        public void Open()
        {
            IsOpened = true;
        }
    }
}
