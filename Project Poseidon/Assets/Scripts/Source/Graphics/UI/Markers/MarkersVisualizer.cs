using System;
using System.Collections.Generic;
using Base.Graphics;
using Source.Battle_Field;
using UnityEngine;

namespace Source.Graphics.Markers
{
    public class MarkersVisualizer : CollectionGameObjectsVisualizer<List<GameObject>>
    {
        [SerializeField] private Vector2 _fieldOffset = new(4.5f,4.5f);
        [SerializeField] private Vector2 _cellOffset = new(10f, 10f);

        private List<MarkerData> _markersData = new();
        
        public override void Visualize()
        {
            foreach (var data in _markersData)
            {
               CreateAndPlaceMarker(data.Position, data.Type);
            }

            _markersData.Clear();
        }

        public void AddMarker(Vector2Int position, OpenType type)
        {
            _markersData.Add(new MarkerData(position, type));
        }

        public void AddMarkers(IReadOnlyList<Vector2Int> positions, IReadOnlyList<OpenType> types)
        {
            if (positions.Count != types.Count)
                throw new InvalidOperationException(
                    "Number of coords must be same as number of types for placing markers");
            for (var i = 0; i < positions.Count; i++)
            {
                AddMarker(positions[i], types[i]);
            }
        }

        private void CreateAndPlaceMarker(Vector2Int position, OpenType type)
        {
            Visual ??= new List<GameObject>();

            var prefab = MarkerFabric.Create(type);
            var marker = Instantiate(prefab.GetPlacemenOrigin(), _container);
            var markerPosition = new Vector2((position.x - _fieldOffset.x) * _cellOffset.x, (position.y - _fieldOffset.y) * _cellOffset.y);

            marker.GetComponent<RectTransform>().anchoredPosition = markerPosition;
            Visual.Add(marker);
        }

        protected override void Dispose()
        {
            _cellOffset = Vector2.zero;
            _fieldOffset = Vector2.zero;
            _markersData = null;
            base.Dispose();
        }
        
        private struct MarkerData
        {
            public Vector2Int Position;
            public OpenType Type;

            public MarkerData(Vector2Int position, OpenType type)
            {
                Position = position;
                Type = type;
            }
        }
    }
}
