using System.Collections.Generic;
using UnityEngine;

namespace Source.Graphics
{
    public abstract class GameObjectsVisualizer<T> : MonoBehaviour where T : IEnumerable<GameObject>
    {
        [SerializeField] protected Transform _container;
        protected T _visuals;

        protected virtual void OnValidate()
        {
            _container ??= GetComponent<Transform>();
        }

        public virtual void CleanVisual()
        {
            if (_visuals == null) return;

            foreach (var visual in _visuals)
            {
                visual.SetActive(false);
                Destroy(visual);
            }

            _visuals = default;
        }

        protected virtual void OnDestroy()
        {
            CleanVisual();
            _container = null;
        }
    }
}