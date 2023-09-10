using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Ships
{
    /// <summary>
    /// Represents a ship in the game.
    /// </summary>
    public class ShipLogicalRepresentation
    {
        private static readonly int[,] RELATIVE_POSITIONS = {
            { -1, -1 }, { -1, 0 }, { -1, 1 },
            { 0, -1 },             { 0, 1 },
            { 1, -1 }, { 1, 0 }, { 1, 1 }
        };
        
        private readonly Ship _ship;
        
        private Orientation _orientation = Orientation.Vertical;
        private Vector2Int _bowCoord;
        private int _hp;
        
        private HashSet<Vector2Int> _segmentsCoords;
        private HashSet<Vector2Int> _restrictedAreaCoords;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipLogicalRepresentation"/> class.
        /// </summary>
        /// <param name="ship">The ship data.</param>
        public ShipLogicalRepresentation(Ship ship)
        {
            _ship = ship;
            _hp = _ship.Size;
        }
        
        /// <summary>
        /// Gets the coordinates of the ship's segments.
        /// </summary>
        public IEnumerable<Vector2Int> SegmentsCoords => _segmentsCoords;
        
        /// <summary>
        /// Gets the coordinates of the ship's restricted area.
        /// </summary>
        public IEnumerable<Vector2Int> RestrictedAreaCoords => _restrictedAreaCoords;
        
        /// <summary>
        /// Gets the HP of the ship.
        /// </summary>
        public int HP => _hp;
        
        /// <summary>
        /// Sets the ship's initial position to the specified coordinate.
        /// </summary>
        /// <param name="bowCoord">The coordinate of the ship's bow.</param>
        public void SetPosition(Vector2Int bowCoord)
        {
            _bowCoord = bowCoord;
            RecalculateAllCoords();
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
            _segmentsCoords = new HashSet<Vector2Int> { _bowCoord };
            
            if (_orientation == Orientation.Horizontal)
            {
                for (var i = 1; i < _ship.Size; i++)
                {
                    var previousCoord = _segmentsCoords.Last();
                    _segmentsCoords.Add(new Vector2Int(previousCoord.x + 1, previousCoord.y));
                }   
            }
            else
            {
                for (var i = 1; i < _ship.Size; i++)
                {
                    var previousCoord = _segmentsCoords.Last();
                    _segmentsCoords.Add(new Vector2Int(previousCoord.x, previousCoord.y + 1));
                }
            }
        }

        private void CalculateRestrictedArea()
        {
            if (_segmentsCoords == null) CalculateSegmentsCoords();

            _restrictedAreaCoords = new HashSet<Vector2Int>();

            foreach (var segment in _segmentsCoords!)
            {
                AddCoordinatesToRestrictedArea(in segment);
            }
            
            _restrictedAreaCoords.ExceptWith(_segmentsCoords);
        }
        
        private void AddCoordinatesToRestrictedArea(in Vector2Int segment)
        {
            for (var i = 0; i < RELATIVE_POSITIONS.GetLength(0); i++)
            {
                var dx = RELATIVE_POSITIONS[i, 0];
                var dy = RELATIVE_POSITIONS[i, 1];
                var coord = new Vector2Int(segment.x + dx, segment.y + dy);
                _restrictedAreaCoords.Add(coord);
            }
        }
        
        /// <summary>
        ///  Rotates the ship, changing its orientation and recalculating coordinates accordingly.
        /// </summary>
        public void Rotate()
        {
            _orientation++;
            RecalculateAllCoords();
        }
    }
}
