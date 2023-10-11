using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public interface IOpener : IDisposable
    {
        public IEnumerable<Vector2Int> GetOpenInformation();
        public void RemoveCoord(Vector2Int coord);
    }
}
