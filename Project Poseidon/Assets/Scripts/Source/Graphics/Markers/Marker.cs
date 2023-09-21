using UnityEngine;

namespace Source.Graphics.Markers
{
     public abstract class Marker
    {
        protected readonly GameObject _marker;

        protected Marker(GameObject marker)
        {
            _marker = marker;
        }

        public GameObject GetPlacemenOrigin()
        {
            return _marker;
        }
    }
}
