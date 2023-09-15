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

        public void OpenCells(IOpener opener)
        {
            foreach (var coord in opener.GetOpenInformation())
            {
                Debug.Log($"Coord {coord.x}{coord.y}");
                
                if (!_cells.ContainsKey(coord)) continue;

                _cells[coord].TryOpen();
            }
        }
    }
}
