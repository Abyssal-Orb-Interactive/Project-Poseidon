using UnityEngine;

namespace Base.Graphics
{
    public abstract class UnityEntityVisualizer<T> : MonoBehaviour
    {
        [SerializeField] protected Transform _container;
        protected T Visual;

        protected virtual void OnValidate()
        {
            Validate();
        }

        protected virtual void Validate()
        {
            _container ??= GetComponent<Transform>();
        }

        public abstract void Visualize();

        public abstract void CleanVisual();

        protected virtual void Dispose()
        {
            _container = null;
        }
        
        protected virtual void OnDestroy()
        {
            CleanVisual();
            Dispose();
        }
    }
}