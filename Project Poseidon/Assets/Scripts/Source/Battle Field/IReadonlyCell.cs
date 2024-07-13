using System;
using Source.Ships;

namespace Source.Battle_Field
{
    public interface IReadonlyCell : IDisposable
    {
        public bool IsOpened { get; }
        public bool HasShip { get; }
        public IReadonlyLogicalRepresentation Ship { get; }
    }
}
