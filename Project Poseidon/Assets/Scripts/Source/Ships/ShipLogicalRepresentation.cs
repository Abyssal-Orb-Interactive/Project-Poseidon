using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Ships
{
    public class ShipLogicalRepresentation : IReadonlyLogicalRepresentation
    {
        private static readonly (int, int)[] RelativePositions =
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 1),
            (1, -1), (1, 0), (1, 1)
        };
       
        private Ship _ship;
        private HashSet<Vector2Int> _segmentsCoords;
        private HashSet<Vector2Int> _restrictedAreaCoords;

        public event ExplosionContext Explosion;
        
        public ShipLogicalRepresentation(Ship ship)
        {
            _ship = ship ? ship : throw new ArgumentNullException(nameof(ship));
            HealthPoints = _ship.Size;
            _segmentsCoords = new HashSet<Vector2Int>();
            _restrictedAreaCoords = new HashSet<Vector2Int>();
            BowCoord = Vector2Int.zero;
        }
        
        public IEnumerable<Vector2Int> SegmentsCoords => _segmentsCoords;
        public IEnumerable<Vector2Int> RestrictedAreaCoords => _restrictedAreaCoords;
        public int HealthPoints { get; private set; }
        public Vector2Int BowCoord { get; private set; }
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        
        public void SetPosition(Vector2Int bowCoord)
        {
            BowCoord = bowCoord;
            RecalculateAllCoords();
        }
        
        public void Rotate()
        {
            Orientation = Orientation == Orientation.VerticalReversed ? Orientation.Horizontal : Orientation + 1;
            RecalculateAllCoords();
        }
        
        public GameObject GetGraphicsRepresentation()
        {
            return _ship.ShipPrefab;
        }
        
        public void TakeHit()
        {
            HealthPoints--;
            
            if (HealthPoints > 0) return;
            
            Explosion!.Invoke();
            Destroy();
        }
        
        public ShipExplosion GetExplosionZoneOpener()
        {
            return new ShipExplosion(_restrictedAreaCoords);
        }
        
        public void Destroy()
        {
            _segmentsCoords = null;
            _restrictedAreaCoords = null;
            _ship = null;
            Explosion = null;
        }
        
        private void RecalculateAllCoords()
        {
            _segmentsCoords.Clear();
            _restrictedAreaCoords.Clear();
            CalculateSegmentsCoords();
            CalculateRestrictedArea();
        }
        
        private void CalculateSegmentsCoords()
        {
            _segmentsCoords.Add(BowCoord);
            var previousCoord = _segmentsCoords.Last();
            
            var (dx, dy) = GetRelativePosition(1);
            for (var i = 1; i < _ship.Size; i++)
            {
                _segmentsCoords.Add(new Vector2Int(previousCoord.x + dx, previousCoord.y + dy));
                previousCoord = _segmentsCoords.Last();
            }
        }

        private void CalculateRestrictedArea()
        {
            foreach (var segment in _segmentsCoords)
            {
                AddCoordinatesToRestrictedArea(segment);
            }

            _restrictedAreaCoords.ExceptWith(_segmentsCoords);
        }

        private void AddCoordinatesToRestrictedArea(in Vector2Int segment)
        {
            foreach (var (dx, dy) in RelativePositions)
            {
                var coord = new Vector2Int(segment.x + dx, segment.y + dy);
                _restrictedAreaCoords.Add(coord);
            }
        }
        
        private (int, int) GetRelativePosition(int index)
        {
            int dx, dy;
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    dx = index;
                    dy = 0;
                    break;
                case Orientation.Vertical:
                    dx = 0;
                    dy = index;
                    break;
                case Orientation.HorizontalReversed:
                    dx = -index;
                    dy = 0;
                    break;
                case Orientation.VerticalReversed:
                    dx = 0;
                    dy = -index;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Orientation));
            }
            return (dx, dy);
        }
    }
    public delegate void ExplosionContext();
}
