using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Input
{
    public class ShootHandler
    {
        private PlayerActions _actions;
        private Vector2Int? _coord;
        private Camera _camera;
        
        public ShootHandler(PlayerActions actions, Camera camera)
        {
            _actions = actions ?? throw new ArgumentNullException(nameof(actions));
            _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
            _coord = null;
            
            Enable();
        }

        public Vector2Int GetCoord()
        {
            return (Vector2Int)_coord;
        }

        public void Enable()
        {
            SubscribeToShoot();
        }

        public void Disable()
        {
            UnsubscribeToShoot();
        }
        private void OnShootPerformed()
        {
            var mouseScreenPos = Mouse.current.position.ReadValue();
            _coord = MousePositionCalculator.CalculateMousePosition(mouseScreenPos, _camera);
        }

        private void SubscribeToShoot()
        {
            _actions.Base.Shoot.performed += _ => OnShootPerformed();
        }

        private void UnsubscribeToShoot()
        {
            _actions.Base.Shoot.performed -= _ => OnShootPerformed();
        }

        public void Destroy()
        {
            Disable();
            _coord = null;
            _actions = null;
            _camera = null;
        }
    }
}
