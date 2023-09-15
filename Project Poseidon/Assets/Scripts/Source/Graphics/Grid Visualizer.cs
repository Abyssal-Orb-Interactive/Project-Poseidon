using UnityEngine;
using UnityEngine.UI;
using Grid = Source.Battle_Field.Grid;

namespace Source.Graphics
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Transform _gridContainer;

        private Grid _logicalGrid;
        
        private void OnValidate()
        {
            _gridContainer ??= GetComponentInParent<Transform>();
            //_cellPrefab
        }

        public void Initialize(Grid grid)
        {
            _logicalGrid = grid;

            foreach (var coord in _logicalGrid.Coords)
            {
                var cell = Instantiate(_cellPrefab, _gridContainer);

                var cellX = coord.x * _cellPrefab.GetComponent<RectTransform>().rect.width;
                var cellY = coord.y * _cellPrefab.GetComponent<RectTransform>().rect.height;

                cell.GetComponent<RectTransform>().anchoredPosition = new Vector2(cellX, cellY);
                
                
            }
        }
    }
}
