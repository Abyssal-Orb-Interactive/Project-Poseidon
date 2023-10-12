using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public static class GridFabric
    {
        private const int DEFAULT_X = 10;
        private const int DEFAULT_Y = 10;
        
        public static IDictionary<Vector2Int, Cell> CreateGrid(int startX, int startY)
        {
            var cells = new Dictionary<Vector2Int, Cell>();
            
            for (var x = startX; x < DEFAULT_X + startX ; x++)
            {
                for (var y = startY; y < DEFAULT_Y + startY; y++)
                {
                    cells.Add(new Vector2Int(x,y), new Cell());
                }
            }

            return cells;
        }
    }
}
