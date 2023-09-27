using System.Collections.Generic;
using UnityEngine;

namespace Source.Ships
{
    public interface IReadonlyLogicalRepresentation
    {
        public event Explosion OnExplosion;
        
        public IEnumerable<Vector2Int> SegmentsCoords { get; }
        public IEnumerable<Vector2Int> RestrictedAreaCoords { get; }
        public int HP { get; }
        public Vector2Int BowCoord { get; }

        public GameObject GetGraphicsRepresentation();

        public ShipExplosion GetExplosionZoneOpener();
    }
}
