using UnityEngine;
using Grid = Source.Battle_Field.Grid;

namespace Source.Graphics
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private Vector3 _fieldOffset = new(4.5f, 0, 4.5f);

        private Grid _logicalGrid;
        
        private void OnValidate()
        {
            _gridContainer ??= GetComponentInParent<Transform>();
            //_cellPrefab
        }

        public void Initialize(Grid grid)
        {
            _logicalGrid = grid;
            
            var cellRect = _cellPrefab.GetComponent<RectTransform>().rect;
            var cellSize = new Vector2(cellRect.width, cellRect.height);
            
            foreach (var coord in _logicalGrid.Coords)
            {
                var cell = Instantiate(_cellPrefab,_gridContainer);

                var cellPosition = new Vector2((coord.x - _fieldOffset.x) * cellSize.x, (coord.y - _fieldOffset.z) * cellSize.y);

                cell.GetComponent<RectTransform>().anchoredPosition = cellPosition;
                
                
            }
        }
    }
}
