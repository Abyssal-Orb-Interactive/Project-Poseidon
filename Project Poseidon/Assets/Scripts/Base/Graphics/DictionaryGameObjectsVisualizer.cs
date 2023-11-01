using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Base.Graphics
{
    public abstract class DictionaryGameObjectsVisualizer<T> : UnityEntityVisualizer<T> where T : Dictionary<Vector2Int, GameObject>
    {
        public override void CleanVisual()
        {
            if(Visual == null) return;
            
            foreach (var key in Visual.Keys.ToList())
            {
                Visual[key].SetActive(false);
                Destroy(Visual[key]);
                Visual.Remove(key);
            }

            Visual = null;
        }
    }
}