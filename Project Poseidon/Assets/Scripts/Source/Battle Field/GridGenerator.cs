using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public static class GridGenerator
    {
        public static IDictionary<Vector2Int, Cell> CreateGrid()
        {
            var cells = new Dictionary<Vector2Int, Cell>();
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    cells.Add(new Vector2Int(x,y), new Cell());
                }
            }

            return cells;
        }
    }
}
