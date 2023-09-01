using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public class Grid
    {
        private Dictionary<Vector2Int, Cell> _cells = null;

        public Grid(IDictionary<Vector2Int, Cell> cells)
        {
            _cells = new Dictionary<Vector2Int, Cell>(cells);
        }

        public IEnumerable<IReadonlyCell> Cells => _cells.Values;
        public IReadOnlyDictionary<Vector2Int, Cell> Field => _cells;

        public void OpenCell(IOpener opener)
        {
            foreach (var coord in opener.GetOpenInformation())
            {
                Debug.Log($"Coord {coord.x}{coord.y}");
                _cells[coord].Open();
            }
        }
    }
}
