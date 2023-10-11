using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public interface IReadonlyOpener : IDisposable
    {
        public IEnumerable<Vector2Int> GetOpenInformation();
    }
}