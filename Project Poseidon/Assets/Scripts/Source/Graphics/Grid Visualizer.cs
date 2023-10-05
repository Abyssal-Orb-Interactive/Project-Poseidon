using System;
using System.Collections.Generic;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source.Graphics
{
    public class GridVisualizer : GameObjectsVisualizer<List<GameObject>>
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);

        private Grid _logicalGrid;

        public void Initialize(Grid grid)
        {
            _logicalGrid = grid;
            _visuals ??= new List<GameObject>();
            
            var cellRect = _cellPrefab.GetComponent<RectTransform>().rect;
            var cellSize = new Vector2(cellRect.width, cellRect.height);
            
            foreach (var coord in _logicalGrid.Coords)
            {
                var cell = Instantiate(_cellPrefab,_container);
                
                _visuals.Add(cell);

                var cellPosition = new Vector2((coord.x - _fieldOffset.x) * cellSize.x, (coord.y - _fieldOffset.y) * cellSize.y);

                cell.GetComponent<RectTransform>().anchoredPosition = cellPosition;
            }
        }


        public override void CleanVisual()
        {
            base.CleanVisual();
            _logicalGrid = null;
            _visuals = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _cellPrefab = null;
        }
    }
}
