using UnityEngine;

namespace Source.Graphics.Markers
{
     public abstract class Marker
    {
        protected readonly Object _marker;

        protected Marker(Object marker)
        {
            _marker = marker;
        }

        public object GetPlacemenOrigin()
        {
            return _marker;
        }
    }
}
