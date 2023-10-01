using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public interface IOpener
    {
        public IEnumerable<Vector2Int> GetOpenInformation();
        public void RemoveCoord(Vector2Int coord);

        public void Destroy();
    }
}
