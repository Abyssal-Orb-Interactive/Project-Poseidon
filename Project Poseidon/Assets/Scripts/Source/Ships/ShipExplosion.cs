using System.Collections.Generic;
using Source.Battle_Field;
using UnityEngine;

namespace Source.Ships
{
    public class ShipExplosion : IOpener
    {
        private List<Vector2Int> _explosionZone;

        public ShipExplosion(IEnumerable<Vector2Int> explosionZone)
        {
            _explosionZone = new List<Vector2Int>(explosionZone);
        }
        
        
        public IEnumerable<Vector2Int> GetOpenInformation()
        {
            return _explosionZone;
        }

        public void RemoveCoord(Vector2Int coord)
        {
            _explosionZone.Remove(coord);
        }

        public void Dispose()
        {
            _explosionZone = null;
        }
    }
}