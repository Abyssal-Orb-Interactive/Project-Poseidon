using System;
using UnityEngine;

namespace Source.Graphics.Markers
{
     public abstract class Marker : IDisposable
    {
        private GameObject _marker;

        protected Marker(GameObject marker)
        {
            _marker = marker;
        }

        public GameObject GetPlacemenOrigin()
        {
            return _marker;
        }

        public void Destroy()
        {
            _marker = null;
        }

        public abstract void Dispose();
    }
}
