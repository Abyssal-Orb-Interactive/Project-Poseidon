using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Opener : IOpener
    {
        public Vector2Int hit0 = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9));
        public Vector2Int hit1 = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9));
        public Vector2Int hit2 = new Vector2Int(Random.Range(0, 9), Random.Range(0, 9));
        
        public IEnumerable<Vector2Int> GetOpenInformation()
        {
            return new List<Vector2Int>{hit0, hit1, hit2};
        }
    }
}
