using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Grid
    {
        private readonly Dictionary<Vector2Int, Cell> _cells;

        public Grid(IDictionary<Vector2Int, Cell> cells)
        {
            _cells = new Dictionary<Vector2Int, Cell>(cells);
        }

        public IEnumerable<IReadonlyCell> Cells => _cells.Values;
        public IEnumerable<Vector2Int> Coords => _cells.Keys;

        public bool TryOpenCells(IOpener opener)
        {
            foreach (var coord in opener.GetOpenInformation())
            {

                if (_cells.ContainsKey(coord) && _cells[coord].TryOpen())
                {
                    Debug.Log($"Coord {coord.x}-{coord.y}");
                    continue;
                }
                
                foreach (var coord1 in opener.GetOpenInformation())
                {
                    if (coord == coord1) continue;
                    
                    _cells[coord1].TryClose();
                }
                return false;
            }

            return true;
        }
    }
}
