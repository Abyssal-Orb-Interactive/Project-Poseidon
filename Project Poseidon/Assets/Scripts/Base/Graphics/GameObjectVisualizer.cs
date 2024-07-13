using UnityEngine;

namespace Base.Graphics
{
    public abstract class GameObjectVisualizer : UnityEntityVisualizer<GameObject>
    {
        public override void CleanVisual()
        {
            Visual.SetActive(false);
            Destroy(Visual);
            Visual = null;
        }
        
    }
}