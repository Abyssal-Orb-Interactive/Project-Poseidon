using System;
using System.Collections.Generic;
using Source.Battle_Field;
using UnityEngine;

namespace Source.Graphics.Markers
{
    public class MarkersVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform _markersContainer;
        [SerializeField] private Vector2 _fieldOffset = new(4.5f,4.5f);
        [SerializeField] private Vector2 _cellOffset = new Vector2(10f, 10f);

        private List<GameObject> _markers;
        
        private void OnValidate()
        {
            _markersContainer ??= GetComponentInParent<Transform>();
        }
        public void PlaceMarkers(IReadOnlyList<Vector2Int> positions, IReadOnlyList<OpenType> types)
        {
            _markers ??= new List<GameObject>();
            
            if (positions.Count != types.Count) throw new InvalidOperationException("Number of coords must be same as number of types for placing markers");
            
            for (var i = 0; i < positions.Count; i++)
            {
                PlaceMarker(positions[i], types[i]);
            }
        }

        public void PlaceMarker(Vector2Int position, OpenType openType)
        {
            _markers ??= new List<GameObject>();
            
            var prefab = MarkerCreator.Create(openType);
            
            var marker =  Instantiate(prefab.GetPlacemenOrigin(), _markersContainer);
            var markerPosition = new Vector2((position.x - _fieldOffset.x) * _cellOffset.x, (position.y - _fieldOffset.y) * _cellOffset.y);

            marker.GetComponent<RectTransform>().anchoredPosition = markerPosition;
            
            _markers.Add(marker);
        }


        public void CleanVisual()
        {
            if(_markers == null) return;
            
            foreach (var marker in _markers)
            {
                marker.SetActive(false);
                Destroy(marker);
            }

            _markers = null;
        }

        private void OnDestroy()
        {
            CleanVisual();
            _markersContainer = null;
        }
    }
}
