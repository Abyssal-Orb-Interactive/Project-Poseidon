using System;
using System.Collections.Generic;
using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source.Graphics
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f, 4.5f);

        private List<GameObject> _cells;

        private Grid _logicalGrid;
        
        private void OnValidate()
        {
            _gridContainer ??= GetComponentInParent<Transform>();
        }

        public void Initialize(Grid grid)
        {
            _logicalGrid = grid;
            _cells ??= new List<GameObject>();
            
            var cellRect = _cellPrefab.GetComponent<RectTransform>().rect;
            var cellSize = new Vector2(cellRect.width, cellRect.height);
            
            foreach (var coord in _logicalGrid.Coords)
            {
                var cell = Instantiate(_cellPrefab,_gridContainer);
                
                _cells.Add(cell);

                var cellPosition = new Vector2((coord.x - _fieldOffset.x) * cellSize.x, (coord.y - _fieldOffset.y) * cellSize.y);

                cell.GetComponent<RectTransform>().anchoredPosition = cellPosition;
            }
        }


        public void CleanVisual()
        {
            if(_cells == null) return;
            
            foreach (var cell in _cells)
            {
                cell.SetActive(false);
                Destroy(cell);
            }

            _cells = null;
            _logicalGrid = null;
        }

        private void OnDestroy()
        {
            CleanVisual();
            _cellPrefab = null;
            _gridContainer = null;
        }
    }
}
