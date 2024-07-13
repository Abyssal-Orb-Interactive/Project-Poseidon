using System.Collections.Generic;
using UnityEngine;

namespace Base.Graphics
{
    public abstract class CollectionGameObjectsVisualizer<T> : UnityEntityVisualizer<T> where T : IEnumerable<GameObject>
    {

        public override void CleanVisual()
        {
            if (Visual == null) return;

            foreach (var visual in Visual)
            {
                visual.SetActive(false);
                Destroy(visual);
            }

            Visual = default;
        }
    }
}