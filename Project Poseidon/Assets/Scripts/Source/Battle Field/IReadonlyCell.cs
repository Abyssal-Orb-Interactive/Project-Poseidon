using Source.Ships;

namespace Source.Battle_Field
{
    public interface IReadonlyCell
    {
        public bool IsOpened { get; }
        public bool HasShip { get; }
        public IReadonlyLogicalRepresentation Ship { get; }
    }
}
