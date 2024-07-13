using System;
using UnityEngine;

namespace Source.Graphics.Markers
{
     public abstract class Marker : IDisposable
    {
        protected GameObject _marker;

        protected Marker(GameObject marker)
        {
            _marker = marker;
        }

        public GameObject GetPlacemenOrigin()
        {
            return _marker;
        }

        public virtual void Dispose()
        {
            _marker.SetActive(false);
            _marker = null;
            GC.SuppressFinalize(this);
        }
    }
}
