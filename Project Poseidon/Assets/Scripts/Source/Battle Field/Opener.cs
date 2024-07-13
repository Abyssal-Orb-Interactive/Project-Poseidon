using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Opener : IOpener
    {
        private Vector2Int _coord;

        public Opener(Vector2Int coord)
        {
            _coord = coord;
        }
        
        public IEnumerable<Vector2Int> GetOpenInformation()
        {
            return new List<Vector2Int>{_coord};
        }

        public void RemoveCoord(Vector2Int coord)
        {
            _coord = Vector2Int.zero;
        }

        public void Dispose()
        {
            _coord = Vector2Int.zero;
            GC.SuppressFinalize(this);
        }
    }
}
