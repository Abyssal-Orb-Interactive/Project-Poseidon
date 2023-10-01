using System.Collections.Generic;
using UnityEngine;

namespace Source.Ships
{
    public interface IReadonlyLogicalRepresentation
    {
        public event ExplosionContext Explosion;
        
        public IEnumerable<Vector2Int> SegmentsCoords { get; }
        public IEnumerable<Vector2Int> RestrictedAreaCoords { get; }
        public int HealthPoints { get; }
        public Vector2Int BowCoord { get; }

        public GameObject GetGraphicsRepresentation();

        public ShipExplosion GetExplosionZoneOpener();
    }
}
