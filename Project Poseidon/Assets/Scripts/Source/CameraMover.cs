using UnityEngine;

namespace Source
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _activeTarget;
        [SerializeField] private Vector3 _cameraOffset;
        [SerializeField] private float _smoothingSpeed = 5f;

        public void Move()
        {
            if (_activeTarget == null) return;
            
            var targetPosition = _activeTarget.position + _cameraOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothingSpeed * Time.deltaTime);
        }

        public void ChangeMovingTarget(Transform target)
        {
            _activeTarget = target;
        }
    }
}