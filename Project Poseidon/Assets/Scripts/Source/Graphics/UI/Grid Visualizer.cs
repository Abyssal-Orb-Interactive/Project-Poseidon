using System.Collections.Generic;
using Base.Graphics;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source.Graphics.UI
{
    public class GridVisualizer : CollectionGameObjectsVisualizer<List<GameObject>>
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);
        private Grid _grid;

        public void Initialize(Grid grid)
        {
           _grid = grid;
        }

        public override void Visualize()
        {
            VisualizeGrid();
        }

        private void VisualizeGrid()
        {
            Visual ??= new List<GameObject>();
            
            var cellRect = _cellPrefab.GetComponent<RectTransform>().rect;
            var cellSize = new Vector2(cellRect.width, cellRect.height);
            
            foreach (var coord in _grid.Coords)
            {
                var cell = Instantiate(_cellPrefab,_container);
                
                Visual.Add(cell);

                var cellPosition = new Vector2((coord.x - _fieldOffset.x) * cellSize.x, (coord.y - _fieldOffset.y) * cellSize.y);

                cell.GetComponent<RectTransform>().anchoredPosition = cellPosition;
            }
        }

        protected override void Destruct()
        {
            _grid = null;
            _cellPrefab = null;
            _fieldOffset = Vector2.zero;
            base.Destruct();
        }
    }
}
