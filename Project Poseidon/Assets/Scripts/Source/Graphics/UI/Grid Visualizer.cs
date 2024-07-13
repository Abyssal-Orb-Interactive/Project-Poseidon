using System;
using System.Collections.Generic;
using System.Linq;
using Base.Graphics;
using Source.Battle_Field;
using UnityEngine;

namespace Source.Graphics.UI
{
    public class GridVisualizer : CollectionGameObjectsVisualizer<List<GameObject>>
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);
        private ReadonlyGrid _grid;

        public void Initialize(ReadonlyGrid grid)
        {
           _grid = grid;
        }

        public override void Visualize()
        {
            try
            {
                VisualizeGrid();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        private void VisualizeGrid()
        {
            Visual ??= new List<GameObject>();
            
            var cellRect = _cellPrefab.GetComponent<RectTransform>().rect;
            var cellSize = new Vector2(cellRect.width, cellRect.height);
           
            foreach (var coord in _grid.GetCoords())
            {
                var cell = Instantiate(_cellPrefab,_container);
                
                Visual.Add(cell);

                var cellPosition = new Vector2((coord.x - _fieldOffset.x) * cellSize.x, (coord.y - _fieldOffset.y) * cellSize.y);

                cell.GetComponent<RectTransform>().anchoredPosition = cellPosition;
            }
        }
        protected override void Dispose()
        {
            _grid = null;
            _cellPrefab = null;
            _fieldOffset = Vector2.zero;
            base.Dispose();
        }
    }
}
