using Source.Battle_Field;
using UnityEngine;

namespace Source.Graphics.Markers
{
    public class MarkersVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform _markersContainer;
        [SerializeField] private Vector3 _fieldOffset = new(4.5f, 0, 4.5f);
        [SerializeField] private Vector2 _cellOffset = new Vector2(10f, 10f);
        
        private void OnValidate()
        {
            _markersContainer ??= GetComponentInParent<Transform>();
        }

        public void PlaceMarker(Vector2Int position, TypeOfOpens type)
        {
            var prefab = MarkerCreator.Create(type);
            
            var marker =  Instantiate(prefab.GetPlacemenOrigin(), _markersContainer);
            var markerPosition = new Vector2((position.x - _fieldOffset.x) * _cellOffset.x, (position.y - _fieldOffset.z) * _cellOffset.y);

            marker.GetComponent<RectTransform>().anchoredPosition = markerPosition;
        }
    }
}
