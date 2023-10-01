using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public static class GridFabric
    {
        private const int DEFAULT_X = 10;
        private const int DEFAULT_Y = 10;
        
        public static IDictionary<Vector2Int, Cell> CreateGrid()
        {
            var cells = new Dictionary<Vector2Int, Cell>();
            
            for (var x = 0; x < DEFAULT_X; x++)
            {
                for (var y = 0; y < DEFAULT_Y; y++)
                {
                    cells.Add(new Vector2Int(x,y), new Cell());
                }
            }

            return cells;
        }
        
        public static IDictionary<Vector2Int, Cell> CreateGrid(int x, int y)
        {
            var cells = new Dictionary<Vector2Int, Cell>();
            
            for (var xIndex = x; xIndex < DEFAULT_X + x; xIndex++)
            {
                for (var yIndex = y; yIndex < DEFAULT_Y + y; yIndex++)
                {
                    cells.Add(new Vector2Int(xIndex,yIndex), new Cell());
                }
            }

            return cells;
        }
    }
}
