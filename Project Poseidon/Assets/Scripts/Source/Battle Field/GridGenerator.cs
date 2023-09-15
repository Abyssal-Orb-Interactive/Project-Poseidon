using System.Collections.Generic;
using UnityEngine;

namespace Source.Battle_Field
{
    public static class GridGenerator
    {
        private const int X = 10;
        private const int Y = 10;
        
        public static IDictionary<Vector2Int, Cell> CreateGrid()
        {
            var cells = new Dictionary<Vector2Int, Cell>();
            
            for (var x = 0; x < X; x++)
            {
                for (var y = 0; y < Y; y++)
                {
                    cells.Add(new Vector2Int(x,y), new Cell());
                }
            }

            return cells;
        }
        
        public static IDictionary<Vector2Int, Cell> CreateGrid(int x, int y)
        {
            var cells = new Dictionary<Vector2Int, Cell>();
            
            for (var xIndex = 0; xIndex < x; xIndex++)
            {
                for (var yIndex = 0; yIndex < y; yIndex++)
                {
                    cells.Add(new Vector2Int(xIndex,yIndex), new Cell());
                }
            }

            return cells;
        } 
    }
}
