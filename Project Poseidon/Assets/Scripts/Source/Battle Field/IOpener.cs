using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public interface IOpener
    {
        public IReadOnlyList<Vector2Int> GetOpenInformation();
    }
}
