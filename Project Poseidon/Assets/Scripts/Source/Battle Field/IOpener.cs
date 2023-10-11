using UnityEngine;

namespace Source.Battle_Field
{
    public interface IOpener : IReadonlyOpener
    {
        public void RemoveCoord(Vector2Int coord);
    }
}
