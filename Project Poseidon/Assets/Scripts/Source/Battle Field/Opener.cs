using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Opener : IOpener
    {
        private readonly Vector2Int _coord;

        public Opener(Vector2Int coord)
        {
            _coord = coord;
        }
        
        public IEnumerable<Vector2Int> GetOpenInformation()
        {
            return new List<Vector2Int>{_coord};
        }
    }
}
